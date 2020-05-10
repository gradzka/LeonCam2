import React, { Component } from 'react';
import { InputBox } from '../Shared/InputBox';
import { PasswordBox } from '../Shared/PasswordBox';
import { authenticationService } from '../../services/AuthenticationService';
import { Popover, PopoverBody } from 'reactstrap';

export class Register extends Component {
    constructor(props) {
        super(props);
        this.state = {
            regname: '',
            regpass: '',
            reregpass: '',
            popoverMessage: '',
            popoverClass: '',
            popoverIsOpen: false,
            isSubmitting: false,
        }

        this.handleInputChange = this.handleInputChange.bind(this);
        this.hidePopover = this.hidePopover.bind(this);
        this.pushCard = this.pushCard.bind(this);
        this.popCard = this.popCard.bind(this);
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

    pushCard() {
        this.props.onOnTopChanged('registerCardOnTop', true);
    }

    popCard() {
        this.props.onOnTopChanged('registerCardOnTop', false);
    }

    render() {
        let toggledHidden = "alt" + (this.props.onTop === true ? '' : ' hidden');

        return (
            <div className={"card " + toggledHidden}>
                <div className="toggle" tabIndex="0" onClick={this.pushCard}>Register</div>
            <h1 className="title">Register
                <div className="close" tabIndex="0" onClick={this.popCard}></div>
            </h1>
            <form onSubmit={this.register.bind(this)}>
                    <InputBox id="regname" type="text" placeholder="Username" className={toggledHidden} value={this.state["regname"]} onChange={this.handleInputChange} autoComplete="new-password"/>
                    <PasswordBox id="regpass" placeholder="Password" className={toggledHidden} value={this.state["regpass"]} onChange={this.handleInputChange} withPasswordStrength={true} autoComplete="new-password"/>
                    <PasswordBox id="reregpass" placeholder="Repeat Password" className={toggledHidden} value={this.state["reregpass"]} onChange={this.handleInputChange} autocomplete="new-password" />
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