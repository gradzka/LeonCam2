import { handleResponse } from '../authorization/ResponseHandler';
import { getAuthorizationHeader } from '../authorization/AuthorizationHeader';

export const userService = {
    changeUsername,
    changePassword,
    resetAccount,
    deleteAccount
};

function changeUsername(newUsername, password) {
    const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json', ...getAuthorizationHeader() },
        body: JSON.stringify({ newUsername, password })
    };

    return fetch(`users/changeUsername`, requestOptions)
        .then(handleResponse)
        .then(data => { return data; });
}

function changePassword(oldPassword, newPassword, confirmNewPassword) {
    const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json', ...getAuthorizationHeader() },
        body: JSON.stringify({ oldPassword, newPassword, confirmNewPassword })
    };

    return fetch(`users/changePassword`, requestOptions)
        .then(handleResponse)
        .then(data => { return data; });
}

function resetAccount(password) {
    const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json', ...getAuthorizationHeader() },
        body: JSON.stringify({ password })
    };

    return fetch(`users/resetAccount`, requestOptions)
        .then(handleResponse)
        .then(data => { return data; });
}

function deleteAccount(password) {
    const requestOptions = {
        method: 'POST',
        headers: { 'Content-Type': 'application/json', ...getAuthorizationHeader() },
        body: JSON.stringify({ password })
    };

    return fetch(`users/deleteAccount`, requestOptions)
        .then(handleResponse)
        .then(data => { return data; });
}