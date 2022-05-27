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
    RoomValidationError, MessageValidationError
from chat.database import db_session, init_db, Base
from chat.models import User, Room, Message


clients = {}
ROOMS = defaultdict(set)


app = Flask(__name__)
# app.json_encoder = ImprovedEncoder
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
    return redirect(url_for("login"))


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
        else:
            response_str = render_template("invalid_login.html", errors=[f"Invalid password for user {username}."])
    else:
        response_str = render_template("invalid_login.html", errors=[f"Unknown user {username}"])
    response = make_response(response_str)
    if session.get('user') is None:
        response.status_code = 401
    return response


@app.route("/logout")
def logout():
    session.pop('user', None)
    return redirect(url_for('login'))


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
            return redirect(url_for('chat'))
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
    room_id = json_data['room_id']
    created_at = json_data['created_at']
    # room = Room.query.filter(Room.name == 'general').first()
    if msg == 'open':
        clients[user_id] = ws
    if msg == 'join':
        join_room(room_id, user_id)
    if msg == 'leave':
        leave_room(room_id, user_id)
        # ws.send(json.dumps({'message': 'connected', 'user_id': user_id, 'username': username, 'text': 'connected'}))
    # if msg == 'ping':
    #     ws.send(json.dumps({'message': 'pong'}))
    try:
        if msg == 'message':
            save_message(room_id, user_id, text, created_at)
    except Exception as e:
        pass

    for client_id in ROOMS[room_id]:
        clients[client_id].send(data)


def join_room(room_id, user_id):
    ROOMS[room_id].add(user_id)


def leave_room(room_id, user_id):
    ROOMS[room_id].discard(user_id)


def create_room(room_name, user_id):
    from chat.models import Room
    room = Room(name=room_name, created_by=user_id)
    db_session.add(room)
    db_session.commit()
    return room


def save_message(room_id, user_id, text, created_at):
    message = Message(room_id=room_id, user_id=user_id, content=text, created_at=created_at)
    db_session.add(message)
    db_session.commit()
    return message


if __name__ == "__main__":
    app.run(host="0.0.0.0", debug=True)
    # sock.run(app, host="0.0.0.0", debug=True)
