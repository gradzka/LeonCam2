import React, { Component } from 'react';
import './InputBox.css';

export class InputBox extends Component {

    constructor(props) {
        super(props);
    }

    toggleEye(event) {
        event.target.classList.toggle("fa-eye-slash");
        event.target.classList.toggle("fa-eye");
        event.target.parentNode.parentNode.childNodes[0].type = event.target.parentNode.parentNode.childNodes[0].type === 'text' ? 'password' : 'text';
    }

    render() {
        const isInputPasswordType = this.props.type === "password";
        let eye = isInputPasswordType ? <div className="input-group-addon">
            <span className="fa fa-eye-slash" aria-hidden="true" onMouseDown={this.toggleEye} onMouseUp={this.toggleEye} />
        </div> : null;

        return (
            <div className={"input-container " + (isInputPasswordType ? 'password-container ' : '') + (this.props.className === undefined ? '' : this.props.className)}>
                <input type={this.props.type} id={this.props.id} required="required" />
                <label forhtml={this.props.id}>{this.props.placeholder}</label>
                {eye}
                <div className="bar"></div>
            </div>
        );
    }
}