import React, { Component } from 'react';
import './Footer.css';

export class Footer extends Component {
    constructor(props) {
        super(props);
    }

    render() {
        return (
            <footer>
                    <ul>
                        <li><b>LeonCam2</b> is the surveillance system with IP network cameras. It supports the ONVIF standard. </li>
                     </ul>                
                    <ul>
                        <li>
                            <span> See source code </span>
                            <a href="https://github.com/gradzka/LeonCam2">
                                <i class="fa fa-github"/>
                            </a>
                        </li>
                        <li className="divider"/>
                        <li>
                            <span> 2020 </span> 
                            <a href="https://github.com/gradzka">Gradzka</a>
                            <span> & </span> 
                            <a href="https://github.com/kazimierczak-robert">Kazimierczak</a>
                        </li>
                        <li className="divider" />
                        <li> 
                            <span> Design by Kuzayova</span>
                            <a href="https://www.facebook.com/kuzayova">
                                <i class="fa fa-facebook-f"/>
                            </a>
                            <a href="https://www.instagram.com/kuzayova.art">
                                <i class="fa fa-instagram"/>
                            </a>
                        </li>
                    </ul>
            </footer>
        );
    }
}