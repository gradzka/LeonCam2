import React, { Component } from 'react';

export class CameraEdition extends Component {
    constructor(props) {
        super(props);
        console.log(this.props.location.state.id)

    }

    render() {
        return (
            <div>
                Hello from camera edition!
            </div>
        )
    }
}