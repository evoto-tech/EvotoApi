import React from 'react'
import Box from '../../parts/Box.jsx'
import LoadableOverlay from '../../parts/LoadableOverlay.jsx'
import CustomField from './parts/CustomField.jsx'

class CustomFields extends React.Component {
  constructor (props) {
    super(props)
    this.state = { loaded: false, customFields: [] }
  }

  componentDidMount () {
    fetch(`/regi/settings/custom-fields`, { credentials: 'same-origin' })
      .then((res) => res.json())
      .then(this.cleanValidationJson)
      .then((data) => {
        this.setState({ customFields: data, loaded: true })
      })
      .catch(console.error)
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
    this.setState({ customFields: [].concat(this.state.customFields).concat([ { name: 'New Field', type: '', required: true, validation: {} } ]) })
  }

  updateCustomField (index, field) {
    const customFields = [].concat(this.state.customFields)
    customFields[index] = field
    this.setState({ customFields })
  }

  deleteCustomField (index) {
    const customFields = [].concat(this.state.customFields)
    customFields.splice(index, 1)
    this.setState({ customFields })
  }

  cleanValidationJson (json) {
    if (json && Array.isArray(json)) {
      return json.map((item) => {
        if (item.validation) {
          const newItemValidation = Object.assign({}, item.validation)
          Object.keys(newItemValidation).forEach((field) => {
            const fieldValue = newItemValidation[field]
            newItemValidation[field] = fieldValue === null || fieldValue === 'null' || fieldValue === '' ? '' : fieldValue
          })
          item.validation = newItemValidation
        }
        return item
      })
    }
    return json
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
      .then((res) => res.json())
      .then(this.cleanValidationJson)
      .then((data) => {
        if (typeof(data) === 'string') {
          try {
            data = JSON.parse(data)
          } catch (e) {
            data = data
          }
        }
        if (data.Message && data.Message === 'An error has occurred.') {
          this.swalSaveErrorAlert()
        }
      })
      .catch(console.error)
  }

  render () {
    const footer = (
      <div className='btn-group'>
        <button type='button' className='btn btn-success' onClick={this.addCustomField.bind(this)}>Add New Custom Field</button>
        <button type='button' className='btn btn-success' onClick={this.saveCustomFields.bind(this)}>Save Fields</button>
      </div>
    )
    return (
      <Box
        type='success'
        title='Custom Fields'
        subtitle='Other fields required for client user accounts'
        footer={footer}
      >
        <LoadableOverlay loaded={this.state.loaded} />
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
