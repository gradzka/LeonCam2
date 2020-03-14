import React, { Component } from 'react';
import { InputBox } from '../Shared/InputBox';
import { pushCard, popCard } from '../Shared/Card';
import { PasswordBox } from '../Shared/PasswordBox';

export class Register extends Component {
    render() {
        return (<div className="card alt hidden">
            <div className="toggle" tabIndex="0" onClick={pushCard}>Register</div>
            <h1 className="title">Register
                <div className="close" tabIndex="0" onClick={popCard.bind(this, 'Register')}></div>
            </h1>
            <form>
                <InputBox id="regname" placeholder="Username" className="alt hidden" />
                <PasswordBox id="regpass" placeholder="Password" className="alt hidden" withPasswordStrength={true} />
                <PasswordBox id="reregpass" placeholder="Repeat Password" className="alt hidden" />
                <div className="button-container">
                    <button><span>Sign up</span></button>
                </div>
            </form>
        </div>);
    }
}