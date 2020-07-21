import React, { Component } from 'react';
import { SortableContainer, SortableElement } from "react-sortable-hoc";
import arrayMove from "array-move";
import { Container, Row, Col } from 'reactstrap';
import "./Dashboard.css";

const SortableItem = SortableElement(({ value }) => {
    return (
        <div className="dashboard-grid-cell">
            <Container>
                <Row>
                    {value}
                </Row>
                <Row>
                    <Col><div className="circle"><i className="fa fa-power-off" /></div></Col>
                    <Col><div className="circle"><i className="fa fa-camera" /></div></Col>
                    <Col><div className="circle"><i className="fa fa-edit" /></div></Col>
                    <Col><div className="circle"><i className="fa fa-trash" /></div></Col>
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
            "Item 1",
            "Item 2",
            "Item 3",
            "Item 4",
            "Item 5",
            "Item 6",
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
            <SortableComponent/>
        );
    }
}