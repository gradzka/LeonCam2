import React, { Component } from 'react';
import { InputBox } from '../Shared/InputBox';
import { PasswordBox } from '../Shared/PasswordBox';
import { authenticationService } from '../../services/AuthenticationService';
import { Popover, PopoverBody } from 'reactstrap';

export class Login extends Component {
    constructor() {
        super();
        this.state = {
            name: '',
            pass: '',
            error: '',
            isError: false,
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
            isError: false
        });
    }

    login(event) {
        this.setState({
            isSubmitting: true,
            isError: false
        });
        authenticationService.login(this.state.name, this.state.pass).then(
            user => {
                const { from } = this.props.location.state || {
                    from: { pathname: "/dashboard" }
                };
                this.props.history.push(from);
            },
            error => {
                this.setState({
                    isSubmitting: false,
                    error: error.message,
                    isError: document.activeElement === document.getElementById('signIn')
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
                    <InputBox id="name" placeholder="Username" value={this.state["name"]} onChange={this.handleInputChange} />
                    <PasswordBox id="pass" placeholder="Password" value={this.state["pass"]} onChange={this.handleInputChange} />
                    <div className="button-container">
                        <button id='signIn' disabled={this.state.isSubmitting} onBlur={this.hidePopover}><span>Sign in</span></button>
                    </div>

                    <Popover className="popover-error" placement='top' isOpen={this.state.isError} target='signIn'>
                        <PopoverBody>{this.state.error}</PopoverBody>
                    </Popover>
                </form>
            </div>
        );
    }
}