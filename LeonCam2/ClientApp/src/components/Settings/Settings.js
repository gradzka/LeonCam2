import React, { Component } from 'react';
import { Popover, PopoverBody } from 'reactstrap';
import { authenticationService } from '../../services/AuthenticationService';
import { userService } from '../../services/UserService';
import { browserHistory } from '../../router/BrowserHistory';
import { InputBox } from '../Shared/InputBox';
import { PasswordBox } from '../Shared/PasswordBox';

import './Settings.css';

export class Settings extends Component {
    constructor(props) {
        super(props);
        this.state = {
            changeUsernameNewUsername: '',
            changeUsernamePassword: '',
            changeUsernamePopoverMessage: '',
            changeUsernamePopoverClass: '',
            changeUsernamePopoverIsOpen: false,
            changeUsernameIsSubmitting: false,

            changePasswordOldPassword: '',
            changePasswordNewPassword: '',
            changePasswordConfirmNewPassword: '',
            changePasswordPopoverMessage: '',
            changePasswordPopoverClass: '',
            changePasswordPopoverIsOpen: false,
            changePasswordIsSubmitting: false,

            resetAccountPassword: '',
            resetAccountPopoverMessage: '',
            resetAccountPopoverClass: '',
            resetAccountPopoverIsOpen: false,
            resetAccountIsSubmitting: false,

            deleteAccountPassword: '',
            deleteAccountPopoverMessage: '',
            deleteAccountPopoverClass: '',
            deleteAccountPopoverIsOpen: false,
            deleteAccountIsSubmitting: false,
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
            changeUsernamePopoverIsOpen: false,
            changePasswordPopoverIsOpen: false,
            resetAccountPopoverIsOpen: false,
            deleteAccountPopoverIsOpen: false
        });
    }

    changeUsername(event) {
        this.setState({
            changeUsernameIsSubmitting: true,
            changeUsernamePopoverIsOpen: false
        });

        userService.changeUsername(this.state.changeUsernameNewUsername, this.state.changeUsernamePassword).then(
                () => {
                    this.setState({
                        changeUsernameIsSubmitting: false,
                        changeUsernamePopoverClass: 'popover-success-reversed',
                        changeUsernamePopoverMessage: 'Success',
                        changeUsernamePopoverIsOpen: document.activeElement === document.getElementById('changeUsername'),
                        changeUsernamePassword: ''
                    });
                },
                error => {
                    this.setState({
                        changeUsernameIsSubmitting: false,
                        changeUsernamePopoverClass: 'popover-error-reversed',
                        changeUsernamePopoverMessage: error === "Unexpected error" ? "Change Username Error" : error,
                        changeUsernamePopoverIsOpen: true
                    });
                }

            )
        
        event.preventDefault();
    }

    changePassword(event) {
        this.setState({
            changePasswordIsSubmitting: true,
            changePasswordPopoverIsOpen: false
        });

        if (this.state.changePasswordNewPassword !== this.state.changePasswordConfirmNewPassword) {
            this.setState({
                changePasswordIsSubmitting: false,
                changePasswordPopoverClass: 'popover-error-reversed',
                changePasswordPopoverMessage: "Passwords must be the same",
                changePasswordPopoverIsOpen: true
            });
        }
        else {
            userService.changePassword(this.state.changePasswordOldPassword, this.state.changePasswordNewPassword, this.state.changePasswordConfirmNewPassword).then(
                () => {
                    this.setState({
                        changePasswordIsSubmitting: false,
                        changePasswordPopoverClass: 'popover-success-reversed',
                        changePasswordPopoverMessage: 'Success',
                        changePasswordPopoverIsOpen: document.activeElement === document.getElementById('changePassword'),
                        changePasswordOldPassword: '',
                        changePasswordNewPassword: '',
                        changePasswordConfirmNewPassword: ''
                    });
                },
                error => {
                    this.setState({
                        changePasswordIsSubmitting: false,
                        changePasswordPopoverClass: 'popover-error-reversed',
                        changePasswordPopoverMessage: error === "Unexpected error" ? "Change Password Error" : error,
                        changePasswordPopoverIsOpen: true
                    });
                }
            )
        }

        event.preventDefault();
    }

    resetAccount(event) {
        this.setState({
            resetAccountIsSubmitting: true,
            resetAccountPopoverIsOpen: false
        });

        userService.resetAccount(this.state.resetAccountPassword).then(
            () => {
                this.setState({
                    resetAccountIsSubmitting: false,
                    resetAccountPopoverClass: 'popover-success-reversed',
                    resetAccountPopoverMessage: 'Success',
                    resetAccountPopoverIsOpen: document.activeElement === document.getElementById('resetAccount'),
                    resetAccountPassword: ''
                });
            },
            error => {
                this.setState({
                    resetAccountIsSubmitting: false,
                    resetAccountPopoverClass: 'popover-error-reversed',
                    resetAccountPopoverMessage: error === "Unexpected error" ? "Reset Account Error" : error,
                    resetAccountPopoverIsOpen: true
                });
            }

        )

        event.preventDefault();
    }

    deleteAccount(event) {
        this.setState({
            deleteAccountIsSubmitting: true,
            deleteAccountPopoverIsOpen: false
        });

        userService.deleteAccount(this.state.deleteAccountPassword).then(
            () => {
                this.setState({
                    deleteAccountIsSubmitting: false,
                    deleteAccountPopoverClass: 'popover-success-reversed',
                    deleteAccountPopoverMessage: 'Success',
                    deleteAccountPopoverIsOpen: document.activeElement === document.getElementById('deleteAccount'),
                    deleteAccountPassword: ''
                });

                localStorage.removeItem('currentUser');
                authenticationService.currentUser.next(null);
                browserHistory.push('/');
                event.preventDefault();
            },
            error => {
                this.setState({
                    deleteAccountIsSubmitting: false,
                    deleteAccountPopoverClass: 'popover-error-reversed',
                    deleteAccountPopoverMessage: error === "Unexpected error" ? "Delete Account Error" : error,
                    deleteAccountPopoverIsOpen: true
                });
            }
        )

        event.preventDefault();
    }

    render() {
        return (
            <div>
                <h1>Settings</h1>
                <div className="row">
                    <div className="col-md-6 grid-margin stretch-card">
                        <div className="card">
                            <div className="card-body">
                                <h4 className="card-title">Change username</h4>
                                <form onSubmit={this.changeUsername.bind(this)}>
                                    <InputBox id="changeUsernameNewUsername" type="text" placeholder="Username" className='settings' value={this.state["changeUsernameNewUsername"]} onChange={this.handleInputChange} />
                                    <PasswordBox id="changeUsernamePassword" placeholder="Password" className='settings' value={this.state["changeUsernamePassword"]} onChange={this.handleInputChange} autoComplete="new-password" />

                                    <button type="submit" id="changeUsername" className="btn btn-primary" disabled={this.state.changeUsernameIsSubmitting}>Change</button>
                                    <Popover className={this.state.changeUsernamePopoverClass} placement='right' isOpen={this.state.changeUsernamePopoverIsOpen} target='changeUsername'>
                                        <PopoverBody>{this.state.changeUsernamePopoverMessage}</PopoverBody>
                                    </Popover>
                                </form>
                            </div>
                        </div>
                    </div>
                    <div className="col-md-6 grid-margin stretch-card">
                        <div className="card">
                            <div className="card-body">
                                <h4 className="card-title">Change password</h4>
                                <form onSubmit={this.changePassword.bind(this)}>
                                    <PasswordBox id="changePasswordOldPassword" placeholder="Old password" className='settings' value={this.state["changePasswordOldPassword"]} onChange={this.handleInputChange} autoComplete="new-password" />        
                                    <PasswordBox id="changePasswordNewPassword" placeholder="New password" className='settings' value={this.state["changePasswordNewPassword"]} onChange={this.handleInputChange} withPasswordStrength={true} autoComplete="new-password" />
                                    <PasswordBox id="changePasswordConfirmNewPassword" placeholder="Confirm new password" className='settings' value={this.state["changePasswordConfirmNewPassword"]} onChange={this.handleInputChange} autoComplete="new-password" />

                                    <button type="submit" id="changePassword" className="btn btn-primary" disabled={this.state.changePasswordIsSubmitting}>Change</button>
                                    <Popover className={this.state.changePasswordPopoverClass} placement='right' isOpen={this.state.changePasswordPopoverIsOpen} target='changePassword'>
                                        <PopoverBody>{this.state.changePasswordPopoverMessage}</PopoverBody>
                                    </Popover>
                                </form>
                            </div>
                        </div>
                    </div>
                </div>
                <div className="row">
                    <div className="col-md-6 grid-margin stretch-card">
                        <div className="card">
                            <div className="card-body">
                                <h4 className="card-title">Reset account</h4>
                                <p className="card-description">All saved cameras will be removed irreversibly!</p>
                                <form onSubmit={this.resetAccount.bind(this)}>
                                    <PasswordBox id="resetAccountPassword" placeholder="Password" className='settings' value={this.state["resetAccountPassword"]} onChange={this.handleInputChange} autoComplete="new-password" />

                                    <button type="submit" id="resetAccount" className="btn btn-primary" disabled={this.state.resetAccountIsSubmitting}>Reset</button>
                                    <Popover className={this.state.resetAccountPopoverClass} placement='right' isOpen={this.state.resetAccountPopoverIsOpen} target='resetAccount'>
                                        <PopoverBody>{this.state.resetAccountPopoverMessage}</PopoverBody>
                                    </Popover>
                                </form>
                            </div>
                        </div>
                    </div>
                    <div className="col-md-6 grid-margin stretch-card">
                        <div className="card">
                            <div className="card-body">
                                <h4 className="card-title">Delete account</h4>
                                <p className="card-description">Your account will be removed irreversibly!</p>
                                <form onSubmit={this.deleteAccount.bind(this)}>
                                    <PasswordBox id="deleteAccountPassword" placeholder="Password" className='settings' value={this.state["deleteAccountPassword"]} onChange={this.handleInputChange} autoComplete="new-password" />

                                    <button type="submit" id="deleteAccount" className="btn btn-primary" disabled={this.state.deleteAccountIsSubmitting}>Delete</button>
                                    <Popover className={this.state.deleteAccountPopoverClass} placement='right' isOpen={this.state.deleteAccountPopoverIsOpen} target='deleteAccount'>
                                        <PopoverBody>{this.state.deleteAccountPopoverMessage}</PopoverBody>
                                    </Popover>
                                </form>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}