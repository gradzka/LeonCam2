import React, { Component } from 'react';
import ReactDOM from 'react-dom';
import './InputBox.css';

export class InputBox extends Component {

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
            inputContainer.childNodes[1].childNodes[2].style.visibility = "visible";
        }
        else {
            inputContainer.childNodes[1].childNodes[2].style.visibility = "hidden";
        }
    }

    hideCapsLkWarning(event) {
        let inputContainer = event.target.parentNode;
        inputContainer.childNodes[1].childNodes[2].style.visibility = "hidden";
    }

    render() {
        const isInputPasswordType = this.props.type === "password";
        let eye = isInputPasswordType ? <div className="input-group-addon">
            <span className="fa fa-eye-slash" aria-hidden="true" onMouseDown={this.showPassword} onMouseUp={this.hidePassword}/>
        </div> : null;
        let capsLk = isInputPasswordType ? <span id="capsLk" className="badge badge-warning">CapsLk</span> : null;

        return (
            <div className={"input-container " + (isInputPasswordType ? 'password-container ' : '') + (this.props.className === undefined ? '' : this.props.className)}>
                <input type={this.props.type} id={this.props.id} required="required" onKeyDown={isInputPasswordType ? this.toggleCapsLkWarning : null} onBlur={isInputPasswordType ? this.hideCapsLkWarning : null} />
                <label forhtml={this.props.id}>{this.props.placeholder} {capsLk} </label>
                {eye}
                <div className="bar"></div>
            </div>
        );
    }
}