import React, { Component } from 'react';
import { SortableContainer, SortableElement } from "react-sortable-hoc";
import arrayMove from "array-move";
import { Container, Row, Col } from 'reactstrap';
import "./Dashboard.css";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPowerOff, faCamera, faEdit, faTrash } from '@fortawesome/free-solid-svg-icons';

const CircleActionButton = ({ icon }) => (
    <Col xs="auto" className="padding-right-left-5">
        <div className="circle">
            <FontAwesomeIcon icon={icon}/>
        </div>
    </Col>
);

const SortableItem = SortableElement(({ value, camera }) => {
    return (
        <div className="dashboard-grid-cell">
            <Container>
                <Row className="justify-content-center padding-top-bottom-5">
                    {value}
                </Row>
                <Row>
                    <div className="camera-container"></div>
                </Row>
                <Row className="justify-content-center padding-top-bottom-5">
                    <CircleActionButton icon={faPowerOff} />
                    <CircleActionButton icon={faCamera} />
                    <CircleActionButton icon={faEdit} />
                    <CircleActionButton icon={faTrash} />
                </Row>
            </Container>
        </div>
    );
});

const SortableList = SortableContainer(({ items }) => {
    return (
        <div>
            {items.map((value, index) => (
                <SortableItem key={`item-${index}`} index={index} value={value} />
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
                <SortableList axis="xy" items={this.state.items} onSortEnd={this.onSortEnd}/>
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