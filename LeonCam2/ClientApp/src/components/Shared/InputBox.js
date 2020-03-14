import React, { Component } from 'react';
import './InputBox.css';

export class InputBox extends Component {

    constructor(props) {
        super(props);
        this.handleChange = this.handleChange.bind(this);
    }

    handleChange(event) {
        if (typeof this.props.onChange === "function") {
            const text = event.target.value;
            this.props.onChange(this.props.id, text);
        }
    }

    render() {
        return (
            <div className={"input-container " + (this.props.className === undefined ? '' : this.props.className)}>
                <input type="text" id={this.props.id} required="required" value={this.props.value} onChange={this.handleChange} />
                <label forhtml={this.props.id}>{this.props.placeholder}</label>
                <div className="bar"></div>
            </div>
        );
    }
}