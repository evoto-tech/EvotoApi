import React from 'react'
import { Link } from 'react-router'
import WarningModal from './WarningModal.jsx'

class VoteList extends React.Component {
  constructor (props) {
    super(props)
    this.state = {
      user: { id: 2 },
      name: '',
      expiryDate: '',
      state: 'draft',
      errors: {}
    }
  }

  componentDidMount () {
    this.loadExpiryDateTimePicker()
    $('#expiryDate').on('dp.change', this.handleDateTimeChange.bind(this))
  }

  componentDidUpdate () {
    this.loadExpiryDateTimePicker()
  }

  loadExpiryDateTimePicker () {
    $(function () {
      $('#expiryDate').datetimepicker({
        inline: true,
        sideBySide: true
      })
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
      state: this.state.state
    })
  }

  saveVote () {
    const vote = this.makeVote()
    fetch('/mana/vote/create'
      , { method: 'POST',
        body: JSON.stringify(vote),
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json'
        }
      })
      .then((res) => {
        console.log('res', res)
        return res.json()
      })
      .then((data) => console.log)
      .catch((err) => {
        console.error(err)
      })
  }

  checkValid (action) {
    this.clearErrors()
    if (this.isValid()) {
      action()
    }
  }

  saveDraft () {
    this.setState({ state: 'draft' }, () => {
      this.checkValid(this.saveVote.bind(this))
    })
  }

  savePublish () {
    this.checkValid(this.refs.publishModal.show.bind(this.refs.publishModal))
  }

  confirmPublish () {
    this.setState({ state: 'published' }, () => {
      this.saveVote()
    })
  }

  render () {
    return (
      <div className='content-wrapper' style={{ height: '100%' }}>
        <WarningModal
          title='Are you sure you want to publish? This cannot be undone.'
          name='warningModal'
          ref='publishModal'
          confirm={this.confirmPublish.bind(this)} />
        <section className='content-header' style={{ height: '100%' }}>
          <h1>New Vote<small>Create a new vote for your organisation</small></h1>
          <ol className='breadcrumb'>
            <li>
              <Link to='/vote/new'><i className='fa fa-plus' />New Vote</Link>
            </li>
          </ol>
        </section>
        <section className='content'>
          <div className='box box-primary'>
            <div className='box-header with-border'>
              <h3 className='box-title'>New Vote Details</h3>
            </div>
            <form role='form'>
              <div className='box-body'>
                <div className={this.state.errors.name ? 'form-group has-error' : 'form-group'}>
                  <label htmlFor='voteName'>Name</label>
                  <input type='text' className='form-control' id='voteName' placeholder='Enter vote name' value={this.state.name} onChange={this.handleNameChange.bind(this)} />
                  <span className='help-block'>{this.state.errors.name}</span>
                </div>
                <div className={this.state.errors.expiryDate ? 'form-group has-error' : 'form-group'}>
                  <label>End Date</label>
                  <div style={{ overflow: 'hidden' }}>
                    <div className='form-group'>
                      <div className='row'>
                        <div className='col-md-4'>
                          <div id='expiryDate' />
                        </div>
                      </div>
                    </div>
                  </div>
                  <span className='help-block'>{this.state.errors.expiryDate}</span>
                </div>
              </div>
              <div className='box-footer'>
                <div className='btn-group'>
                  <button type='button' className='btn' onClick={this.saveDraft.bind(this)}>Save as a Draft</button>
                  <button type='button' className='btn btn-success' onClick={this.savePublish.bind(this)}>Save and Publish</button>
                </div>
              </div>
            </form>
          </div>
        </section>
      </div>
    )
  }
}

export default VoteList
