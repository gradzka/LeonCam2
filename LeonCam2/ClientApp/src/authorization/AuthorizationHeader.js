import { authenticationService } from '../services/AuthenticationService';

export function getAuthorizationHeader() {
    // return authorization header with jwt token
    const currentUser = authenticationService.currentUserValue;

    if (currentUser && currentUser.token) {
        return {
            Authorization: `Bearer ${currentUser.token}`
        };
    }
    else {
        return {};
    }
}