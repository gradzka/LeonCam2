import React, { Component } from 'react';
import { Footer } from './Shared/Footer';
import { authenticationService } from '../services/AuthenticationService';
import { Navbar } from './Shared/Navbar';
import { Sidebar } from './Shared/Sidebar';
import './Layout.css';

export class Layout extends Component {
    static displayName = Layout.name;
    state = {}

    render() {
        const currentUser = authenticationService.currentUserValue;

        let navbarComponent = !this.state.isFullPageLayout && currentUser ? <Navbar /> : '';
        let sidebarComponent = !this.state.isFullPageLayout && currentUser ? <Sidebar /> : '';

        let paddingStyle = !currentUser ? { padding: 0 } : { } ;

        return (
            <div className="container-scroller">
                {navbarComponent}
                <div className="container-fluid page-body-wrapper">
                    {sidebarComponent}
                    <div className="main-panel" style={paddingStyle }>
                        <div className="content-wrapper" style={paddingStyle}>
                            {this.props.children}
                        </div>
                        <Footer />
                    </div>
                </div>
            </div>
        );
    }
}
