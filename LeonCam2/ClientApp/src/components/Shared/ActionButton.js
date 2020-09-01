import React from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';

export const ActionButton = ({ icon, className, onClickAction, size }) => (
    <div xs="auto" className="display-contents no-padding">
        <button className={className} onClick={onClickAction}>
            <FontAwesomeIcon icon={icon} size={size} />
        </button>
    </div>
);