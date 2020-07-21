import React, { Component } from 'react';
import './Navbar.css';
import { browserHistory } from '../../router/BrowserHistory';
import { authenticationService } from '../../services/AuthenticationService';

export class Navbar extends Component {
 
    logout(evt) {
        authenticationService.logout();
        browserHistory.push('/');
        evt.preventDefault();
    }

    render() {
        return (
            <nav className="navbar col-lg-12 col-12 p-lg-0 fixed-top d-flex flex-row">
                <div className="navbar-menu-wrapper d-flex align-items-center justify-content-between">
                    <ul className="navbar-nav navbar-nav-left">
                        <li className="nav-item d-none d-lg-flex">
                            <a href="!#" onClick={evt => evt.preventDefault()} className="nav-link">
                                <i className="fa fa-camera"></i>
                            </a>
                        </li>
                    </ul>
                    <ul className="navbar-nav ml-lg-auto">
                        <li className="nav-item nav-profile border-0">
                            <a href="!#" onClick={this.logout} className="nav-link">
                                <i className="fa fa-sign-out"></i>
                            </a>
                        </li>
                    </ul>
                </div>
            </nav>
        );
    }
}