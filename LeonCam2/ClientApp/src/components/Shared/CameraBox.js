import React, { Component } from 'react';
import { Container, Row, Col } from 'reactstrap';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPowerOff, faCamera, faEdit, faTrash, faExpandAlt } from '@fortawesome/free-solid-svg-icons';
import { browserHistory } from '../../router/BrowserHistory';

const CircleActionButton = ({ icon, className, onClickAction }) => (
    <Col xs="auto" className="padding-right-left-5" >
        <button className={"circle " + className} onClick={onClickAction}>
            <FontAwesomeIcon icon={icon} />
        </button>
    </Col>
);

export class CameraBox extends Component {
    constructor(props) {
        super(props);
        this.state = {
            isOn: false
        }

        this.powerOnOff = this.powerOnOff.bind(this);
    }

    powerOnOff(event) {
        this.setState({
            isOn: !this.state.isOn
        });

        this.props.onPowerOff(this.props.indexCopy);
    }

    render() {

        let fullScreenItem = browserHistory.location.pathname !== "/camerafullscreen" ? <CircleActionButton icon={faExpandAlt} onClickAction={() => browserHistory.push('/camerafullscreen', { id: this.props.camera.id })} /> : '';

        return (
            <div className="dashboard-grid-cell">
                <Container>         
                    <Row className="justify-content-center padding-top-bottom-10 no-select">
                        {this.props.camera.name}
                    </Row>
                    <div className="embed-responsive embed-responsive-16by9">
                        <div className="embed-responsive-item camera-container"></div>
                    </div>
                    <Row className="justify-content-center padding-top-bottom-10">
                        <CircleActionButton icon={faPowerOff} onClickAction={this.powerOnOff} className={this.state.isOn ? 'leon-green' : 'leon-red'} />
                        {fullScreenItem}
                        <CircleActionButton icon={faEdit} onClickAction={() => browserHistory.push('/cameraedition', { id: this.props.camera.id })} />
                        <CircleActionButton icon={faCamera} onClickAction={() => this.props.onTakePhoto(this.props.camera)} />
                        <CircleActionButton icon={faTrash} onClickAction={() => this.props.onRemove(this.props.indexCopy, this.props.camera)} className="leon-red" />
                    </Row>
                </Container>
            </div>
        );
    }
}