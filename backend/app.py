import json
from datetime import datetime
from flask import Flask, request, render_template, make_response, \
    session, redirect, \
    url_for, jsonify
from flask_session import Session
# from flask_socketio import SocketIO, emit
from flask_sock import Sock
from json import JSONEncoder

from sqlalchemy.exc import NoResultFound

from chat.authentication import authenticated
from chat.exceptions import PasswordValidationError, \
    EmailValidationError, \
    PostValidationError
from chat.database import db_session, init_db, Base
from chat.models import User, Post

POSTS_PER_PAGE = 1
clients = {}


class ImprovedEncoder(JSONEncoder):
    def default(self, obj):
        if isinstance(obj, Base):
            dict_repr = {}
            for column in obj.__table__.columns:
                value = getattr(obj, column.name)
                dict_repr[column.name] = value
            return dict_repr
        elif isinstance(obj, datetime):
            return obj.strftime("%Y-%m-%d %H:%M:%S")
        else:
            return super(ImprovedEncoder, self).default(obj)


app = Flask(__name__)
app.json_encoder = ImprovedEncoder
app.config['SESSION_TYPE'] = 'filesystem'
app.config['SESSION_FILE_DIR'] = 'sessions'
app.config['SESSION_FILE_MODE'] = 0o640
Session(app)
app.secret_key = 'mega_secret'
sock = Sock(app)

init_db()


@app.teardown_appcontext
def shutdown_db_session(exception=None):
    db_session.remove()


@app.route('/')
def welcome():
    return "<p>Welcome to the course</p>"


@app.route("/register", methods=['POST'])
def signup():
    username = request.form.get('username')
    email = request.form.get('email')
    password = request.form.get('password')
    password_verification = request.form.get('password_verification')
    full_name = request.form.get('full_name')
    try:
        new_user = User(username, password, password_verification, email, full_name)
        db_session.add(new_user)
        db_session.commit()
    except PasswordValidationError as exc:
        return f"{exc}", 400
    except EmailValidationError as exc:
        return f"{exc}", 400
    return '<p>Registration successful. Proceed to login <a href="/login.html">here</a></p>'


@app.route("/login", methods=['POST'])
def login():
    username = request.form.get('username')
    password = request.form.get('password')
    user = User.query.filter(User.name == username).first()
    if user:
        if user.check_password(password):
            session['user'] = user
            session['user_id'] = user.id
            response_str = redirect(url_for("user", user_id=user.id))
        else:
            response_str = render_template("invalid_login.html", errors=[f"Invalid password for user {username}."])
    else:
        response_str = render_template("invalid_login.html", errors=[f"Unknown user {username}"])
    response = make_response(response_str)
    if session.get('user') is None:
        response.status_code = 401
    return response


@app.route("/post", methods=['GET', 'POST'])
@authenticated(session)
def post():
    if request.method == 'GET':
        return render_template("post_form.html")
    if request.method == 'POST':
        post_obj = request.json
        if post_obj is None:
            title = request.form.get('title')
            content = request.form.get('content')
        else:
            title = post_obj['title']
            content = post_obj['content']
        author = session.get("user")
        try:
            new_post = Post(title, content, author.id)
            db_session.add(new_post)
            db_session.commit()
            return redirect(url_for("user", user_id=author.id))
        except PostValidationError as exc:
            if post_obj is None:
                return render_template("post_form.html", title=title, content=content, error=str(exc))
            else:
                return {'error': str(exc)}


@app.route("/user/<user_id>", methods=['GET', 'POST'])
def user(user_id):
    if session.get('user') is not None:
        logged_in = True
    else:
        logged_in = False
    try:
        user = User.query.filter(User.id == user_id).one()
    except NoResultFound:
        return f"No user with ID {user_id} exists!", 404
    posts_count = Post.query.filter(Post.author == user_id).count()
    pages_count = int(posts_count / POSTS_PER_PAGE)
    current_page = request.args.get('page', 0)
    pages = range(0, pages_count)
    frmt = request.args.get('format', 'html')
    if len(pages) > 20:
        pages = pages[:3] + '|' + pages[page:3] + '|' + pages[-3:]
    posts = Post.query.filter(Post.author == user_id).limit(POSTS_PER_PAGE).offset(current_page * POSTS_PER_PAGE).all()
    if frmt == 'json':
        return {'posts': posts,
                'pages': list(pages),
                'current_page': current_page,
                'username': user.name,
                'logged_in': logged_in}
    else:
        return render_template("post_list.html", posts=posts, pages=pages, current_page=current_page,
                               username=user.name, logged_in=logged_in)


@app.route("/echo")
def echo():
    return render_template("echo.html", username=session.get('user').name, user_id=session.get('user').id)


# @socketio.on('message', namespace='/echo')
# def handle_message(data):
#     print('received message: ' + data)
#
# @socketio.on('my event', namespace='/echo')
# def handle_message(data):
#     print('received message: ' + data)


@sock.route("/api")
def ws_api(ws):
    while True:
        # ws.send("Hello World!")
        #  client = ws.environ.get('wsgi.websocket')
        data = ws.receive()
        json_data = json.loads(data)
        msg = json_data['message']
        user_id = json_data['user_id']
        username = json_data['username']
        text = json_data['text']
        if msg == 'open':
            clients[user_id] = ws
        if msg == 'ping':
            ws.send(json.dumps({'message': 'pong'}))
        for client_id in clients:
            if client_id != user_id and msg == 'message':
                clients[client_id].send(data)


if __name__ == "__main__":
    app.run(host="0.0.0.0", debug=True)
    # sock.run(app, host="0.0.0.0", debug=True)
