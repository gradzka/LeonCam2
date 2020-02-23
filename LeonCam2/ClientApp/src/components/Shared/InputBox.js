import React, { Component } from 'react';
import './InputBox.css';

export class InputBox extends Component {

    constructor(props) {
        super(props);
    }

    render() {
        const isInputPasswordType = this.props.type === "password";
        let eye = isInputPasswordType ? <div className="input-group-addon">
            <button className="fa fa-eye-slash" aria-hidden="true"/>
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