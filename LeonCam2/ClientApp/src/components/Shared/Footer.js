import React, { Component } from 'react';
import './Footer.css';

export class Footer extends Component {
    constructor(props) {
        super(props);
    }

    getCurrentYear() {
        return new Date().getFullYear();
    }

    render() {
        return (
            <footer>
                <ul>
                    <li><b>LeonCam2</b> is the surveillance system with IP network cameras. It supports the ONVIF standard. </li>
                </ul>
                <ul>
                    <li>
                        <span>See source code</span>
                        <a href="https://github.com/gradzka/LeonCam2">
                            <i className="fa fa-github" />
                        </a>
                    </li>
                    <li className="divider" />
                    <li>
                        <span>{this.getCurrentYear()}</span>
                        <a href="https://github.com/gradzka">Gradzka</a>
                        <span> & </span>
                        <a href="https://github.com/kazimierczak-robert">Kazimierczak</a>
                    </li>
                    <li className="divider" />
                    <li>
                        <span>Design by Kuzayova</span>
                        <a href="https://www.facebook.com/kuzayova">
                            <i className="fa fa-facebook-f" />
                        </a>
                        <a href="https://www.instagram.com/kuzayova.art">
                            <i className="fa fa-instagram" />
                        </a>
                    </li>
                </ul>
            </footer>
        );
    }
}