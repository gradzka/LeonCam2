import React, { Component } from 'react';
import { Camera } from "../Shared/Camera";
import { CameraBox } from "../Shared/CameraBox";

export class CameraFullScreen extends Component {
    constructor(props) {
        super(props);
        console.log(this.props.location.state.id)

        this.state = {
            camera: new Camera(-1, "Reading data...", "")
        }

        this.getCamera = this.getCamera.bind(this);
    }

    componentDidMount() {
        this.getCamera();
    }

    getCamera() {
        //Get camera by id: TODO
        this.setState({
            camera: new Camera(1, "Baby room", "192.168.1.1")
        });
    }

    powerOff = () => {

    }

    remove = () => {

    }

    takePhoto = (camera) => {

    }

    render() {
        return (
            <CameraBox camera={this.state.camera} onPowerOff={this.powerOff} onRemove={this.remove} onTakePhoto={this.takePhoto} />
        )
    }
}