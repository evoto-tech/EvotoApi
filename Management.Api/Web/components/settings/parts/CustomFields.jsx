import React from 'react'
import Box from '../../parts/Box.jsx'
import LoadableOverlay from '../../parts/LoadableOverlay.jsx'
import CustomField from './parts/CustomField.jsx'
import { insert, update, remove } from '../../../lib/state-utils'
import cleanValidationJson from '../../../lib/clean-validation-json'

class CustomFields extends React.Component {
  constructor (props) {
    super(props)
    this.cleanValidationJson = cleanValidationJson
    this.state = { loaded: false, customFields: [], message: '' }
  }

  componentDidMount () {
    fetch(`/regi/settings/custom-fields`, { credentials: 'same-origin' })
      .then((data) => {
        if (data.status !== 200) {
          this.swalLoadErrorAlert()
        }
        return data.json()
      })
      .then(this.cleanValidationJson)
      .then((data) => {
        this.setState({ customFields: data, loaded: true })
      })
      .catch(console.error)
  }

  swalLoadErrorAlert () {
    swal({
      title: 'Error',
      text: 'There was a problem loading the custom fields.',
      type: 'error',
      confirmButtonColor: '#d73925',
      confirmButtonText: 'Close',
      closeOnConfirm: true,
      allowOutsideClick: true
    })
  }

  swalSaveErrorAlert () {
    swal({
      title: 'Error',
      text: 'There was a problem saving the custom fields. Your changes have not been saved.',
      type: 'error',
      confirmButtonColor: '#d73925',
      confirmButtonText: 'Close',
      closeOnConfirm: true,
      allowOutsideClick: true
    })
  }

  addCustomField (e) {
    e.preventDefault()
    this.setState({ customFields: insert(this.state.customFields, { name: 'New Field', type: '', required: true, validation: {} }) })
  }

  updateCustomField (index, field) {
    this.setState({ customFields: update(this.state.customFields, index, field) })
  }

  deleteCustomField (index) {
    this.setState({ customFields: remove(this.state.customFields, index) })
  }

  saveCustomFields () {
    fetch(`/regi/settings/custom-fields`,
      { method: 'POST',
        body: JSON.stringify(this.state.customFields),
        headers: {
          'Accept': 'application/json',
          'Content-Type': 'application/json'
        },
        credentials: 'same-origin'
      })
      .then((data) => {
        if (data.status !== 200) {
          this.setState({ message: 'There was a problem saving.' },
            this.swalSaveErrorAlert.bind(this)
          )
        }
        return data.json()
      })
      .then(this.cleanValidationJson)
      .then((data) => {
        if (typeof data === 'string') {
          try {
            data = JSON.parse(data)
          } catch (e) {}
        }
        if (data && data.Message && data.Message === 'An error has occurred.') {
          this.setState({ message: 'There was a problem saving.' },
            this.swalSaveErrorAlert.bind(this)
          )
        } else {
          this.setState({ message: 'Saved successfully!' })
        }
      })
      .catch((err) => {
        console.error(err)
        this.setState({ message: 'There was a problem saving.' },
          this.swalSaveErrorAlert.bind(this)
        )
      })
  }

  render () {
    const overlay = (<LoadableOverlay loaded={this.state.loaded} />)
    const footer = (
      <div className='btn-group'>
        <button type='button' className='btn btn-success' onClick={this.addCustomField.bind(this)}>Add New Custom Field</button>
        <button type='button' className='btn btn-success' onClick={this.saveCustomFields.bind(this)}>Save Fields</button>
        <span className='help-block' style={{ display: 'inline-block', marginLeft: '1em' }}>
          {this.state.message}
        </span>
      </div>
    )
    return (
      <Box
        type={this.state.message !== 'There was a problem saving.' ? 'success' : 'danger'}
        title='Custom Fields'
        subtitle='Other fields required for client user accounts'
        overlay={overlay}
        footer={footer}
      >
        {this.state.customFields.map((f, i) => (
          <CustomField
            field={f}
            key={i}
            index={i}
            onDelete={this.deleteCustomField.bind(this)}
            onUpdate={this.updateCustomField.bind(this)}
          />
        ))}
      </Box>
    )
  }
}

export default CustomFields
