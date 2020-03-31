import React, { Component } from 'react';
import { InputBox } from '../Shared/InputBox';
import { pushCard, popCard } from '../Shared/Card';
import { authenticationService } from '../../services/AuthenticationService';
import { Popover, PopoverBody } from 'reactstrap';

export class ForgotPassword extends Component {
    constructor() {
        super();
        this.state = {
            username: '',
            forgotmail: '',
            popoverMessage: '',
            popoverClass: '',
            popoverIsOpen: false,
            checkUsernamePopoverIsOpen: false,
            isSubmitting: false
        }

        this.handleInputChange = this.handleInputChange.bind(this);
        this.hidePopover = this.hidePopover.bind(this);
    }

    checkUsername(event) {
        event.persist();

        authenticationService.checkUsername(this.props.username).then(       
            data => {
                this.setState({
                    popoverClass: 'popover-success-reversed',
                    popoverMessage: 'Sent',
                    checkUsernamePopoverIsOpen: document.activeElement === document.getElementById('sendLink')
                });

                pushCard(event);
            },
            error => {
                this.setState({
                    popoverClass: 'popover-error-reversed',
                    popoverMessage: error === "Unexpected error" ? "Forgot Password Error" : error,
                    checkUsernamePopoverIsOpen: document.activeElement === document.getElementById('sendLink')
                });
            }
        )
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

    forgotpassword(event) {
        this.setState({
            isSubmitting: true,
            popoverIsOpen: false
        });
        authenticationService.forgotPassword(this.state.forgotmail).then(
            data => {
                this.setState({
                    isSubmitting: false,
                    popoverClass: 'popover-success-reversed',
                    popoverMessage: 'Sent',
                    popoverIsOpen: document.activeElement === document.getElementById('sendLink')
                });
            },
            error => {
                this.setState({
                    isSubmitting: false,
                    popoverClass: 'popover-error-reversed',
                    popoverMessage: error === "Unexpected error" ? "Forgot Password Error" : error,
                    popoverIsOpen: document.activeElement === document.getElementById('sendLink')
                });
            }  
        )
        event.preventDefault();
    }

    render() {
        return (
            <div className="card alt hidden bottom">
                <div id="forgotPasswordLink" className="toggle" tabIndex="0" onClick={this.checkUsername.bind(this)}>Forgot password?</div>
                <h1 className="title">Forgot<br />password?
                        <div className="close" tabIndex="0" onClick={popCard.bind(this, 'Forgot password?')}></div>
                </h1>
                <form onSubmit={this.forgotpassword.bind(this)}>
                    <InputBox id="forgotmail" type="email" placeholder="E-Mail" className="alt hidden" value={this.state["forgotmail"]} onChange={this.handleInputChange} />
                    <div className="button-container">
                        <button id='sendLink' disabled={this.state.isSubmitting} onBlur={this.hidePopover}><span>Send link</span></button>
                    </div>

                    <Popover className={this.state.popoverClass} placement='top' isOpen={this.state.popoverIsOpen} target='sendLink'>
                        <PopoverBody>{this.state.popoverMessage}</PopoverBody>
                    </Popover>

                    <Popover className={this.state.popoverClass} placement='top' isOpen={this.state.checkUsernamePopoverIsOpen} target='forgotPasswordLink'>
                        <PopoverBody>{this.state.popoverMessage}</PopoverBody>
                    </Popover>
                </form>
            </div>
        );
    }
}