import { BehaviorSubject } from 'rxjs';
import { handleResponse } from '../authorization/ResponseHandler';

const currentUser = new BehaviorSubject(JSON.parse(localStorage.getItem('currentUser')));

export const authenticationService = {
    getLeadingQuestion,
    login,
    logout,
    register,
    checkAnswer,
    currentUser: currentUser.asObservable(),
    get currentUserValue() {
        return currentUser.value
    }
};

function getLeadingQuestion(username) {
    const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify( username )
    };

    return fetch(`users/GetLeadingQuestion`, requestOptions)
        .then(handleResponse)
        .then(data => { return data; });
}

function login(username, password) {
    const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ username, password })
    };

    return fetch(`users/login`, requestOptions)
        .then(handleResponse)
        .then(user => {
            return setCurrentUser(user);
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

function checkAnswer(username, answer) {
    const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ username, answer })
    };

    return fetch(`users/checkanswer`, requestOptions)
        .then(handleResponse)
        .then(user => {
            return setCurrentUser(user);
        });}

function logout() {
    // Remove user from local storage to log user out
    localStorage.removeItem('currentUser');
    currentUser.next(null);
}

function setCurrentUser(user) {
    // Store user details and jwt token in local storage to keep user logged in between page refreshes
    localStorage.setItem('currentUser', JSON.stringify(user));
    currentUser.next(user);

    return user;
}