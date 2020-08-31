import React, { Component } from 'react';
import { Layout } from './components/Layout';
import { Home } from './components/Home/Home';
import { PublicRoute } from './router/PublicRoute';
import { PrivateRoute } from './router/PrivateRoute';
import { Dashboard } from './components/Dashboard/Dashboard';
import { Settings } from './components/Settings/Settings';
import { CameraEdition } from './components/CameraEdition/CameraEdition';
import { CameraFullScreen } from './components/CameraFullScreen/CameraFullScreen';

import './custom.css'

export default class App extends Component {
    static displayName = App.name;

    render() {
        return (
            <Layout>
                <PublicRoute exact path='/' component={Home} />
                <PrivateRoute path='/dashboard' component={Dashboard} />
                <PrivateRoute path='/settings' component={Settings} />
                <PrivateRoute path='/cameraedition' component={CameraEdition} />
                <PrivateRoute path='/camerafullscreen' component={CameraFullScreen} />
            </Layout>
        );
    }
}
