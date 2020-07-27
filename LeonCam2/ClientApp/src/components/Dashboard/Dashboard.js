import React, { Component } from 'react';
import { SortableContainer, SortableElement } from "react-sortable-hoc";
import arrayMove from "array-move";
import { Container, Row, Col } from 'reactstrap';
import "./Dashboard.css";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPowerOff, faCamera, faEdit, faTrash, faExpandAlt } from '@fortawesome/free-solid-svg-icons';

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
                    <Row className="justify-content-center padding-top-bottom-5 no-select">
                        {this.props.value}
                    </Row>
                    <Row>
                        <div className="camera-container"></div>
                    </Row>
                    <Row className="justify-content-center padding-top-bottom-5">
                        <CircleActionButton icon={faPowerOff} onClickAction={this.powerOnOff} className={this.state.isOn ? 'leon-green' : 'leon-red'} />
                        <CircleActionButton icon={faExpandAlt} />
                        <CircleActionButton icon={faEdit} />
                        <CircleActionButton icon={faCamera} />
                        <CircleActionButton icon={faTrash} onClickAction={() => this.props.onRemove(this.props.indexCopy)} className="leon-red" />
                    </Row>
                </Container>
            </div>
        );
    }
}

const SortableItem = SortableElement(({ indexCopy, value, onRemove }) => {
    return (
        <SortableCamera value={value} indexCopy={indexCopy} onRemove={onRemove} />
    );
});

const SortableList = SortableContainer(({ items, onRemove }) => {
    return (
        <div>
            {items.map((value, index) => (
                <SortableItem key={`item-${index}`} index={index} indexCopy={index} value={value} onRemove={onRemove} />
            ))}
        </div>
    );
});

class SortableComponent extends Component {
    constructor(props) {
        super(props);

        this.state = {
            items: Array.apply(null, this.props.items).map((val, index) => val)
        };
    }   

    componentDidMount() { }

    remove = ( index ) => {
        this.state.items.splice(index, 1);
        this.setState({ items: this.state.items });
    }

    onSortEnd = ({ oldIndex, newIndex }) => {
        this.setState(({ items }) => ({
            items: arrayMove(items, oldIndex, newIndex)
        }));
    };

    render() {
        return (
            <div className="dashboard-grid">
                <SortableList axis="xy" items={this.state.items} onSortEnd={this.onSortEnd} onRemove={this.remove} pressDelay={200}/>
            </div>
        );
    }
}

export class Dashboard extends Component {
    render() {
        return (
            <SortableComponent items={["Baby room", "Living room","Front door","Garage","Garden"]}/>
        );
    }
}