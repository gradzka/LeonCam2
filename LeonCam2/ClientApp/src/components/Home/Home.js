import React, { Component } from 'react';
import './Home.css';
import { InputBox } from '../Shared/InputBox';
import { PasswordBox } from '../Shared/PasswordBox';

export class Home extends Component {
    static displayName = Home.name;

    constructor(props) {
        super(props);
        this.popCard = this.popCard.bind(this);
    }

    pushCard(event) {
        let card_alt = event.target.parentNode;
        card_alt.classList.remove('hidden');
        Array.from(card_alt.getElementsByClassName('input-container')).forEach(element => element.classList.remove('hidden'));

        event.target.innerHTML = "";
    }

    popCard(innerHTML, event) {
        let card_alt = event.target.parentNode.parentNode;
        card_alt.classList.add('hidden');
        Array.from(card_alt.getElementsByClassName('input-container')).forEach(element => element.classList.add('hidden'));

        card_alt.childNodes[0].innerHTML = innerHTML;
    }

    render() {
        return (
            <div className="card-container">
                <div className="card alt hidden">
                    <div className="toggle" tabIndex="0" onClick={this.pushCard}>Register</div>
                    <h1 className="title">Register
                        <div className="close" tabIndex="0" onClick={this.popCard.bind(this, 'Register')}></div>
                    </h1>
                    <form>
                        <InputBox id="regname" placeholder="Username" className="alt hidden" />
                        <PasswordBox id="regpass" placeholder="Password" className="alt hidden" withPasswordStrength={true} />
                        <PasswordBox id="reregpass" placeholder="Repeat Password" className="alt hidden" />
                        <div className="button-container">
                            <button><span>Sign up</span></button>
                        </div>
                    </form>
                </div>
                <div className="card alt hidden bottom">
                    <div className="toggle" tabIndex="0" onClick={this.pushCard}>Forgot password?</div>
                    <h1 className="title">Forgot<br />password?
                        <div className="close" tabIndex="0" onClick={this.popCard.bind(this, 'Forgot password?')}></div>
                    </h1>
                    <form>
                        <InputBox id="forgotmail" placeholder="E-Mail" className="alt hidden" />
                        <div className="button-container">
                            <button><span>Send link</span></button>
                        </div>
                    </form>
                </div>
                <div className="card first"></div>
                <div className="card">
                    <h1 className="title">Login</h1>
                    <form>
                        <InputBox id="name" placeholder="Username"/>
                        <PasswordBox id="pass" placeholder="Password" />
                        <div className="button-container">
                            <button><span>Sign in</span></button>
                        </div>
                    </form>
                </div>
            </div>
        );
    }
}
