import React, { Component, Fragment } from 'react';
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import './NavMenu.css';
import { browserHistory } from '../router/BrowserHistory';
import { authenticationService } from '../services/AuthenticationService';

export class NavMenu extends Component {
    static displayName = NavMenu.name;

    constructor(props) {
        super(props);

        this.toggleNavbar = this.toggleNavbar.bind(this);
        this.state = {
            collapsed: true,
            currentUser: null
        };
    }

    toggleNavbar() {
        this.setState({
            collapsed: !this.state.collapsed
        });
    }

    componentDidMount() {
        authenticationService.currentUser.subscribe(x => this.setState({ currentUser: x }));
    }

    logout() {
        authenticationService.logout();
        browserHistory.push('/');
    }

    render() {
        const { currentUser } = this.state;

        return (
            <header>
                <Navbar className="navbar-expand-sm navbar-toggleable-sm ng-white border-bottom box-shadow mb-3" light>
                    <Container>
                        <NavbarBrand tag={Link} to="/"><b>LeonCam2</b></NavbarBrand>
                        {currentUser &&
                            <Fragment>
                                <NavbarToggler onClick={this.toggleNavbar} className="mr-2" />
                                <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={!this.state.collapsed} navbar>
                                    <ul className="navbar-nav flex-grow">
                                        <NavItem>
                                            <NavLink tag={Link} className="text-dark" to="/">Home</NavLink>
                                        </NavItem>
                                        <NavItem>
                                            <NavLink tag={Link} className="text-dark" to="/counter">Counter</NavLink>
                                        </NavItem>
                                        <NavItem>
                                            <NavLink tag={Link} className="text-dark" to="/fetch-data">Fetch data</NavLink>
                                        </NavItem>
                                        <NavItem>
                                            <a className="text-dark nav-link" onClick={this.logout}>Logout</a>
                                        </NavItem>
                                    </ul>
                                </Collapse>
                            </Fragment>
                        }
                    </Container>
                </Navbar>
            </header>
        );
    }
}
