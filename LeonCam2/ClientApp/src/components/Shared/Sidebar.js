import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { Collapse } from 'reactstrap';
import { Dropdown, DropdownToggle, DropdownMenu, DropdownItem } from 'reactstrap';
import './Sidebar.css';
import { browserHistory } from '../../router/BrowserHistory';
import { authenticationService } from '../../services/AuthenticationService';

export class Sidebar extends Component {
    constructor(props) {
        super(props);
        this.state = {
            basicUiMenuOpen: false,
            formElementsMenuOpen: false
        }

        document.body.classList.add('sidebar-icon-only');
    }

    logout(evt) {
        authenticationService.logout();
        browserHistory.push('/');
        evt.preventDefault();
    }

    toggleMenuState(menuState) {
        if (this.state[menuState]) {
            this.setState({ [menuState]: false });
        } else if (Object.keys(this.state).length === 0) {
            this.setState({ [menuState]: true });
        } else {
            Object.keys(this.state).forEach(i => {
                this.setState({ [i]: false });
            });
            this.setState({ [menuState]: true });
        }
    }

    componentDidUpdate(prevProps) {
        if (this.props.location !== prevProps.location) {
            this.onRouteChanged();
        }
    }

    onRouteChanged() {
        document.querySelector('#sidebar').classList.remove('active');
        Object.keys(this.state).forEach(i => {
            this.setState({ [i]: false });
        });

        const dropdownPaths = [
            { path: '/basic-ui', state: 'basicUiMenuOpen' },
            { path: '/form-elements', state: 'formElementsMenuOpen' }
        ];

        dropdownPaths.forEach((obj => {
            if (this.isPathActive(obj.path)) {
                this.setState({ [obj.state]: true })
            }
        }));

    }

    toggleOffcanvas() {
        document.body.classList.toggle('sidebar-icon-only');
    }

    render() {
        return (
            <nav className="sidebar sidebar-offcanvas" id="sidebar">
                <div className="text-right sidebar-brand-wrapper d-flex align-items-center">
                    <button className="navbar-toggler navbar-toggler-right align-self-center" type="button" onClick={this.toggleOffcanvas}>
                        <i className="fa fa-navicon menu-icon" />
                    </button>
                </div>
                <ul className="nav">
                    <li className={this.isPathActive('/dashboard') ? 'nav-item active' : 'nav-item'}>
                        <Link className="nav-link" to="/dashboard">
                            <i className="fa fa-home menu-icon" />
                            <span className="menu-title">Dashboard</span>
                        </Link>
                    </li>
                    <li className={this.isPathActive('/cameras') ? 'nav-item active' : 'nav-item'}>
                        <div className={this.state.basicUiMenuOpen ? 'nav-link menu-expanded' : 'nav-link'} onClick={() => this.toggleMenuState('basicUiMenuOpen')} data-toggle="collapse">
                            <i className="fa fa-camera menu-icon" />
                            <span className="menu-title">Cameras</span>
                            <i className="fa fa-angle-right menu-icon menu-arrow"></i>
                        </div>
                        <Collapse isOpen={this.state.basicUiMenuOpen}>
                            <ul className="nav flex-column sub-menu">
                                <li className="nav-item"> <Link className={this.isPathActive('/basic-ui/dropdowns') ? 'nav-link active' : 'nav-link'} to="/basic-ui/dropdowns">New</Link></li>
                                <li className="nav-item"> <Link className={this.isPathActive('/basic-ui/buttons') ? 'nav-link active' : 'nav-link'} to="/basic-ui/buttons">List</Link></li>
                            </ul>
                        </Collapse>
                    </li>
                    <li className={this.isPathActive('/memories') ? 'nav-item active' : 'nav-item'}>
                        <div className={this.state.basicUiMenuOpen ? 'nav-link menu-expanded' : 'nav-link'} onClick={() => this.toggleMenuState('basicUiMenuOpen')} data-toggle="collapse">
                            <i className="fa fa-photo menu-icon" />
                            <span className="menu-title">Visual</span>
                            <i className="fa fa-angle-right menu-icon menu-arrow"></i>
                        </div>
                        <Collapse isOpen={this.state.basicUiMenuOpen}>
                            <ul className="nav flex-column sub-menu">
                                <li className="nav-item"> <Link className={this.isPathActive('/basic-ui/dropdowns') ? 'nav-link active' : 'nav-link'} to="/basic-ui/dropdowns">Pictures</Link></li>
                                <li className="nav-item"> <Link className={this.isPathActive('/basic-ui/buttons') ? 'nav-link active' : 'nav-link'} to="/basic-ui/buttons">Videos</Link></li>
                            </ul>
                        </Collapse>
                    </li>
                    <li className={this.isPathActive('/settings') ? 'nav-item active' : 'nav-item'}>
                        <Link className="nav-link" to="/settings">
                            <i className="fa fa-cog menu-icon" />
                            <span className="menu-title">Settings</span>
                        </Link>
                    </li>
                    <li className="nav-item">
                        <a className="nav-link" onClick={this.logout}>
                            <i className="fa fa-sign-out menu-icon" />
                            <span className="menu-title">Logout</span>
                        </a>
                    </li>
                </ul>
            </nav>
        );
    }

    isPathActive(path) {
        if (!this.props.location) {
            return false;
        }
        return this.props.location.pathname.startsWith(path);
    }
}
