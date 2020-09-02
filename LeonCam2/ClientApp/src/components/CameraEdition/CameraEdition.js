import React, { Component } from 'react';
import { Row } from 'reactstrap';
import { Popover, PopoverBody } from 'reactstrap';
import { InputBox } from '../Shared/InputBox';
import { PasswordBox } from '../Shared/PasswordBox';
import { faExpandAlt, faPowerOff, faTableTennis, faTh, faTrash } from '@fortawesome/free-solid-svg-icons';
import { browserHistory } from '../../router/BrowserHistory';
import './CameraEdition.css';
import { CircleActionButton } from "../Shared/CircleActionButton";

export class CameraEdition extends Component {
    constructor(props) {
        super(props);
        console.log(this.props.location.state.id)

        this.state = {
            description: '',
            ip: '',
            username: '',
            editCameraPopoverMessage: '',
            editCameraPopoverClass: '',
            editCameraPopoverIsOpen: false,
            editCameraIsSubmitting: false,

            oldPassword: '',
            newPassword: '',
            confirmNewPassword: '',
            changePasswordPopoverMessage: '',
            changePasswordPopoverClass: '',
            changePasswordPopoverIsOpen: false,
            changePasswordIsSubmitting: false,

            pingPopoverMessage: '',
            pingPopoverClass: '',
            pingPopoverIsOpen: false,
            pingIsSubmitting: false,
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
        this.setState({
            pingPopoverIsOpen: true,
            pingPopoverMessage: 'PING to camera'
        });
    }

    componentWillUnmount() {
        document.body.removeEventListener('click', this.hidePopover);
    }

    hidePopover() {
        this.setState({
            editCameraPopoverIsOpen: false,
            changePasswordPopoverIsOpen: false,
        });
    }

    editCamera(event) {
        this.setState({
            editCameraIsSubmitting: true,
            editCameraPopoverIsOpen: false
        });

        //service
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
            //service
        }

        event.preventDefault();
    }


    render() {
        return (
            <div>
                <h1>
                    Camera edition
                </h1>
                <Row className="justify-content-center padding-top-bottom-10">
                    <CircleActionButton icon={faPowerOff} className='leon-green' />
                    <CircleActionButton id="ping" icon={faTableTennis} />
                    <CircleActionButton icon={faExpandAlt} onClickAction={() => browserHistory.push('/camerafullscreen', { id: this.props.location.state.id })} />
                    <CircleActionButton icon={faTh} className="leon-red" />
                    <CircleActionButton icon={faTrash} className="leon-red" />

                    <Popover className={this.state.pingPopoverClass} placement='top' isOpen={this.state.pingPopoverIsOpen} target='ping'>
                        <PopoverBody>{this.state.pingPopoverMessage}</PopoverBody>
                    </Popover>
                </Row>
                <div className="row">
                    <div className="col-md-6 grid-margin stretch-card">
                        <div className="card">
                            <div className="card-body">
                                <h4 className="card-title">Edit camera</h4>
                                <form onSubmit={this.editCamera.bind(this)}>
                                    <InputBox id="description" className='settings' type="text" placeholder="Camera description" value={this.state.description} onChange={this.handleInputChange} />
                                    <InputBox id="ip" className='settings' type="text" placeholder="IP address" value={this.state.ip} onChange={this.handleInputChange} />
                                    <InputBox id="username" className='settings' type="text" placeholder="Username" value={this.state.username} onChange={this.handleInputChange} />

                                    <button type="submit" id="editCamera" className="btn btn-primary" disabled={this.state.editCameraIsSubmitting}>Edit</button>
                                    <Popover className={this.state.editCameraPopoverClass} placement='right' isOpen={this.state.editCameraPopoverIsOpen} target='editCamera'>
                                        <PopoverBody>{this.state.editCameraPopoverMessage}</PopoverBody>
                                    </Popover>
                                </form>
                            </div>
                        </div>
                    </div>
                    <div className="col-md-6 grid-margin stretch-card">
                        <div className="card">
                            <div className="card-body">
                                <h4 className="card-title">Change camera password</h4>
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
            </div>
        )
    }
}