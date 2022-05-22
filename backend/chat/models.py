import re
from datetime import datetime
from hashlib import sha256
from sqlalchemy import Column, Integer, String, Text, DateTime, ForeignKey
from sqlalchemy.orm import relationship
from chat.database import Base
from chat.exceptions import PasswordValidationError, \
    EmailValidationError, \
    UserValidationError, \
    PostValidationError, \
    RoomValidationError, \
    MessageValidationError


class User(Base):
    __tablename__ = 'users'
    id = Column(Integer, primary_key=True)
    name = Column(String(50), unique=True)
    fullname = Column(String(50))
    email = Column(String(120), unique=True)
    password = Column(String(256), nullable=False)
    posts = relationship("Post", order_by="Post.id")

    def __init__(self, username, password, password_verification, email, full_name):
        self._validate_password(password, password_verification)
        self.name = username
        self.password = sha256(password.encode('utf-8')).hexdigest()
        self.email = email
        self.fullname = full_name

    def _validate_password(self, password, validate_password):
        if password != validate_password:
            raise PasswordValidationError("Passwords don't match")
        if len(password) < 8:
            raise PasswordValidationError("Password too short")
        if not re.search(r'[a-z]{1}', password):
            raise PasswordValidationError("Password must contain lowercase letters")
        if not re.search(r'[A-Z]{1}', password):
            raise PasswordValidationError("Password must contain uppercase letters")
        if not re.search(r'[0-9]{1}', password):
            raise PasswordValidationError("Password must contain digits")
        if not re.search(r'[\!\?\@\#\$\%\^\&\*\+\-\_]{1}', password):
            raise PasswordValidationError("Password must contain special symbols")

    def check_password(self, password):
        return self.password == sha256(password.encode('utf-8')).hexdigest()


class Post(Base):
    __tablename__ = 'posts'
    id = Column(Integer, primary_key=True)
    author = Column(Integer, ForeignKey(User.id))
    title = Column(String(50))
    content = Column(Text)
    submitted_at = Column(DateTime, default=datetime.now())
    user = relationship("User", back_populates="posts")

    def __init__(self, title=None, content=None, user_id=None):
        self._validate_title(title)
        self._validate_content(content)
        if user_id is None:
            return PostValidationError("No author!!!")
        self.title = title
        self.content = content
        self.author = user_id

    def _validate_title(self, title):
        if title is None:
            raise PostValidationError("Post title cannot be empty!")
        if len(title) > 50:
            raise PostValidationError("Title mmust be 50 characters at most.")

    def _validate_content(self, content):
        if content is None:
            raise PostValidationError("Post content cannot be empty!")
        if len(content) > 250:
            raise PostValidationError("Content must be 250 characters at most.")


class Room(Base):
    __tablename__ = 'rooms'
    id = Column(Integer, primary_key=True)
    name = Column(String(50), unique=True)
    created_by = Column(Integer, ForeignKey(User.id))


def __init__(self, name=None, user_id=None):
    self._validate_name(name)
    if user_id is None:
        return RoomValidationError("No room creator id.")
    self.name = name
    self.created_by = user_id


def _validate_name(self, name):
    if name is None:
        raise PostValidationError("Room name cannot be empty!")
    if len(name) > 50:
        raise PostValidationError("Name must be 50 characters at most.")


class Message(Base):
    __tablename__ = 'messages'
    id = Column(Integer, primary_key=True)
    room_id = Column(Integer, ForeignKey(Room.id))
    user_id = Column(Integer, ForeignKey(User.id))
    content = Column(Text)
    created_at = Column(DateTime, default=datetime.now())

    def __init__(self, content=None, user_id=None, room_id=None):
        self._validate_message(content, user_id, room_id)

        self.content = content
        self.user_id = user_id
        self.room_id = room_id

    def _validate_message(self, content, user_id, room_id):
        if user_id is None:
            return MessageValidationError("No user id!")
        if room_id is None:
            return MessageValidationError("No room id!")
        # if content is None:
        #     raise MessageValidationError("Message content cannot be empty!")
        if len(content) > 250:
            raise MessageValidationError("Content must be 250 characters at most.")
