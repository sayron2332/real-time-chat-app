const API_URL = 'https://sayron-chat.azurewebsites.net';

document.addEventListener('DOMContentLoaded', function () {
    checkAuth();

    const registerForm = document.getElementById('registerForm');
    const loginForm = document.getElementById('loginForm');

    if (registerForm) {
        registerForm.addEventListener('submit', handleRegister);
    }
    if (loginForm) {
        loginForm.addEventListener('submit', handleLogin);
    }
});

async function handleRegister(e) {
    e.preventDefault();
    clearAllErrors();

    const formData = {
        name: document.getElementById('regName').value,
        surname: document.getElementById('regSurname').value,
        email: document.getElementById('regEmail').value,
        password: document.getElementById('regPassword').value,
        confirmPassword: document.getElementById('regConfirmPassword').value,
        appRoleId: 1
    };

    try {
        const response = await fetch(`${API_URL}/api/auth/register`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(formData)
        });

        const responseText = await response.text();
        const authResponse = JSON.parse(responseText);

        if (response.ok && authResponse.success) {
            alert('Реєстрація успішна! Тепер увійдіть у акаунт.');
            window.location.href = 'login.html';
        } else {
            showBackendErrors(authResponse, 'register');
        }
    } catch (error) {
        alert('Мережева помилка: ' + error.message);
    }
}

async function handleLogin(e) {
    e.preventDefault();
    clearAllErrors();

    const formData = {
        email: document.getElementById('loginEmail').value,
        password: document.getElementById('loginPassword').value
    };

    try {
        const response = await fetch(`${API_URL}/api/auth/login`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(formData)
        });

        const responseText = await response.text();
        const authResponse = JSON.parse(responseText);

        if (response.ok && authResponse.success) {
            saveAuthData(authResponse, authResponse.username || formData.email);
            window.location.href = 'index.html';
        } else {
            showBackendErrors(authResponse, 'login');
        }
    } catch (error) {
        alert('Мережева помилка: ' + error.message);
    }
}

function showBackendErrors(authResponse, formType) {
    clearAllErrors();

    if (!authResponse) {
        showGeneralError('Невідома помилка', formType);
        return;
    }


    if (authResponse.errors) {
        Object.keys(authResponse.errors).forEach(field => {
            const errorMessages = authResponse.errors[field];
            if (errorMessages && errorMessages.length > 0) {
                showFieldError(field, errorMessages[0], formType);
            }
        });
    }

    else if (authResponse.message) {
        if (formType === 'login') {
            showGeneralError(authResponse.message, formType);
        } else {
            showFieldErrorsFromMessage(authResponse.message);
        }
    }
}

function showFieldError(fieldName, errorMessage, formType) {
    const fieldMap = {
        'Name': 'regName',
        'Surname': 'regSurname',
        'Email': formType === 'login' ? 'loginEmail' : 'regEmail',
        'Password': formType === 'login' ? 'loginPassword' : 'regPassword',
        'ConfirmPassword': 'regConfirmPassword'
    };

    const fieldId = fieldMap[fieldName];
    if (!fieldId) return;

    const inputElement = document.getElementById(fieldId);
    const errorElement = document.getElementById(fieldId + 'Error');

    if (inputElement && errorElement) {
        inputElement.classList.add('is-invalid');
        errorElement.textContent = errorMessage;
        errorElement.style.display = 'block';
    }
}

function showGeneralError(message, formType) {
    if (formType === 'login') {
        const generalErrorElement = document.getElementById('loginGeneralError');
        if (generalErrorElement) {
            generalErrorElement.textContent = message;
            generalErrorElement.classList.remove('d-none');
        } else {

            alert(message);
        }
    } else {

        alert(message);
    }
}

function clearAllErrors() {

    const errorElements = document.querySelectorAll('.error-message, .invalid-feedback');
    errorElements.forEach(element => {
        element.style.display = 'none';
        element.textContent = '';
    });

    const inputs = document.querySelectorAll('.is-invalid');
    inputs.forEach(input => {
        input.classList.remove('is-invalid');
    });


    const generalError = document.getElementById('loginGeneralError');
    if (generalError) {
        generalError.classList.add('d-none');
        generalError.textContent = '';
    }
}

function showFieldErrorsFromMessage(message) {
    if (message.toLowerCase().includes('email')) {
        showFieldError('Email', message, 'register');
    } else if (message.toLowerCase().includes('password')) {
        showFieldError('Password', message, 'register');
    } else {
        alert(message);
    }
}

function saveAuthData(authData, username) {
    localStorage.setItem('accessToken', authData.accessToken);
    localStorage.setItem('username', username);
}

function checkAuth() {
    const token = localStorage.getItem('accessToken');
    const username = localStorage.getItem('username');
    const currentPage = window.location.pathname.split('/').pop();

    if (token && username) {
        if (currentPage === 'login.html' || currentPage === 'register.html') {
            window.location.href = 'index.html';
        }

        const userDisplay = document.getElementById('currentUserDisplay');
        if (userDisplay) {
            userDisplay.textContent = `Вітаємо, ${username}!`;
        }
    } else {
        if (currentPage === 'index.html') {
            window.location.href = 'login.html';
        }
    }
}

function logout() {
    localStorage.removeItem('accessToken');
    localStorage.removeItem('username');
    window.location.href = 'login.html';
}

window.logout = logout;