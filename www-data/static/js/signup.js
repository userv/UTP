function displayError(error) {
    error_div = document.getElementById('error');
    error_div.innerHTML = error;
    error_div.style.display = 'block';
}

function signupValidation() {
    username = document.getElementsByName('username')[0].value;
    password = document.getElementsByName('password')[0].value;
    passowrd_verification = document.getElementsByName('password_verification')[0].value;
    full_name = document.getElementsByName('full_name')[0].value;
    email = document.getElementsByName('email')[0].value;
    if (password != passowrd_verification) {
        displayError('Паролите не съвпадат');
        return false;
    }
    if (username == '') {
        displayError('Потребителското име не може да бъде празно.');
        return false;
    }
    if (password.length < 8) {
        displayError('Паролата трябва да съдържа поне 8 символа.');
        return false;
    }
    if (!password.match('[a-z]') || !password.match('[0-9]') || !password.match('[A-Z]') || !password.match('[\!\?\@\#\$\%\^\&\*\+\_\-]')) {
        displayError('Паролата трябва да съдържа цифра, главна и малкa буква, поне един специален символ.');
        return false;
    }
    if (!email.match('[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Z|a-z]{2,}$')) {
        displayError('Невалиден e-mail адрес.');
        return false;
    }
    return true;
}
