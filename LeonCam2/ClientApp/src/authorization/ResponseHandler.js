import { authenticationService } from '../services/AuthenticationService';

export function handleResponse(response) {
    return response.text().then(text => {
        try {
            const data = text && JSON.parse(text);
            if (!response.ok) {
                if ([401, 403].indexOf(response.status) !== -1) {
                    // auto logout if [401] Unauthorized or [403] Forbidden response returned from api
                    authenticationService.logout();
                    window.location.reload(true);
                }

                const error = (data && data.message) || response.statusText;
                return Promise.reject(error);
            }

            return data;
        }
        catch (error) {
            return Promise.reject(new Error("Unexpected error"));
        }
    });
}