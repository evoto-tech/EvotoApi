import React from 'react'
import { withRouter, Link } from 'react-router'
import Question from './parts/Question.jsx'
import LoadableOverlay from '../parts/LoadableOverlay.jsx'

class NewVote extends React.Component {
  constructor (props) {
    super(props)
    this.state = Object.assign({}, this.stateFromProps(props), { errors: {} })
  }

  propTypes: {
    vote: React.PropTypes.object,
    loaded: React.PropTypes.bool,
    title: React.PropTypes.string,
    description: React.PropTypes.string,
    save: React.PropTypes.func,
    disabled: React.PropTypes.bool
  }

  contextTypes: {
    router: React.PropTypes.object
  }

  stateFromProps (props) {
    const nonEmptyVote = props.vote && (Object.keys(props.vote).length > 0)
    return {
      user: nonEmptyVote ? { id: props.vote.createdBy } : { id: 2 },
      name: nonEmptyVote ? props.vote.name : '',
      expiryDate: nonEmptyVote ? props.vote.expiryDate : '',
      published: nonEmptyVote ? props.vote.published : false,
      questions: nonEmptyVote ? (JSON.parse(props.vote.questions) || []) : [],
      loaded: props.hasOwnProperty('loaded') ? props.loaded : true
    }
  }

  componentWillReceiveProps (nextProps) {
    this.setState(this.stateFromProps(nextProps))
  }

  componentDidMount () {
    this.loadExpiryDateTimePicker()
    $('#expiryDate').on('dp.change', this.handleDateTimeChange.bind(this))
  }

  componentDidUpdate () {
    this.loadExpiryDateTimePicker()
  }

  loadExpiryDateTimePicker () {
    const expiryDate = this.state.expiryDate
    $(function () {
      $('#expiryDate').datetimepicker({
        inline: true,
        sideBySide: true
      })
      if (expiryDate) $('#expiryDate').data('DateTimePicker').date(moment(expiryDate))
    })
  }

  handleNameChange (e) {
    this.setState({ name: e.target.value })
  }

  handleDateTimeChange (e) {
    this.setState({ expiryDate: e.date.format() })
  }

  isValid () {
    const vote = this.makeVote()
    const expectedKeys = [ 'createdBy', 'expiryDate', 'name' ]
    let errors = {}
    const valid = expectedKeys.every((k) => {
      let propValid = vote.hasOwnProperty(k) && vote[k] !== ''
      if (!propValid) errors[k] = (`This is required!`)
      return propValid
    })
    this.setState({ errors })
    return valid
  }

  clearErrors () {
    this.setState({ errors: {} })
  }

  makeVote () {
    return ({
      createdBy: this.state.user.id,
      name: this.state.name,
      expiryDate: this.state.expiryDate,
      published: this.state.published,
      questions: JSON.stringify(this.state.questions)
    })
  }

  saveVote () {
    const vote = this.makeVote()
    this.props.save
      ? this.props.save(vote, this.postSave.bind(this), this.showErrors)
      : this.save(vote, this.postSave.bind(this), this.showErrors)
  }

