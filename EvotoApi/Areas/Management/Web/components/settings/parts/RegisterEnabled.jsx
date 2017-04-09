import React from 'react'
import Box from '../../parts/Box.jsx'
import LoadableOverlay from '../../parts/LoadableOverlay.jsx'
import NamedInput from '../../parts/form-components/NamedInput.jsx'

class RegisterEnabled extends React.Component {
  constructor (props) {
    super(props)
    this.state = Object.assign({}, this.stateFromProps(props))
  }

  stateFromProps (props) {
    return {
      enabled: props.setting ? props.setting.value === 'true' : false,
      message: this.state ? (this.state.message || '') : ''
    }
  }

  componentDidMount () {
    $(this.enableRegistrationCheckbox).iCheck({
      checkboxClass: 'icheckbox_flat-green',
      radioClass: 'icheckbox_flat-green'
    })
    $(this.enableRegistrationCheckbox).on('ifChanged', (e) => {
      this.setState({ enabled: e.target.checked })
    })
  }

  componentWillReceiveProps (nextProps) {
    this.setState(this.stateFromProps(nextProps), () => {
      const checkState = this.state.enabled ? 'check' : 'uncheck'
      $(this.enableRegistrationCheckbox).iCheck(checkState)
    })
  }

  save (e) {
    this.setState({ message: '' })
    fetch('/regi/settings',
      { method: 'POST',
        body: JSON.stringify(
          { name: this.props.name,
            value: `${this.state.enabled}`
          }),
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json'
        }
      })
      .then((data) => {
        if (data.status !== 200) {
          this.setState({ message: 'There was a problem saving.' })
        }
        return data.json()
      })
      .then((json) => {
        if (json) {
          this.setState({ message: 'Saved successfully!' })
        }
      })
      .catch((err) => {
        console.error(err)
        this.setState({ message: 'There was a problem saving.' })
      })
  }

  render () {
    const overlay = (<LoadableOverlay loaded={this.props.loaded} />)
    const footer = (
      <div>
        <div className='btn-group'>
          <button type='button' className='btn btn-success' onClick={this.save.bind(this)}>Save</button>
        </div>
        <span className='help-block' style={{ display: 'inline-block', marginLeft: '1em' }}>
          {this.state.message}
        </span>
      </div>
    )
    return (
      <Box
        type='success'
        title='Enable User Registration in Client'
        overlay={overlay}
        footer={footer}
      >
        <form>
          <NamedInput
            name='Enable user registration through the evoto client'
            type='checkbox'
            className='icheckbox_flat-green'
            spanStyle={{ width: 'auto' }}
            inputRef={(input) => { this.enableRegistrationCheckbox = input }}
          />
        </form>
      </Box>
    )
  }
}

RegisterEnabled.propTypes = {
  loaded: React.PropTypes.bool,
  setting: React.PropTypes.object
}

export default RegisterEnabled
