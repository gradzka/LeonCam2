import React, { Component } from 'react';
import './Home.css';
import { ForgotPassword } from './ForgotPassword';
import { Login } from './Login';
import { Register } from './Register';
import { authenticationService } from '../../services/AuthenticationService';
import { Row, Col, Container } from 'reactstrap';

export class Home extends Component {
    static displayName = Home.name;

    constructor(props) {
        super(props);
        this.state = {
            username: '',
            registerCardOnTop: false,
            forgotPasswordCardOnTop: false,
        }

        // Redirect to dashboard if user already logged in
        if (authenticationService.currentUserValue) {
            this.props.history.push('/dashboard');
        }

        this.onUsernameChanged = this.onUsernameChanged.bind(this);
        this.onOnTopChanged = this.onOnTopChanged.bind(this);
    }

    onUsernameChanged(value) {
        this.setState({ username: value });
    }

    onOnTopChanged(key, value) {
        this.setState({ [key]: value });
    }

    render() {
        let loginOnTop = !(this.state.forgotPasswordCardOnTop || this.state.registerCardOnTop);

        return (
            <Container>
                <Row style={{ paddingBottom: 5 + '%', paddingTop: 5 + '%' }}>
                    <Col></Col>
                    <Col lg={5} >
                        <div className="card-container">
                            <Register onTop={this.state.registerCardOnTop} onOnTopChanged={this.onOnTopChanged} />
                            <ForgotPassword onTop={this.state.forgotPasswordCardOnTop} username={this.state.username} location={this.props.location} history={this.props.history} onOnTopChanged={this.onOnTopChanged} />
                            <div className={"card first" + (loginOnTop ? '' : ' underneath')}></div>
                            <Login onTop={loginOnTop} location={this.props.location} history={this.props.history} onUsernameChanged={this.onUsernameChanged} />
                        </div>
                    </Col>
                </Row>
            </Container>
        );
    }
}
