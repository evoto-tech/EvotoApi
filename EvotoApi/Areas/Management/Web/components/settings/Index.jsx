import React from 'react'
import Wrapper from '../parts/Wrapper.jsx'
import RegisterEnabled from './parts/RegisterEnabled.jsx'
import TwoFactorEnabled from './parts/TwoFactorEnabled.jsx'
import CustomFields from './parts/CustomFields.jsx'
import ClientCustomisation from './parts/ClientCustomisation.jsx'

class SettingsIndex extends React.Component {
  componentDidMount () {

  }

  render () {
    return (
      <Wrapper
        title='Settings'
        description='Settings for the evoto client and end users.'
      >
        <RegisterEnabled />
        <TwoFactorEnabled />
        <CustomFields />
        <ClientCustomisation />
      </Wrapper>
    )
  }
}

export default SettingsIndex
