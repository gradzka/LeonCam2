import React, { Component } from 'react';
import './Home.css';

export class Home extends Component {
    static displayName = Home.name;

    constructor(props) {
        super(props);

        //this.login = this.login.bind(this);
    }

    openRegisterButton(event) {
        let container = document.getElementsByClassName("card-container")[0];
        container.classList.add('active');

        let card_alt = document.getElementsByClassName("alt")[0];
        card_alt.classList.remove('hidden');

        let toggle = document.getElementsByClassName("toggle")[0];
        toggle.innerHTML = "";
    }

    closeRegisterButton(event) {
        let container = document.getElementsByClassName("card-container")[0];
        container.classList.remove('active');

        let card_alt = document.getElementsByClassName("alt")[0];
        card_alt.classList.add('hidden');

        let toggle = document.getElementsByClassName("toggle")[0];
        toggle.innerHTML += "sign up";
    }

    openForgotPasswordButton(event) {
        let container = document.getElementsByClassName("card-container")[0];
        container.classList.add('active');
        container.classList.add('bottom');

        let card_alt = document.getElementsByClassName("alt")[1];
        card_alt.classList.remove('hidden');

        let toggle = document.getElementsByClassName("toggle")[1];
        toggle.innerHTML = "";
    }

    closeForgotPasswordButton(event) {
        let container = document.getElementsByClassName("card-container")[0];
        container.classList.remove('active');
        container.classList.remove('bottom');

        let card_alt = document.getElementsByClassName("alt")[1];
        card_alt.classList.add('hidden');

        let toggle = document.getElementsByClassName("toggle")[1];
        toggle.innerHTML += "Forgot your password?";
    }

    render() {
        return (
            <div className="container-fluid content-row row">
                <div className="col-sm-4" style={{ margin: "0 auto 20px" }}>
                    <div className="card" style={{ height: "100%" }}>
                        <img className="card-img-top" src=".../100px180/" alt="Card image cap" />
                        <div className="card-body">
                            <h5 className="card-title">Card title</h5>
                            <p className="card-text">Some quick example text to build on the card title and make up the bulk of the card's content.</p>
                            <a href="#" className="btn btn-primary">Go somewhere</a>
                        </div>
                    </div>
                </div>
                <div className="col">
                    <div className="card-container col-sm-8">
                        <div className="card"></div>
                        <div className="card">
                            <h1 className="title">Login</h1>
                            <form>
                                <div className="input-container">
                                    <input type="text" id="name" required="required" />
                                    <label forhtml="name">Username</label>
                                    <div className="bar"></div>
                                </div>
                                <div className="input-container">
                                    <input type="password" id="pass" required="required" />
                                    <label forhtml="pass">Password</label>
                                    <div className="bar"></div>
                                </div>
                                <div className="button-container">
                                    <button><span>sign in</span></button>
                                </div>
                                <div className="footer"></div>
                            </form>
                        </div>
                        <div className="card alt hidden">
                            <div className="toggle" tabIndex="0" onClick={this.openRegisterButton}>sign up</div>
                            <h1 className="title">Register
                                <div className="close" tabIndex="0" onClick={this.closeRegisterButton}></div>
                            </h1>
                            <form>
                                <div className="input-container">
                                    <input type="text" id="regname" required="required" />
                                    <label forhtml="regname">Username</label>
                                    <div className="bar"></div>
                                </div>
                                <div className="input-container">
                                    <input type="password" id="regpass" required="required" />
                                    <label forhtml="regpass">Password</label>
                                    <div className="bar"></div>
                                </div>
                                <div className="input-container">
                                    <input type="password" id="reregpass" required="required" />
                                    <label forhtml="reregpass">Repeat Password</label>
                                    <div className="bar"></div>
                                </div>
                                <div className="button-container">
                                    <button><span>sign up</span></button>
                                </div>
                            </form>
                        </div>
                        <div className="card alt hidden bottom">
                            <div className="toggle" tabIndex="0" onClick={this.openForgotPasswordButton}>Forgot your password?</div>
                            <h1 className="title">Forgot your password?
                                <div className="close" tabIndex="0" onClick={this.closeForgotPasswordButton}></div>
                            </h1>
                            <form>
                                <div style={{ height: 60 + "px" }}></div>

                                <div className="input-container">
                                    <input type="text" id="forgotmail" required="required" />
                                    <label forhtml="regname">E-Mail</label>
                                    <div className="bar"></div>
                                </div>
                                <div style={{ height: 70+"px" }}></div>
                                <div className="button-container">
                                    <button><span>Send password</span></button>
                                </div>
                            </form>
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}
