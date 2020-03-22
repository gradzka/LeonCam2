﻿import React, { Component } from 'react';
import { InputBox } from '../Shared/InputBox';
import { PasswordBox } from '../Shared/PasswordBox';
import { authenticationService } from '../../services/AuthenticationService';
import { Popover, PopoverBody } from 'reactstrap';

export class Login extends Component {
    constructor() {
        super();
        this.state = {
            username: '',
            password: '',
            popoverMessage: '',
            popoverIsOpen: false,
            isSubmitting: false
        }

        this.handleInputChange = this.handleInputChange.bind(this);
        this.hidePopover = this.hidePopover.bind(this);
    }

    handleInputChange(inputId, value) {
        this.setState({
                [inputId]: value
            });
    }

    hidePopover() {
        this.setState({
            popoverIsOpen: false
        });
    }

    login(event) {
        this.setState({
            isSubmitting: true,
            popoverIsOpen: false
        });
        authenticationService.login(this.state.username, this.state.password).then(
            user => {
                const { from } = this.props.location.state || {
                    from: { pathname: "/dashboard" }
                };
                this.props.history.push(from);
            },
            error => {
                this.setState({
                    isSubmitting: false,
                    popoverMessage: error === "Unexpected error" ? "Sign-In Error" : error,
                    popoverIsOpen: document.activeElement === document.getElementById('signIn')
                });
            }
        )
        event.preventDefault();
    }

    render() {
        return (
            <div className="card">
                <h1 className="title">Login</h1>
                <form onSubmit={this.login.bind(this)}>
                    <InputBox id="username" type="text" placeholder="Username" value={this.state["username"]} onChange={this.handleInputChange} />
                    <PasswordBox id="password" placeholder="Password" value={this.state["password"]} onChange={this.handleInputChange} />
                    <div className="button-container">
                        <button id='signIn' disabled={this.state.isSubmitting} onBlur={this.hidePopover}><span>Sign in</span></button>
                    </div>

                    <Popover className="popover-error" placement='top' isOpen={this.state.popoverIsOpen} target='signIn'>
                        <PopoverBody>{this.state.popoverMessage}</PopoverBody>
                    </Popover>
                </form>
            </div>
        );
    }
}