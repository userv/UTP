from functools import wraps


def authenticated(session):
    def deco_func(func):
        @wraps(func)
        def wrapper(*args, **kwargs):
            if session.get('user'):
                return func(*args, **kwargs)
            else:
                return f"Invalid session", 401
        return wrapper

    return deco_func
