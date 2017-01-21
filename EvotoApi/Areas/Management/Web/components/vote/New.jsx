import React from 'react'
import { withRouter, Link } from 'react-router'
import WarningModal from './WarningModal.jsx'

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
    save: React.PropTypes.func
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
      state: nonEmptyVote ? props.vote.state : 'draft',
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
      state: this.state.state
    })
  }

  saveVote () {
    const vote = this.makeVote()
    this.props.save ? this.props.save(vote) : this.save(vote)
  }

  save (vote) {
    fetch('/mana/vote/create'
      , { method: 'POST',
        body: JSON.stringify(vote),
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json'
        }
      })
      .then(() => {
        this.props.router.push('/')
      })
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
    const title = this.props.title || 'New Vote'
    const description = this.props.description || 'Create a new vote'
    return (
      <div>
        <WarningModal
          title='Are you sure you want to publish? This cannot be undone.'
          name='warningModal'
          ref='publishModal'
          confirm={this.confirmPublish.bind(this)} />
        
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
    )
  }
}

export default withRouter(NewVote)
