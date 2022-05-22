import json
from collections import defaultdict
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
    PostValidationError, RoomValidationError, MessageValidationError
from chat.database import db_session, init_db, Base
from chat.models import User, Post, Room

POSTS_PER_PAGE = 1
clients = {}
ROOMS = defaultdict(list)


# ROOMS = ['general', 'games', 'programming', 'news']


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
app.config['SOCK_SERVER_OPTIONS'] = {'ping_interval': 25}
Session(app)
app.secret_key = 'mega_secret'
sock = Sock(app)

init_db()


@app.teardown_appcontext
def shutdown_db_session(exception=None):
    db_session.remove()


@app.route('/')
def index():
    return render_template('index.html')


@app.route("/main")
def main():
    return render_template("main.html", username=session.get('user').name, user_id=session.get('user').id)


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


@app.route("/login", methods=['GET', 'POST'])
def login():
    if request.method == 'GET':
        return render_template("login.html")
    username = request.form.get('username')
    password = request.form.get('password')
    user = User.query.filter(User.name == username).first()
    if user:
        if user.check_password(password):
            session['user'] = user
            session['user_id'] = user.id
            response_str = redirect(url_for("chat", user_id=user.id))
        # response_str = redirect(url_for("user", user_id=user.id))
        else:
            response_str = render_template("invalid_login.html", errors=[f"Invalid password for user {username}."])
    else:
        response_str = render_template("invalid_login.html", errors=[f"Unknown user {username}"])
    response = make_response(response_str)
    if session.get('user') is None:
        response.status_code = 401
    return response


# @app.route("/post", methods=['GET', 'POST'])
# @authenticated(session)
# def post():
#     if request.method == 'GET':
#         return render_template("bak/post_form.html")
#     if request.method == 'POST':
#         post_obj = request.json
#         if post_obj is None:
#             title = request.form.get('title')
#             content = request.form.get('content')
#         else:
#             title = post_obj['title']
#             content = post_obj['content']
#         author = session.get("user")
#         try:
#             new_post = Post(title, content, author.id)
#             db_session.add(new_post)
#             db_session.commit()
#             return redirect(url_for("user", user_id=author.id))
#         except PostValidationError as exc:
#             if post_obj is None:
#                 return render_template("bak/post_form.html", title=title, content=content, error=str(exc))
#             else:
#                 return {'error': str(exc)}


# @app.route("/user/<user_id>", methods=['GET', 'POST'])
# def user(user_id):
#     if session.get('user') is not None:
#         logged_in = True
#     else:
#         logged_in = False
#     try:
#         user = User.query.filter(User.id == user_id).one()
#     except NoResultFound:
#         return f"No user with ID {user_id} exists!", 404
#     posts_count = Post.query.filter(Post.author == user_id).count()
#     pages_count = int(posts_count / POSTS_PER_PAGE)
#     current_page = request.args.get('page', 0)
#     pages = range(0, pages_count)
#     frmt = request.args.get('format', 'html')
#     if len(pages) > 20:
#         pages = pages[:3] + '|' + pages[pages:3] + '|' + pages[-3:]
#     posts = Post.query.filter(Post.author == user_id).limit(POSTS_PER_PAGE).offset(current_page * POSTS_PER_PAGE).all()
#     if frmt == 'json':
#         return {'posts': posts,
#                 'pages': list(pages),
#                 'current_page': current_page,
#                 'username': user.name,
#                 'logged_in': logged_in}
#     else:
#         return render_template("bak/post_list.html", posts=posts, pages=pages, current_page=current_page,
#                                username=user.name, logged_in=logged_in)


@app.route("/logout")
def logout():
    session.pop('user', None)
    return redirect(url_for('login'))


@app.route("/echo")
def echo():
    if session.get('user') is None:
        return redirect(url_for('login'))
    else:
        return render_template("echo.html", username=session.get('user').name, user_id=session.get('user').id)


@app.route("/chat")
def chat():
    if session.get('user') is None:
        return redirect(url_for('login'))
    else:
        rooms = Room.query.all()
        return render_template("chat.html", username=session.get('user').name, user_id=session.get('user').id,
                               rooms=rooms)


@app.route("/room", methods=['GET', 'POST'])
def create_room():
    if session.get('user') is None:
        return redirect(url_for('login'))
    if request.method == 'GET':
        return render_template("create_room.html")
    if request.method == 'POST':
        room_name = request.form.get('room-name')
        user_id = session.get('user').id
        try:
            new_room = create_room(room_name, user_id)
            return redirect(url_for('echo', room_id=new_room.id))
        except RoomValidationError as exc:
            return {'error': str(exc)}


@sock.route("/api")
def ws_api(ws):
    while True:
        handle_messages(ws)


def handle_messages(ws):
    data = ws.receive()
    json_data = json.loads(data)
    msg = json_data['message']
    user_id = json_data['user_id']
    username = json_data['username']
    text = json_data['text']
    room = Room.query.filter(Room.name == 'general').first()
    if msg == 'open':
        clients[user_id] = ws
        ROOMS[room.id].append(user_id)
        # ws.send(json.dumps({'message': 'connected', 'user_id': user_id, 'username': username, 'text': 'connected'}))
    # if msg == 'ping':
    #     ws.send(json.dumps({'message': 'pong'}))
    for client_id in clients:
        if client_id != user_id and msg == 'message' or msg == 'connected':
            clients[client_id].send(data)
    for room_id in ROOMS:
        for client_id in ROOMS[room_id]:
            clients[client_id].send(data)


def create_room(room_name, user_id):
    from chat.models import Room
    room = Room(name=room_name, created_by=user_id)
    db_session.add(room)
    db_session.commit()
    return room


def save_message(room_id, user_id, content):
    from chat.models import Message
    message = Message(room_id=room_id, user_id=user_id, content=content)
    db_session.add(message)
    db_session.commit()
    return message


if __name__ == "__main__":
    app.run(host="0.0.0.0", debug=True)
    # sock.run(app, host="0.0.0.0", debug=True)
