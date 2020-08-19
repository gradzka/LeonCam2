import React, { Component } from 'react';

export class CameraFullScreen extends Component {
    constructor(props) {
        super(props);
        console.log(this.props.location.state.id)
    }

    render() {
        return (
            <div>
                Hello from camera full screen!
            </div>
        )
    }
}