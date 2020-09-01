import React, { Component } from 'react';
import { faCaretLeft, faCaretUp, faCaretRight, faCaretDown, faHome} from '@fortawesome/free-solid-svg-icons';
import "./CameraPTZ.css"
import { ActionButton } from '../Shared/ActionButton';

export class CameraPTZ extends Component {
    constructor(props) {
        super(props);
        this.state = {

        }
    }

    render() {
        return ( 
            <div className={this.props.className}>
                <div />
                <ActionButton icon={faCaretUp} size="2x" className="ptz-control" />
                <div />
                <ActionButton icon={faCaretLeft} size="2x" className="ptz-control" />
                <ActionButton icon={faHome} className="circle" />
                <ActionButton icon={faCaretRight} size="2x" className="ptz-control" />
                <div />
                <ActionButton icon={faCaretDown} size="2x" className="ptz-control" />
                <div />
            </div>
        );
    }
}