import React, { Component } from 'react';
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faGithub } from "@fortawesome/free-brands-svg-icons";
import './Footer.css';

export class Footer extends Component {
    getCurrentYear() {
        return new Date().getFullYear();
    }

    render() {
        return (
            <footer>
                <ul>
                    <li><b>LeonCam2</b> is the surveillance system with IP network cameras that support ONVIF standard</li>
                </ul>
                <ul>
                    <li>
                        <span>See source code</span>
                        <a href="https://github.com/gradzka/LeonCam2">
                            <FontAwesomeIcon icon={faGithub} size="2x" />
                        </a>
                    </li>
                    <li className="divider" />
                    <li>
                        <span>{this.getCurrentYear()}</span>
                        <a href="https://github.com/gradzka">Gradzka</a>
                        <span> & </span>
                        <a href="https://github.com/kazimierczak-robert">Kazimierczak</a>
                    </li>
                </ul>
            </footer>
        );
    }
}