import React from 'react'
import { withRouter } from 'react-router'
import RegisterForm from '../parts/forms/Register.jsx'

class Register extends React.Component {
  propTypes: {
    successLink: React.PropTypes.string,
    buttonText: React.PropTypes.string
  }

  register (user, successLink, handleError) {
    fetch('/api/Account/Register',
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

  render () {
    return (
      <RegisterForm
        onProcess={this.register.bind(this)}
        successLink={this.props.successLink}
        buttonText={this.props.buttonText}
      />
    )
  }
}

export default withRouter(Register)
