import React, { Component } from 'react';
import { SortableContainer, SortableElement } from "react-sortable-hoc";
import arrayMove from "array-move";
import "./Dashboard.css";
import { Camera } from "../Shared/Camera.js";
import {SortableCamera} from "./SortableCamera.js"


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

export class Dashboard extends Component {
    constructor(props) {
        super(props);

        this.state = {
            items: []
        };

        this.getCameras = this.getCameras.bind(this);
    } 

    componentDidMount() {
        this.getCameras();
    }

    powerOff = (camera) => {

    }

    getCameras() {
        let cameras = [
            new Camera(1, "Baby room", "192.168.1.1"),
            new Camera(2, "Living room", "192.168.1.2"),
            new Camera(3, "Front door", "192.168.1.3"),
            new Camera(4, "Garage", "192.168.1.4"),
            new Camera(5, "Garden", "192.168.1.5")
        ]

        this.setState({
            items: Array.apply(null, cameras).map((camera, index) => camera)
        });
    }

    remove = (index, camera) => {
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
                <SortableList axis="xy" items={this.state.items} onPowerOff={this.powerOff} onRemove={this.remove} onSortEnd={this.sortEnd} onTakePhoto={this.takePhoto} pressDelay={200} />
            </div>
        );
    }
}