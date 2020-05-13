import React, { Component } from 'react';
import './InputBox.css';
import './PasswordBox.css';

export class PasswordBox extends Component {

    constructor(props) {
        super(props);
        this.handleChange = this.handleChange.bind(this);
    }

    componentDidMount() {
        if (this.props.withPasswordStrength) {
            document.getElementById(this.props.id).addEventListener('keyup', this.checkPasswordStrength);
        }
    }

    componentWillUnmount() {
        if (this.props.withPasswordStrength) {
            document.getElementById(this.props.id).removeEventListener('keyup', this.checkPasswordStrength);
        }
    }

    componentDidUpdate() {
        if (this.props.withPasswordStrength) {
            if (this.props.value === "")
            {
                let bar = document.getElementById(this.props.id).parentNode.getElementsByClassName('password-strength-bar')[1];
                bar.classList.remove(bar.classList[1]);
            }
        }
    }

    checkPasswordStrength(event) {
        const hasNumber = value => {
            return new RegExp(/[0-9]/).test(value);
        };
        const hasMixed = value => {
            return new RegExp(/[a-z]/).test(value) && new RegExp(/[A-Z]/).test(value);
        };
        const hasSpecial = value => {
            return new RegExp(/[!#@$%^&*)(+=._-]/).test(value);
        }

        let bar = event.target.parentNode.getElementsByClassName('password-strength-bar')[1];
        bar.classList.remove(bar.classList[1]);

        let passwordValue = event.target.value;
        if (passwordValue.length > 0) {
            let strengths = 0;
            if (passwordValue.length > 5) {
                strengths++;
            }
            if (passwordValue.length > 7) {
                strengths++;
            }
            if (hasNumber(passwordValue)) {
                strengths++;
            }
            if (hasSpecial(passwordValue)) {
                strengths++;
            }
            if (hasMixed(passwordValue)) {
                strengths++;
            }

            if (strengths === 5) {
                bar.classList.add('very-strong');
            }
            else if (strengths === 4) {
                bar.classList.add('strong');
            }
            else if (strengths === 3) {
                bar.classList.add('normal');
            }
            else if (strengths === 2) {
                bar.classList.add('weak');
            }
            else {
                bar.classList.add('very-weak');
            }
        }
    }

    handleChange(event) {
        if (typeof this.props.onChange === "function") {
            const text = event.target.value;
            this.props.onChange(this.props.id, text);
        }
    }

    hidePassword(event) {
        event.target.classList.remove("fa-eye");
        event.target.classList.add("fa-eye-slash");
        event.target.parentNode.parentNode.childNodes[0].type = 'password';
    }

    showPassword(event) {
        event.target.classList.remove("fa-eye-slash");
        event.target.classList.add("fa-eye");
        event.target.parentNode.parentNode.childNodes[0].type = 'text';
    }

    toggleCapsLkWarning(event) {
        let inputContainer = event.target.parentNode;

        if (event.getModifierState('CapsLock')) {
            inputContainer.childNodes[1].childNodes[1].style.visibility = "visible";
        }
        else {
            inputContainer.childNodes[1].childNodes[1].style.visibility = "hidden";
        }
    }

    hideCapsLkWarning(event) {
        let inputContainer = event.target.parentNode;
        inputContainer.childNodes[1].childNodes[1].style.visibility = "hidden";
    }

    render() {
        let passwordStrength;
        if (this.props.withPasswordStrength) {
            passwordStrength = <div className="password-strength-container">
                <div className="password-strength-bar"></div>
                <div className="password-strength-bar"></div>
            </div>
        }

        return (
            <div className={"input-container password-container " + (this.props.className === undefined ? '' : this.props.className)}>
                <input type="password" id={this.props.id} required="required" onKeyDown={this.toggleCapsLkWarning} onBlur={this.hideCapsLkWarning} value={this.props.value} onChange={this.handleChange} autoComplete={this.props.autoComplete || "on"}/>
                <label forhtml={this.props.id}>
                    {this.props.placeholder} 
                    <span id="capsLk" className="badge badge-warning">CapsLk</span>
                </label>
                <div className="input-group-addon">
                    <span className="fa fa-eye-slash" aria-hidden="true" onMouseDown={this.showPassword} onTouchStart={this.showPassword} onMouseUp={this.hidePassword} onMouseOut={this.hidePassword} onTouchEnd={this.hidePassword} />
                </div>
                <div className="bar"></div>
                {passwordStrength}
            </div>
        );
    }
}