import React, { Component } from 'react';
import { InputBox } from '../Shared/InputBox';
import { pushCard, popCard } from '../Shared/Card';
import { PasswordBox } from '../Shared/PasswordBox';
import { authenticationService } from '../../services/AuthenticationService';
import { Popover, PopoverBody } from 'reactstrap';

export class Register extends Component {
    constructor() {
        super();
        this.state = {
            regname: '',
            regpass: '',
            reregpass: '',
            popoverMessage: '',
            popoverClass: '',
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

    componentDidMount() {
        document.body.addEventListener('click', this.hidePopover);
    }

    componentWillUnmount() {
        document.body.removeEventListener('click', this.hidePopover);
    }

    hidePopover() {
        this.setState({
            popoverIsOpen: false
        });
    }

    register(event) {
        this.setState({
            isSubmitting: true,
            popoverIsOpen: false
        });

        if (this.state.regpass !== this.state.reregpass) {
            this.setState({
                isSubmitting: false,
                popoverClass: 'popover-error-reversed',
                popoverMessage: "Passwords must be the same",
                popoverIsOpen: true
            });
        }
        else {
            authenticationService.register(this.state.regname, this.state.regpass, this.state.reregpass).then(
                () => {
                    this.setState({
                        isSubmitting: false,
                        popoverClass: 'popover-success-reversed',
                        popoverMessage: 'Registered',
                        popoverIsOpen: document.activeElement === document.getElementById('signUp'),
                        regpass: '',
                        reregpass: ''
                    });

                    document.getElementById('regpass').dispatchEvent(new Event("keyup"));
                },
                error => {
                    this.setState({
                        isSubmitting: false,
                        popoverClass: 'popover-error-reversed',
                        popoverMessage: error === "Unexpected error" ? "Sign-Up Error" : error,
                        popoverIsOpen: true
                    });
                }

            )
        }
        event.preventDefault();
    }


    render() {
        return (<div className="card alt hidden">
            <div className="toggle" tabIndex="0" onClick={pushCard}>Register</div>
            <h1 className="title">Register
                <div className="close" tabIndex="0" onClick={popCard.bind(this, 'Register')}></div>
            </h1>
            <form onSubmit={this.register.bind(this)}>
                <InputBox id="regname" type="text" placeholder="Username" className="alt hidden" value={this.state["regname"]} onChange={this.handleInputChange} autoComplete="new-password"/>
                <PasswordBox id="regpass" placeholder="Password" className="alt hidden" value={this.state["regpass"]} onChange={this.handleInputChange} withPasswordStrength={true} autoComplete="new-password"/>
                <PasswordBox id="reregpass" placeholder="Repeat Password" className="alt hidden" value={this.state["reregpass"]} onChange={this.handleInputChange} autocomplete="new-password" />
                <div className="button-container">
                    <button id='signUp' disabled={this.state.isSubmitting}><span>Sign up</span></button>
                </div>

                <Popover className={this.state.popoverClass} placement='top' isOpen={this.state.popoverIsOpen} target='signUp'>
                    <PopoverBody>{this.state.popoverMessage}</PopoverBody>
                </Popover>
            </form>
        </div>);
    }
}