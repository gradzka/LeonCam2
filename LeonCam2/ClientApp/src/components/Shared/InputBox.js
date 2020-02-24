import React, { Component } from 'react';
import './InputBox.css';

export class InputBox extends Component {

    constructor(props) {
        super(props);  
    }

    render() {
        return (
            <div className={"input-container " + (this.props.className === undefined ? '' : this.props.className)}>
                <input type="text" id={this.props.id} required="required"/>
                <label forhtml={this.props.id}>{this.props.placeholder}</label>
                <div className="bar"></div>
            </div>
        );
    }
}