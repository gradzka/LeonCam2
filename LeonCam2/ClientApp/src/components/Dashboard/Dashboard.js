import React, { Component } from 'react';
import { SortableContainer, SortableElement } from "react-sortable-hoc";
import arrayMove from "array-move";
import { Container, Row, Col } from 'reactstrap';
import "./Dashboard.css";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPowerOff, faCamera, faEdit, faTrash, faExpand, faExchangeAlt, faExpandAlt } from '@fortawesome/free-solid-svg-icons';

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
    }

    render() {
        return (
            <div className="dashboard-grid-cell">
                <Container>
                    <Row className="justify-content-center padding-top-bottom-5">
                        {this.props.value}
                    </Row>
                    <Row>
                        <div className="camera-container"></div>
                    </Row>
                    <Row className="justify-content-center padding-top-bottom-5">
                        <CircleActionButton id="powerOnOff" icon={faPowerOff} onClickAction={this.powerOnOff} className={this.state.isOn ? 'bg-success' : 'bg-danger'} />
                        <CircleActionButton icon={faExpandAlt} />
                        <CircleActionButton icon={faEdit} />
                        <CircleActionButton icon={faCamera} />
                        <CircleActionButton icon={faTrash} />
                    </Row>
                </Container>
            </div>
        );
    }
}

const SortableItem = SortableElement(({ value, camera }) => {
    return (
        <SortableCamera value={value}/>
    );
});

const SortableList = SortableContainer(({ items }) => {
    return (
        <div>
            {items.map((value, index) => (
                <SortableItem key={`item-${index}`} index={index} value={value}/>
            ))}
        </div>
    );
});

class SortableComponent extends Component {
    state = {
        items: [
            "Baby room",
            "Living room",
            "Front door",
            "Garage",
            "Garden"
        ]
    };

    componentDidMount() { }

    onSortEnd = ({ oldIndex, newIndex }) => {
        this.setState(({ items }) => ({
            items: arrayMove(items, oldIndex, newIndex)
        }));
    };

    render() {
        return (
            <div className="dashboard-grid">
                <SortableList axis="xy"
                    items={this.state.items}
                    onSortEnd={this.onSortEnd}
                    pressDelay={250}
                />
            </div>
        );
    }
}

export class Dashboard extends Component {
    render() {
        return (
            <SortableComponent />
        );
    }
}