  save (vote, postSave) {
    fetch('/mana/vote/create',
      { method: 'POST',
        body: JSON.stringify(vote),
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json'
        },
        credentials: 'same-origin'
      })
      .then(postSave)
      .catch((err) => {
        console.error(err)
      })
  }

  showErrors (errors) {
    let errorMessage = (typeof (errors) === 'string') ? errors : errors.join('\n')
    swal({
      title: 'Errors',
      text: errorMessage,
      type: 'error',
      confirmButtonColor: '#DD6B55',
      allowOutsideClick: true
    })
  }

  postSave () {
    if (this.state.published) swal('Vote successfully published!')
    this.props.router.push('/')
  }

  checkValid (action) {
    this.clearErrors()
    if (this.isValid()) {
      action()
    }
  }

  saveDraft () {
    this.setState({ published: false }, () => {
      this.checkValid(this.saveVote.bind(this))
    })
  }

  savePublish () {
    this.checkValid(this.swalPublishAlert.bind(this))
  }

  confirmPublish () {
    this.setState({ published: true }, () => {
      this.saveVote()
    })
  }

  swalPublishAlert () {
    swal({
      title: 'Are you sure?',
      text: `'${this.state.name}' will be published. Once published it cannot be edited or deleted.`,
      type: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#DD6B55',
      confirmButtonText: 'Publish',
      closeOnConfirm: false,
      allowOutsideClick: true,
      showLoaderOnConfirm: true
    },
    () => {
      this.confirmPublish()
    })
  }

  cancel () {
    if (window.isDirty) {
      swal({
        title: 'You will lose any changes that have been made',
        type: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#DD6B55'
      },
      () => {
        this.props.router.push('/')
      })
    } else {
      this.props.router.push('/')
    }
  }

  addQuestion () {
    let questions = this.state.questions
    questions.push({
      question: '',
      options: []
    })
    this.setState({ questions })
  }

  updateQuestion (index, question) {
    let questions = [].concat(this.state.questions)
    questions[index] = question
    this.setState({ questions })
  }

  deleteQuestion (index) {
    let questions = [].concat(this.state.questions)
    questions.splice(index, 1)
    this.setState({ questions })
  }

  render () {
    const title = this.props.title || 'New Vote'
    const description = this.props.description || 'Create a new vote'
    return (
      <div className='content-wrapper' style={{ height: '100%' }}>
        <section className='content-header' style={{ height: '100%' }}>
          <h1>{title}<small>{description}</small></h1>
          <ol className='breadcrumb'>
            <li>
              <Link to='/vote/new'><i className='fa fa-plus' />New Vote</Link>
            </li>
          </ol>
        </section>
        <section className='content'>
          {this.props.children}
          <div className='box box-success'>
            <LoadableOverlay loaded={this.state.loaded} />
            <div className='box-header with-border'>
              <h3 className='box-title'>{title} Details</h3>
            </div>
            <form role='form'>
              <div className='box-body'>
                <div className={this.state.errors.name ? 'form-group has-error' : 'form-group'}>
                  <label htmlFor='voteName'>Name</label>
                  <input
                    type='text'
                    className='form-control'
                    id='voteName'
                    placeholder='Enter vote name'
                    value={this.state.name}
                    onChange={this.handleNameChange.bind(this)}
                    disabled={this.props.disabled}
                  />
                  <span className='help-block'>{this.state.errors.name}</span>
                </div>
                <div className={this.state.errors.expiryDate ? 'form-group has-error' : 'form-group'}>
                  <label>End Date</label>
                  <div style={{ overflow: 'hidden' }}>
                    <div className='form-group'>
                      <div className='row'>
                        <div className='col-md-offset-3 col-md-6 col-lg-offset-4 col-lg-4'>
                          <div id='expiryDate' className={this.props.disabled ? 'disabled-datetimepicker' : ''}>
                            <input type='hidden' />
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
                  <span className='help-block'>{this.state.errors.expiryDate}</span>
                </div>
                <div className={this.state.errors.questions ? 'form-group has-error' : 'form-group'}>
                  <label>Questions</label>
                  <div className='form-group'>
                    <div className='row'>
                      <div className='col-md-offset-2 col-md-8'>
                        {this.state.questions.map((question, i) => (
                          <Question
                            question={question}
                            id={i}
                            key={i}
                            onDelete={this.deleteQuestion.bind(this)}
                            onChange={this.updateQuestion.bind(this, i)}
                            disabled={this.props.disabled}
                            />
                        ))}
                      </div>
                    </div>
                  </div>
                  {this.props.disabled ? '' : (
                    <button type='button' className='btn btn-success' onClick={this.addQuestion.bind(this)}>New Question</button>
                  )}
                  <span className='help-block'>{this.state.errors.questions}</span>
                </div>
              </div>
              {!this.props.disabled || (this.props.disabled && this.props.vote.published) ? '' : (
                <div className='box-footer'>
                  <div className='btn-group'>
                    <Link to={`/vote/${this.props.vote.id}/edit`}>
                      <button type='button' className='btn btn-success'>Edit</button>
                    </Link>
                  </div>
                </div>
              )}
              {this.props.disabled ? '' : (
                <div className='box-footer'>
                  <div className='btn-group'>
                    <button type='button' className='btn btn-danger' onClick={this.cancel.bind(this)}>Cancel</button>
                    <button type='button' className='btn' onClick={this.saveDraft.bind(this)}>Save as a Draft</button>
                    <button type='button' className='btn btn-success' onClick={this.savePublish.bind(this)}>Save and Publish</button>
                  </div>
                </div>
              )}
            </form>
          </div>
        </section>
      </div>
    )
  }
}

export default withRouter(NewVote)
