import React, { Component } from 'react';
import './Home.css';
import { ForgotPassword } from './ForgotPassword';
import { Login } from './Login';
import { Register } from './Register';
import { authenticationService } from '../../services/AuthenticationService';

export class Home extends Component {
    static displayName = Home.name;

    constructor(props) {
        super(props);

        // Redirect to dashboard if user already logged in
        if (authenticationService.currentUserValue) {
            this.props.history.push('/dashboard');
        }
    }

    render() {
        return (
            <div className="card-container">
                <Register/>
                <ForgotPassword />
                <div className="card first"></div>
                <Login />
            </div>
        );
    }
}
