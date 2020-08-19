import React, { Component } from 'react';
import { SortableContainer, SortableElement } from "react-sortable-hoc";
import arrayMove from "array-move";
import { Container, Row, Col } from 'reactstrap';
import "./Dashboard.css";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPowerOff, faCamera, faEdit, faTrash, faExpandAlt } from '@fortawesome/free-solid-svg-icons';
import { Camera } from "../Shared/Camera.js";
import { browserHistory } from '../../router/BrowserHistory';

const CircleActionButton = ({ icon, className, onClickAction }) => (
    <Col xs="auto" className="padding-right-left-5" >
        <button className={"circle " + className} onClick={onClickAction}>
            <FontAwesomeIcon icon={icon} />
        </button>
    </Col>
);

class SortableCamera extends Component {
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
        return (
            <div className="dashboard-grid-cell">
                <Container>
                    <Row className="justify-content-center padding-top-bottom-5 no-select">
                        {this.props.camera.name}
                    </Row>
                    <Row>
                        <div className="camera-container"></div>
                    </Row>
                    <Row className="justify-content-center padding-top-bottom-5">
                        <CircleActionButton icon={faPowerOff} onClickAction={this.powerOnOff} className={this.state.isOn ? 'leon-green' : 'leon-red'} />
                        <CircleActionButton icon={faExpandAlt} onClickAction={() => browserHistory.push('/camerafullscreen', { id: this.props.camera.id })} />
                        <CircleActionButton icon={faEdit} onClickAction={() => browserHistory.push('/cameraedition', { id: this.props.camera.id }) } />
                        <CircleActionButton icon={faCamera} onClickAction={() => this.props.onTakePhoto(this.props.camera)} />
                        <CircleActionButton icon={faTrash} onClickAction={() => this.props.onRemove(this.props.indexCopy, this.props.camera)} className="leon-red" />
                    </Row>
                </Container>
            </div>
        );
    }
}

const SortableItem = SortableElement(({ indexCopy, camera, onPowerOff, onRemove, onTakePhoto }) => {
    return (
        <SortableCamera camera={camera} indexCopy={indexCopy} onPowerOff={onPowerOff} onRemove={onRemove} onTakePhoto={onTakePhoto} />
    );
});

const SortableList = SortableContainer(({ items, onPowerOff, onRemove, onTakePhoto }) => {
    return (
        <div>
            {items.map((camera, index) => (
                <SortableItem key={`item-${index}`} index={index} indexCopy={index} camera={camera} onPowerOff={onPowerOff} onRemove={onRemove} onTakePhoto={onTakePhoto} />
            ))}
        </div>
    );
});

class SortableComponent extends Component {
    constructor(props) {
        super(props);

        this.state = {
            items: Array.apply(null, this.props.items).map((camera, index) => camera)
        };
    }   

    componentDidMount() { }

    powerOff = (camera) => {

    }

    remove = ( index, camera ) => {
        this.state.items.splice(index, 1);
        this.setState({ items: this.state.items });
    }

    sortEnd = ({ oldIndex, newIndex }) => {
        this.setState(({ items }) => ({
            items: arrayMove(items, oldIndex, newIndex)
        }));
    };

    takePhoto = (camera) => {

    }

    render() {
        return (
            <div className="dashboard-grid">
                <SortableList axis="xy" items={this.state.items} onPowerOff={this.powerOff} onRemove={this.remove} onSortEnd={this.sortEnd} onTakePhoto={this.takePhoto} pressDelay={200}/>
            </div>
        );
    }
}

export class Dashboard extends Component {
    render() {
        return (
            <SortableComponent items=
                {
                    [
                        new Camera(1, "Baby room", "192.168.1.1"),
                        new Camera(2, "Living room", "192.168.1.2"),
                        new Camera(3, "Front door", "192.168.1.3"),
                        new Camera(4, "Garage", "192.168.1.4"),
                        new Camera(5, "Garden", "192.168.1.5")
                    ]
                } />
        );
    }
}