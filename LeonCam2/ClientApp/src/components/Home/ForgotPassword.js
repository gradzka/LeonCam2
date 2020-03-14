import React, { Component } from 'react';
import { InputBox } from '../Shared/InputBox';
import { pushCard, popCard } from '../Shared/Card';

export class ForgotPassword extends Component {
    render() {
        return (
            <div className="card alt hidden bottom">
                <div className="toggle" tabIndex="0" onClick={pushCard}>Forgot password?</div>
                <h1 className="title">Forgot<br />password?
                        <div className="close" tabIndex="0" onClick={popCard.bind(this, 'Forgot password?')}></div>
                </h1>
                <form>
                    <InputBox id="forgotmail" placeholder="E-Mail" className="alt hidden" />
                    <div className="button-container">
                        <button><span>Send link</span></button>
                    </div>
                </form>
            </div>
        );
    }
}