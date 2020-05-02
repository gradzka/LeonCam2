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
        this.state = {
            username: ''
        }

        // Redirect to dashboard if user already logged in
        if (authenticationService.currentUserValue) {
            this.props.history.push('/dashboard');
        }

        this.onUsernameChanged = this.onUsernameChanged.bind(this);
    }

    onUsernameChanged(value) {
        this.setState({ username: value });
    }

    render() {
        return (
            <div className="card-container margin-left-md-55">
                <Register />
                <ForgotPassword username={this.state.username} location={this.props.location} history={this.props.history} />
                <div className="card first"></div>
                <Login location={this.props.location} history={this.props.history} onUsernameChanged={this.onUsernameChanged}/>
            </div>
        );
    }
}
