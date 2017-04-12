import React from 'react'
import PropTypes from 'prop-types'
import { withRouter, Link } from 'react-router'
import Slider from 'react-rangeslider'
import uppercamelcase from 'uppercamelcase'
import Question from './parts/Question.jsx'
import LoadableOverlay from '../parts/LoadableOverlay.jsx'
import { insert, update, remove } from '../../lib/state-utils'

class NewVote extends React.Component {
  constructor (props) {
    super(props)
    this.state = Object.assign({}, this.stateFromProps(props), { errors: {} })
  }

  contextTypes: {
    router: React.PropTypes.object
  }

  stateFromProps (props) {
    const nonEmptyVote = props.vote && (Object.keys(props.vote).length > 0)
    return {
      user: nonEmptyVote ? { id: props.vote.createdBy } : { id: 2 },
      name: nonEmptyVote ? props.vote.name : '',
      chainString: nonEmptyVote ? (props.vote.chainString || '') : '',
      expiryDate: nonEmptyVote ? props.vote.expiryDate : '',
      published: nonEmptyVote ? props.vote.published : false,
      questions: nonEmptyVote ? (JSON.parse(props.vote.questions) || []) : [],
      loaded: props.hasOwnProperty('loaded') ? props.loaded : true,
      encrypted: nonEmptyVote ? props.vote.encrypted : true,
      blockSpeed: nonEmptyVote ? props.vote.blockSpeed : 30,
      publishedDate: nonEmptyVote ? props.vote.publishedDate : null
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
    const publishedDate = this.state.publishedDate
    $(function () {
      $('#expiryDate').datetimepicker({
        inline: true,
        sideBySide: true
      })
      if (expiryDate) $('#expiryDate').data('DateTimePicker').date(moment(expiryDate))
      if (publishedDate) {
        $('#publishedDate').datetimepicker({
          inline: true,
          sideBySide: true
        })
        if (publishedDate) $('#publishedDate').data('DateTimePicker').date(moment(publishedDate))
      }
    })
  }

  handleNameChange (e) {
    const update = { name: e.target.value }
    if (uppercamelcase(this.state.name) === this.state.chainString) {
      update.chainString = uppercamelcase(update.name)
    }
    this.setState(update)
  }

  handleChainStringChange (e) {
    this.setState({ chainString: e.target.value })
  }

  handleDateTimeChange (e) {
    this.setState({ expiryDate: e.date.format() })
  }

  handleEncryptedChange (e) {
    this.setState({ encrypted: e.target.checked })
  }

  handleBlockSpeedChange (val) {
    this.setState({ blockSpeed: val })
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
      chainString: this.state.chainString,
      expiryDate: this.state.expiryDate,
      published: this.state.published,
      questions: JSON.stringify(this.state.questions),
      encrypted: this.state.encrypted,
      blockSpeed: this.state.blockSpeed
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
      .then((data) => {
        return data.json()
      })
      .then((json) => {
        if (json.errors) {
          showErrors(json.errors)
        } else {
          postSave()
        }
      })
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
    this.setState({ questions: insert(this.state.questions, { question: '', options: [] }) })
  }

  updateQuestion (index, question) {
    this.setState({ questions: update(this.state.questions, index, question) })
  }

  deleteQuestion (index) {
    this.setState({ questions: remove(this.state.questions, index) })
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
                <div className={this.state.errors.chainString ? 'form-group has-error' : 'form-group'}>
                  <label htmlFor='chainString'>Blockchain Name</label>
                  <input
                    type='text'
                    className='form-control'
                    id='chainString'
                    placeholder='Enter blockchain name'
                    value={this.state.chainString}
                    onChange={this.handleChainStringChange.bind(this)}
                    disabled={this.props.disabled}
                  />
                  <span className='help-block'>{this.state.errors.chainString}</span>
                </div>
                {!this.state.published ? '' : (
                  <div className='form-group'>
                    <label>Published Date</label>
                    <div style={{ overflow: 'hidden' }}>
                      <div className='form-group'>
                        <div className='row'>
                          <div className='col-md-offset-3 col-md-6 col-lg-offset-4 col-lg-4'>
                            <div id='publishedDate' className='disabled-datetimepicker'>
                              <input type='hidden' />
                            </div>
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
                )}
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
                <div className={this.state.errors.encrypted ? 'form-group has-error' : 'form-group'}>
                  <div className='checkbox'>
                    <input
                      type='checkbox'
                      id='encryptResults'
                      checked={this.state.encrypted}
                      onChange={this.handleEncryptedChange.bind(this)}
                      disabled={this.props.disabled}
                    />
                    <label htmlFor='encryptResults'>Encrypt Results?</label>
                    <span className='help-block'>{this.state.errors.name}</span>
                  </div>
                </div>
                <div className={this.state.errors.blockSpeed ? 'form-group has-error' : 'form-group'}>
                  <label htmlFor='blockSpeed'>Block Speed</label>
                  <div className='value'>{this.state.blockSpeed}</div>
                  {(!this.props.disabled)
                    ? <Slider
                      id='blockSpeed'
                      value={this.state.blockSpeed}
                      onChange={this.handleBlockSpeedChange.bind(this)}
                      step={5}
                      min={5}
                      max={110}
                      labels={{5: 'Fast, Larger, More secure', 100: 'Slow, Smaller, Less secure'}}
                      disabled={this.props.disabled}
                  />
                  : ''}
                  <span className='help-block'>{this.state.errors.name}</span>
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

NewVote.propTypes = {
  vote: PropTypes.object,
  loaded: PropTypes.bool,
  title: PropTypes.string,
  description: PropTypes.string,
  save: PropTypes.func,
  disabled: PropTypes.bool
}

export default withRouter(NewVote)
