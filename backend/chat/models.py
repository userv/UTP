import re
from datetime import datetime
from hashlib import sha256
from sqlalchemy import Column, Integer, String, Text, DateTime, ForeignKey
from sqlalchemy.orm import relationship
from chat.database import Base
from chat.exceptions import PasswordValidationError, \
    EmailValidationError, \
    UserValidationError, \
    PostValidationError


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
