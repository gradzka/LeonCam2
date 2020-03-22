import { BehaviorSubject } from 'rxjs';
import { handleResponse } from '../authorization/ResponseHandler';

const currentUser = new BehaviorSubject(JSON.parse(localStorage.getItem('currentUser')));

export const authenticationService = {
    login,
    logout,
    register,
    forgotPassword,
    currentUser: currentUser.asObservable(),
    get currentUserValue() {
        return currentUser.value
    }
};

function login(username, password) {
    const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ username, password })
    };

    return fetch(`users/authenticate`, requestOptions)
        .then(handleResponse)
        .then(user => {
            // Store user details and jwt token in local storage to keep user logged in between page refreshes
            localStorage.setItem('currentUser', JSON.stringify(user));
            currentUser.next(user);

            return user;
        });
}

function register(username, password, repeatedPassword) {
    const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ username, password, repeatedPassword })
    };

    return fetch(`users/register`, requestOptions)
        .then(handleResponse)
        .then(data => { return data; });
}

function forgotPassword(username) {
    const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ username })
    };

    return fetch(`users/forgotpassword`, requestOptions)
        .then(handleResponse)
        .then(data => { return data; });
}

function logout() {
    // Remove user from local storage to log user out
    localStorage.removeItem('currentUser');
    currentUser.next(null);
}