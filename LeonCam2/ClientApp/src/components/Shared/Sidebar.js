import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import { Collapse } from 'reactstrap';
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faBars, faHome, faCamera, faAngleRight, faPlusSquare, faListAlt, faPhotoVideo, faImage, faFilm, faCogs, faSignOutAlt } from "@fortawesome/free-solid-svg-icons";
import { browserHistory } from '../../router/BrowserHistory';
import { authenticationService } from '../../services/AuthenticationService';
import './Sidebar.css';

export class Sidebar extends Component {
    constructor(props) {
        super(props);
        this.state = {
            camerasMenuOpen: false,
            visualMenuOpen: false
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
        if (this.props.location !== prevProps.location)
        {
            this.onRouteChanged();
        }
    }

    onRouteChanged() {
        const dropdownPaths = [
            { path: '/cameras', state: 'camerasMenuOpen' },
            { path: '/visual', state: 'visualMenuOpen' }
        ];

        dropdownPaths.forEach((obj => this.setState({ [obj.state]: this.isPathActive(obj.path) })));
    }

    toggleOffcanvas() {
        document.body.classList.toggle('sidebar-icon-only');
    }

    render() {
        return (
            <nav className="sidebar sidebar-offcanvas" id="sidebar">
                <div className="text-right sidebar-brand-wrapper d-flex align-items-center">
                    <button className="navbar-toggler navbar-toggler-right align-self-center" type="button" onClick={this.toggleOffcanvas}>
                        <FontAwesomeIcon icon={faBars} className="menu-icon" />
                    </button>
                </div>
                <ul className="nav">
                    <li className={this.isPathActive('/dashboard') ? 'nav-item active' : 'nav-item'}>
                        <Link className="nav-link" to="/dashboard">
                            <div className="menu-icon">
                                <FontAwesomeIcon icon={faHome} />
                            </div>
                            <span className="menu-title">Dashboard</span>
                        </Link>
                    </li>
                    <li className={this.isPathActive('/cameras') ? 'nav-item active' : 'nav-item'}>
                        <div className={this.state.camerasMenuOpen ? 'nav-link menu-expanded' : 'nav-link'} onClick={() => this.toggleMenuState('camerasMenuOpen')} data-toggle="collapse">
                            <div className="menu-icon">
                                <FontAwesomeIcon icon={faCamera} />
                            </div>
                            <span className="menu-title">Cameras</span>
                            <div className="menu-icon menu-arrow">
                                <FontAwesomeIcon icon={faAngleRight} />
                            </div>
                        </div>
                        <Collapse isOpen={this.state.camerasMenuOpen}>
                            <ul className="nav flex-column sub-menu">
                                <li className={this.isPathActive('/cameras/new') ? 'nav-item active' : 'nav-item'}>
                                    <Link className='nav-link' to="/cameras/new">
                                        <div className="menu-icon">
                                            <FontAwesomeIcon icon={faPlusSquare} />
                                        </div>
                                        <span className="menu-title">New</span>
                                    </Link>
                                </li>
                                <li className={this.isPathActive('/cameras/list') ? 'nav-item active' : 'nav-item'}>
                                    <Link className='nav-link' to="/cameras/list">
                                        <div className="menu-icon">
                                            <FontAwesomeIcon icon={faListAlt} />
                                        </div>
                                        <span className="menu-title">List</span>
                                    </Link>
                                </li>
                            </ul>
                        </Collapse>
                    </li>
                    <li className={this.isPathActive('/visual') ? 'nav-item active' : 'nav-item'}>
                        <div className={this.state.visualMenuOpen ? 'nav-link menu-expanded' : 'nav-link'} onClick={() => this.toggleMenuState('visualMenuOpen')} data-toggle="collapse">
                            <div className="menu-icon">
                                <FontAwesomeIcon icon={faPhotoVideo} />
                            </div>
                            <span className="menu-title">Visual</span>
                            <div className="menu-icon menu-arrow">
                                <FontAwesomeIcon icon={faAngleRight} />
                            </div>
                        </div>
                        <Collapse isOpen={this.state.visualMenuOpen}>
                            <ul className="nav flex-column sub-menu">
                                <li className={this.isPathActive('/visual/pictures') ? 'nav-item active' : 'nav-item'}>
                                    <Link className='nav-link' to="/visual/pictures">
                                        <div className="menu-icon">
                                            <FontAwesomeIcon icon={faImage} />
                                        </div>
                                        <span className="menu-title">Pictures</span>
                                    </Link>
                                </li>
                                <li className={this.isPathActive('/visual/pictures') ? 'nav-item active' : 'nav-item'}>
                                    <Link className='nav-link' to="/visual/videos">
                                        <div className="menu-icon">
                                            <FontAwesomeIcon icon={faFilm} />
                                        </div>
                                        <span className="menu-title">Videos</span>
                                    </Link>
                                </li>
                            </ul>
                        </Collapse>
                    </li>
                    <li className={this.isPathActive('/settings') ? 'nav-item active' : 'nav-item'}>
                        <Link className="nav-link" to="/settings">
                            <div className="menu-icon">
                                <FontAwesomeIcon icon={faCogs} />
                            </div>
                            <span className="menu-title">Settings</span>
                        </Link>
                    </li>
                    <li className="nav-item">
                        <a className="nav-link" onClick={this.logout}>
                            <div className="menu-icon">
                                <FontAwesomeIcon icon={faSignOutAlt} />
                            </div>
                            <span className="menu-title">Logout</span>
                        </a>
                    </li>
                </ul>
            </nav>
        );
    }

    isPathActive(path) {
        return window.location.pathname.startsWith(path);
    }
}