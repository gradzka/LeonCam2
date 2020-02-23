import React, { Component } from 'react';
import './Home.css';
import { InputBox } from '../Shared/InputBox';

export class Home extends Component {
    static displayName = Home.name;

    constructor(props) {
        super(props);
        this.removeCard = this.removeCard.bind(this);
    }

    putCardOn(event) {
        let card_alt = event.target.parentNode;
        card_alt.classList.remove('hidden');

        event.target.innerHTML = "";
    }

    removeCard(innerHTML, event) {
        let card_alt = event.target.parentNode.parentNode;
        card_alt.classList.add('hidden');

        card_alt.childNodes[0].innerHTML = innerHTML;
    }

    render() {
        return (
            <div className="card-container">
                <div className="card alt hidden">
                    <div className="toggle" tabIndex="0" onClick={this.putCardOn}>Register</div>
                    <h1 className="title">Register
                        <div className="close" tabIndex="0" onClick={this.removeCard.bind(this, 'Register')}></div>
                    </h1>
                    <form>
                        <InputBox type="text" id="regname" placeholder="Username" className="alt" />
                        <InputBox type="password" id="regpass" placeholder="Password" className="alt" />
                        <InputBox type="password" id="reregpass" placeholder="Repeat Password" className="alt" />
                        <div className="button-container">
                            <button><span>Sign up</span></button>
                        </div>
                    </form>
                </div>
                <div className="card alt hidden bottom">
                    <div className="toggle" tabIndex="0" onClick={this.putCardOn}>Forgot password?</div>
                    <h1 className="title">Forgot<br />password?
                        <div className="close" tabIndex="0" onClick={this.removeCard.bind(this, 'Forgot password?')}></div>
                    </h1>
                    <form>
                        <InputBox type="text" id="forgotmail" placeholder="E-Mail" className="alt" />
                        <div className="button-container">
                            <button><span>Send link</span></button>
                        </div>
                    </form>
                </div>
                <div className="card first"></div>
                <div className="card">
                    <h1 className="title">Login</h1>
                    <form>
                        <InputBox type="text" id="name" placeholder="Username"/>
                        <InputBox type="password" id="pass" placeholder="Password" />
                        <div className="button-container">
                            <button><span>Sign in</span></button>
                        </div>
                        <div className="footer"></div>
                    </form>
                </div>
            </div>
        );
    }
}
