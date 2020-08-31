import React, { Component } from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCaretLeft, faCaretUp, faCaretRight, faCaretDown, faHome, faPowerOff, faCamera, faEdit, faTrash, faExpandAlt } from '@fortawesome/free-solid-svg-icons';
import "./CameraPTZ.css"

const ActionButton = ({ icon, className, onClickAction, size }) => (
    <div xs="auto" className="display-contents no-padding">
        <button className={className} onClick={onClickAction}>
            <FontAwesomeIcon icon={icon} size={size} />
        </button>
    </div>
);

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