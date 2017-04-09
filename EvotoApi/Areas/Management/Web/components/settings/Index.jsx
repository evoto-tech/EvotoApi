import React from 'react'
import Wrapper from '../parts/Wrapper.jsx'
import RegisterEnabled from './parts/RegisterEnabled.jsx'
import TwoFactorEnabled from './parts/TwoFactorEnabled.jsx'
import CustomFields from './parts/CustomFields.jsx'
import ClientCustomisation from './parts/ClientCustomisation.jsx'

class SettingsIndex extends React.Component {
  constructor (props) {
    super(props)
    this.state = { loaded: false, settings: [] }
  }

  componentDidMount () {
    fetch(`/regi/settings/list`, { credentials: 'same-origin' })
      .then((res) => res.json())
      .then((data) => {
        this.setState({ loaded: true, settings: data })
      })
      .catch(console.error)
  }

  getSetting (name) {
    return this.state.settings.find((s) => s.name && s.name === name)
  }

  render () {
    return (
      <Wrapper
        title='Settings'
        description='Settings for the evoto client and end users.'
      >
        <RegisterEnabled
          loaded={this.state.loaded}
          name='User Registration'
          setting={this.getSetting('User Registration')}
        />
        <CustomFields />
        <TwoFactorEnabled />
        <ClientCustomisation />
      </Wrapper>
    )
  }
}

export default SettingsIndex
