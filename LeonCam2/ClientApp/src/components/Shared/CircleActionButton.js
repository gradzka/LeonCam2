import React from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { Col } from 'reactstrap';

import "./CircleActionButton.css"

export const CircleActionButton = ({ id, icon, className, onClickAction }) => (
    <Col xs="auto" className="padding-right-left-5" >
        <button id={id} className={"circle " + className} onClick={onClickAction}>
            <FontAwesomeIcon icon={icon} />
        </button>
    </Col>
);