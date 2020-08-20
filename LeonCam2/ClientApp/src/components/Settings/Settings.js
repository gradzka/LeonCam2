import React, { Component } from 'react';
import { FormGroup, Label, Input } from 'reactstrap';
import './Settings.css';

export class Settings extends Component {
    render() {
        return (
            <div>
                <h1>Settings</h1>
                <div className="row">
                    <div className="col-md-6 grid-margin stretch-card">
                        <div className="card">
                            <div className="card-body">
                                <h4 className="card-title">Change username</h4>
                                <form className="forms-sample">
                                    <FormGroup>
                                        <Label htmlFor="changeUsernameNewUsername">Username</Label>
                                        <Input type="text" id="changeUsernameNewUsername" placeholder="Username" size="lg" />
                                    </FormGroup>
                                    <FormGroup>
                                        <Label htmlFor="changeUsernamePassword">Password</Label>
                                        <Input type="password" id="changeUsernamePassword" placeholder="Password" />
                                    </FormGroup>
                                    <button type="submit" className="btn btn-primary mr-2">Change</button>
                                </form>
                            </div>
                        </div>
                    </div>
                    <div className="col-md-6 grid-margin stretch-card">
                        <div className="card">
                            <div className="card-body">
                                <h4 className="card-title">Change password</h4>
                                <form className="forms-sample">
                                    <FormGroup>
                                        <Label htmlFor="changePasswordOldPassword">Username</Label>
                                        <Input type="text" id="changePasswordOldPassword" placeholder="Username" size="lg" />
                                    </FormGroup>
                                    <FormGroup>
                                        <Label htmlFor="changePasswordNewPassword">Password</Label>
                                        <Input type="password" id="changePasswordNewPassword" placeholder="Password" />
                                    </FormGroup>
                                    <FormGroup>
                                        <Label htmlFor="changePasswordConfirmNewPassword">Confirm Password</Label>
                                        <Input type="password" id="changePasswordConfirmNewPassword" placeholder="Password" />
                                    </FormGroup>
                                    <button type="submit" className="btn btn-primary mr-2">Change</button>
                                </form>
                            </div>
                        </div>
                    </div>
                </div>
                <div className="row">
                    <div className="col-md-6 grid-margin stretch-card">
                        <div className="card">
                            <div className="card-body">
                                <h4 className="card-title">Reset account</h4>
                                <p className="card-description">All saved cameras will be removed!</p>
                                <form className="forms-sample">
                                    <FormGroup>
                                        <Label htmlFor="resetAccountPassword">Password</Label>
                                        <Input type="password" id="resetAccountPassword" placeholder="Password" />
                                    </FormGroup>
                                    <button type="submit" className="btn btn-primary mr-2">Reset</button>
                                </form>
                            </div>
                        </div>
                    </div>
                    <div className="col-md-6 grid-margin stretch-card">
                        <div className="card">
                            <div className="card-body">
                                <h4 className="card-title">Delete account</h4>
                                <p className="card-description">Your account with all data will be removed!</p>
                                <form className="forms-sample">
                                    <FormGroup>
                                        <Label htmlFor="deleteAccountPassword">Password</Label>
                                        <Input type="password" id="deleteAccountPassword" placeholder="Password" />
                                    </FormGroup>
                                    <button type="submit" className="btn btn-primary mr-2">Delete</button>
                                </form>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}