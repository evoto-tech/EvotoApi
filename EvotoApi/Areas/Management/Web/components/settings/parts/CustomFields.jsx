import React from 'react'
import Box from '../../parts/Box.jsx'
import LoadableOverlay from '../../parts/LoadableOverlay.jsx'
import CustomField from './parts/CustomField.jsx'

class CustomFields extends React.Component {
  constructor (props) {
    super(props)
    this.state = { loaded: true, customFields: [ { name: 'what', type: '', required: true, validation: {} } ] }
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

  saveCustomFields () {
    console.log(this.state.customFields)
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
