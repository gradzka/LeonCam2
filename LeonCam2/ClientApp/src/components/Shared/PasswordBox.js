import React, { Component } from 'react';
import './InputBox.css';
import './PasswordBox.css';

export class PasswordBox extends Component {

    constructor(props) {
        super(props);
    }

    hidePassword(event) {
        event.target.classList.remove("fa-eye");
        event.target.classList.add("fa-eye-slash");
        event.target.parentNode.parentNode.childNodes[0].type = 'password';
    }

    showPassword(event) {
        event.target.classList.remove("fa-eye-slash");
        event.target.classList.add("fa-eye");
        event.target.parentNode.parentNode.childNodes[0].type = 'text';
    }

    toggleCapsLkWarning(event) {
        let inputContainer = event.target.parentNode;

        if (event.getModifierState('CapsLock')) {
            inputContainer.childNodes[1].childNodes[1].style.visibility = "visible";
        }
        else {
            inputContainer.childNodes[1].childNodes[1].style.visibility = "hidden";
        }
    }

    hideCapsLkWarning(event) {
        let inputContainer = event.target.parentNode;
        inputContainer.childNodes[1].childNodes[1].style.visibility = "hidden";
    }

    render() {
        return (
            <div className={"input-container password-container " + (this.props.className === undefined ? '' : this.props.className)}>
                <input type="password" id={this.props.id} required="required" onKeyDown={this.toggleCapsLkWarning} onBlur={this.hideCapsLkWarning} />
                <label forhtml={this.props.id}>
                    {this.props.placeholder} 
                    <span id="capsLk" className="badge badge-warning">CapsLk</span>
                </label>
                <div className="input-group-addon">
                    <span className="fa fa-eye-slash" aria-hidden="true" onMouseDown={this.showPassword} onMouseUp={this.hidePassword} />
                </div>
                <div className="bar"></div>
            </div>
        );
    }
}