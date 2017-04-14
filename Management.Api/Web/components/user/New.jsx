import React from 'react'
import Wrapper from './parts/Wrapper.jsx'
import RegisterForm from '../parts/forms/Register.jsx'
import NamedInput from '../parts/form-components/NamedInput.jsx'
import cleanValidationJson from '../../lib/clean-validation-json'

class UserNew extends React.Component {
  constructor (props) {
    super(props)
    this.cleanValidationJson = cleanValidationJson
    this.state = { loaded: false, customFields: [], user: {} }
  }

  componentDidMount () {
    fetch(`/regi/settings/custom-fields`, { credentials: 'same-origin' })
      .then((res) => res.json())
      .then(this.cleanValidationJson)
      .then((data) => {
        const user = data.reduce((user, field) => Object.assign(user, { [field.name]: '' }), {})
        this.setState({ customFields: data, loaded: true, user })
      })
      .catch(console.error)
  }

  register (user, successLink, handleError) {
    user.CustomFields = this.state.customFields.map((field) => {
      if (this.state.user.hasOwnProperty(field.name) && field.type === 'Date') {
        return { name: field.name, value: moment(this.state.user[field.name]).format() }
      } else if (this.state.user.hasOwnProperty(field.name)) {
        return { name: field.name, value: this.state.user[field.name] }
      }
    })
    fetch('/regi/account/register',
      { method: 'POST',
        body: JSON.stringify(user),
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json'
        },
        credentials: 'same-origin'
      })
      .then((response) => {
        if (response.ok && response.status === 200) {
          // Do nothing
        } else if (response.status === 401) {
          throw new Error('Those details seem to be wrong! Please try again.')
        } else if (response.status === 400) {
          return response.json()
        } else {
          throw new Error('An unknown error occurred')
        }
      }, handleError)
      .then((data) => {
        if (data) {
          let messages = data.ModelState
              ? Object.keys(data.ModelState).reduce((o, k) => data.ModelState[k], [])
              : undefined
          return handleError(data, messages)
        } else {
          if (successLink) this.props.router.push(successLink)
        }
      }, handleError)
      .catch(handleError)
  }

  updateField (field, type, e) {
    let value = e.target.value
    const user = Object.assign({}, this.state.user, { [field]: value })
    this.setState({ user })
  }

  renderCustomFields () {
    if (!this.state.customFields || this.state.customFields.length < 1) return ''
    const elements = this.state.customFields.reduceRight((elements, element, i) => {
      return [].concat([ element ]).concat([ 'br' ]).concat(elements)
    })
    return elements.map((field, i) => {
      if (typeof field === 'string') return <br key={i} />
      let type = (field.type || 'text').toLowerCase()
      if (type === 'string') type = 'text'
      return (
        <NamedInput
          key={i}
          name={field.name}
          type={type}
          value={this.state.user[field.name]}
          onChange={this.updateField.bind(this, field.name, field.type)}
        />
      )
    })
  }

  render () {
    const title = 'New User'
    const description = 'New User'
    const customFields = this.renderCustomFields()
    return (
      <Wrapper title={title} description={description}>
        <div className='box box-success'>
          <div className='box-header with-border'>
            <h3 className='box-title'>New User Details</h3>
          </div>
          <div className='box-body'>
            <RegisterForm
              onProcess={this.register.bind(this)}
              successLink='/users'
              buttonText='Create New User'
            >
              {customFields ? <div>{customFields}</div> : ''}
            </RegisterForm>
          </div>
        </div>
      </Wrapper>
    )
  }
}

export default UserNew
