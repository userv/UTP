class PasswordValidationError(Exception):
    def __init__(self, message):
        self.message = message

    def __str__(self):
        return f"Password validation failed: {self.message}"

    def __repr__(self):
        return f"<{self.__class__.__name__}: {self.message}>"


class EmailValidationError(Exception):
    def __init__(self, message):
        self.message = message

    def __str__(self):
        return f"E-mail validation failed: {self.message}"

    def __repr__(self):
        return f"<{self.__class__.__name__}: {self.message}>"

class UserValidationError(Exception):
    def __init__(self, message):
        self.message = message

    def __str__(self):
        return f"User validation failed: {self.message}"

    def __repr__(self):
        return f"<{self.__class__.__name__}: {self.message}>"

class PostValidationError(Exception):
    def __init__(self, message):
        self.message = message

    def __str__(self):
        return self.message

    def __repr__(self):
        return f"<{self.__class__.__name__}: {self.message}>"


