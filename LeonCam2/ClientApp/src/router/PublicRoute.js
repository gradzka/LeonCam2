import React from 'react';
import { Route, Redirect } from 'react-router-dom';

import { authenticationService } from '../services/AuthenticationService';

export const PublicRoute = ({ component: Component, ...rest }) => (
    <Route {...rest} render={props => {
        const currentUser = authenticationService.currentUserValue;

        if (currentUser) {
            // Dashboard page redirection
            return <Redirect to={{ pathname: '/dashboard' }} />
        }

        return <Component {...props} />
    }} />
)