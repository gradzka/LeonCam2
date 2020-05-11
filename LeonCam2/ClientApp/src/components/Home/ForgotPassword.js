import React, { Component } from 'react';
import { InputBox } from '../Shared/InputBox';
import { authenticationService } from '../../services/AuthenticationService';
import { Popover, PopoverBody } from 'reactstrap';

export class ForgotPassword extends Component {
    constructor(props) {
        super(props);
        this.state = {
            username: '',
            leadingQuestionAnswer: '',
            popoverMessage: '',
            popoverClass: '',
            popoverIsOpen: false,
            getLeadingQuestionPopoverIsOpen: false,
            isSubmitting: false,
            leadingQuestion: '',
        }

        this.handleInputChange = this.handleInputChange.bind(this);
        this.hidePopover = this.hidePopover.bind(this);
        this.pushCard = this.pushCard.bind(this);
        this.popCard = this.popCard.bind(this);
    }

    getLeadingQuestion(event) {
        this.setState({
            popoverIsOpen: false,
            getLeadingQuestionPopoverIsOpen: false
        });

        if (!this.props.username) {
            this.setState({
                popoverClass: 'popover-error-reversed',
                popoverMessage: "Username is empty",
                getLeadingQuestionPopoverIsOpen: true
            });

            return;
        }

        event.persist();

        authenticationService.getLeadingQuestion(this.props.username).then(     
            data => {
                this.setState({
                    leadingQuestion: data.leadingQuestion
                });

                this.pushCard();
            },
            error => {
                this.setState({
                    popoverClass: 'popover-error-reversed',
                    popoverMessage: error === "Unexpected error" ? "Forgot Password Error" : error,
                    getLeadingQuestionPopoverIsOpen: true
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
            popoverIsOpen: false
        });
        authenticationService.checkAnswer(this.props.username, this.state.leadingQuestionAnswer).then(
            () => {
                const { from } = this.props.location.state || {
                    from: { pathname: "/dashboard" }
                };
                this.props.history.push(from);
            },
            error => {
                this.setState({
                    isSubmitting: false,
                    popoverClass: 'popover-error-reversed',
                    popoverMessage: error === "Unexpected error" ? "Forgot Password Error" : error,
                    popoverIsOpen: true
                });
            }  
        )
        event.preventDefault();
    }

    pushCard() {
        this.props.onOnTopChanged('forgotPasswordCardOnTop', true);
    }

    popCard() {
        this.props.onOnTopChanged('forgotPasswordCardOnTop', false);
    }

    render() {
        let toggledHidden = "alt" + (this.props.onTop === true ? '' : ' hidden');

        return (
            <div className={"card bottom " + toggledHidden}>
                <div id="forgotPasswordLink" className="toggle" tabIndex="0" onClick={this.getLeadingQuestion.bind(this)}>{this.props.onTop ? '' : 'Forgot password?'}</div>
                <h1 className="title">Forgot<br />password?
                        <div className="close" tabIndex="0" onClick={this.popCard}></div>
                </h1>
                <form onSubmit={this.checkAnswer.bind(this)}>
                    <InputBox id="leadingQuestionAnswer" type="text" placeholder={this.state.leadingQuestion} className={toggledHidden} value={this.state.leadingQuestionAnswer} onChange={this.handleInputChange} />
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