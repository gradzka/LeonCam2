import React, { Component } from 'react';
import { InputBox } from '../Shared/InputBox';
import { pushCard, popCard } from '../Shared/Card';
import { authenticationService } from '../../services/AuthenticationService';
import { Popover, PopoverBody } from 'reactstrap';

export class ForgotPassword extends Component {
    constructor() {
        super();
        this.state = {
            username: '',
            answer: '',
            popoverMessage: '',
            popoverClass: '',
            popoverIsOpen: false,
            getLeadingQuestionPopoverIsOpen: false,
            isSubmitting: false,
            leadingQuestion: ''
        }

        this.handleInputChange = this.handleInputChange.bind(this);
        this.hidePopover = this.hidePopover.bind(this);
    }

    getLeadingQuestion(event) {
        this.setState({
            isSubmitting: true,
            popoverIsOpen: false,
            getLeadingQuestionPopoverIsOpen: false
        });

        if (!this.props.username) {
            this.setState({
                popoverClass: 'popover-error-reversed',
                popoverMessage: "Username is empty",
                getLeadingQuestionPopoverIsOpen: document.activeElement === document.getElementById('forgotPasswordLink')
            });

            return;
        }

        event.persist();

        authenticationService.getLeadingQuestion(this.props.username).then(     
            data => {
                this.setState({
                    leadingQuestion: data.leadingQuestion
                    //popoverClass: 'popover-success-reversed',
                    //popoverMessage: 'Sent',
                    //getLeadingQuestionPopoverIsOpen: document.activeElement === document.getElementById('forgotPasswordLink')
                });

                pushCard(event);
            },
            error => {
                this.setState({
                    popoverClass: 'popover-error-reversed',
                    popoverMessage: error === "Unexpected error" ? "Forgot Password Error" : error,
                    getLeadingQuestionPopoverIsOpen: document.activeElement === document.getElementById('forgotPasswordLink')
                });
            }
        )
    }

    handleInputChange(inputId, value) {
        this.setState({
            [inputId]: value
        });
    }

    componentDidMount() {
        document.body.addEventListener('click', this.hidePopover);
    }

    componentWillUnmount() {
        document.body.removeEventListener('click', this.hidePopover);
    }

    hidePopover() {
        this.setState({
            popoverIsOpen: false,
            getLeadingQuestionPopoverIsOpen: false
        });
    }

    checkAnswer(event) {
        this.setState({
            isSubmitting: true,
            popoverIsOpen: false,
            getLeadingQuestionPopoverIsOpen: false
        });
        authenticationService.checkAnswer(this.state.username, this.state.answer).then(
            data => {
                this.setState({
                    isSubmitting: false,
                    popoverClass: 'popover-success-reversed',
                    popoverMessage: 'Sent',
                    popoverIsOpen: document.activeElement === document.getElementById('checkAnswer')
                });
            },
            error => {
                this.setState({
                    isSubmitting: false,
                    popoverClass: 'popover-error-reversed',
                    popoverMessage: error === "Unexpected error" ? "Forgot Password Error" : error,
                    popoverIsOpen: document.activeElement === document.getElementById('checkAnswer')
                });
            }  
        )
        event.preventDefault();
    }

    render() {
        return (
            <div className="card alt hidden bottom">
                <div id="forgotPasswordLink" className="toggle" tabIndex="0" onClick={this.getLeadingQuestion.bind(this)} onBlur={this.hidePopover}>Forgot password?</div>
                <h1 className="title">Forgot<br />password?
                        <div className="close" tabIndex="0" onClick={popCard.bind(this, 'Forgot password?')}></div>
                </h1>
                <form onSubmit={this.checkAnswer.bind(this)}>
                    <InputBox id="leadingQuestionAnswer" type="text" placeholder={this.state.leadingQuestion} className="alt hidden" value={this.state.answer} onChange={this.handleInputChange} />
                    <div className="button-container">
                        <button id='checkAnswer' disabled={this.state.isSubmitting}><span>Check answer</span></button>
                    </div>

                    <Popover className={this.state.popoverClass} placement='top' isOpen={this.state.popoverIsOpen} target='checkAnswer'>
                        <PopoverBody>{this.state.popoverMessage}</PopoverBody>
                    </Popover>

                    <Popover className={this.state.popoverClass} placement='top' isOpen={this.state.getLeadingQuestionPopoverIsOpen} target='forgotPasswordLink'>
                        <PopoverBody>{this.state.popoverMessage}</PopoverBody>
                    </Popover>
                </form>
            </div>
        );
    }
}