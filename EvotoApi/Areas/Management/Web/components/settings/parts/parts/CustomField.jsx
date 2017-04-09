import React from 'react'
import Box from '../../../parts/Box.jsx'
import FormGroup from '../../../parts/form-components/FormGroup.jsx'
import NamedInput from '../../../parts/form-components/NamedInput.jsx'

class CustomField extends React.Component {
  constructor (props) {
    super(props)
    this.state = Object.assign({}, this.stateFromProps(props))
  }

  propTypes: {
    field: React.PropTypes.object.isRequired,
    index: React.ProtoTypes.number,
    onDelete: React.ProtoTypes.func,
    onUpdate: React.ProtoTypes.func
  }

  stateFromProps (props) {
    return props.field || {}
  }

  componentDidMount () {
    $(this.requiredCheckbox).iCheck({
      checkboxClass: 'icheckbox_flat-green',
      radioClass: 'icheckbox_flat-green'
    })
    $(this.requiredCheckbox).on('ifChanged', (e) => {
      this.updateField('required', e, e.target.checked)
    })
  }

  componentWillReceiveProps (nextProps) {
    this.setState(this.stateFromProps(nextProps))
  }

  componentWillUpdate (nextProps, nextState) {
    if (this.state.type !== nextState.type) {
      this.setState({ validation: this.getTypeValidationState(nextState.type) })
    }
  }

  delete (e) {
    e.preventDefault()
    this.props.onDelete(this.props.index)
  }

  onUpdate () {
    this.props.onUpdate(this.props.index, this.state)
  }

  updateField (type, e, value) {
    e.preventDefault()
    value = value === undefined ? e.target.value : value
    this.setState({ [type]: value }, this.onUpdate)
  }

  updateValidationField (field, e) {
    e.preventDefault()
    let value = e.target.value
    if (e.target.type === 'number') {
      value = parseInt(value)
      if (value === 'NaN') value = 0
    }
    const validation = Object.assign({}, this.state.validation, { [field]: value })
    this.setState({ validation }, this.onUpdate)
  }

  getTypeValidationState (type) {
    if (type === 'String') {
      return {
        minLength: '',
        maxLength: ''
      }
    } else if (type === 'Number') {
      return {
        min: '',
        max: ''
      }
    } else if (type === 'Date') {
      return {
        minLength: '',
        maxLength: '',
        regex: ''
      }
    } else {
      return {}
    }
  }

  getTypeValidationFields (type) {
    if (type === 'String') {
      return (
        <FormGroup>
          <NamedInput name='Minimum Length' type='number' value={this.state.validation.minLength} onChange={this.updateValidationField.bind(this, 'minLength')} />
          <NamedInput name='Maximum Length' type='number' value={this.state.validation.maxLength} onChange={this.updateValidationField.bind(this, 'maxLength')} />
          <NamedInput name='Rule' type='text' value={this.state.validation.regex} onChange={this.updateValidationField.bind(this, 'regex')} />
        </FormGroup>
      )
    } else if (type === 'Number') {
      return (
        <FormGroup>
          <NamedInput name='Minimum' type='number' value={this.state.validation.min} onChange={this.updateValidationField.bind(this, 'min')} />
          <NamedInput name='Maximum' type='number' value={this.state.validation.max} onChange={this.updateValidationField.bind(this, 'max')} />
        </FormGroup>
      )
    } else if (type === 'Date') {
      return (
        <FormGroup>
          <NamedInput name='Minimum Date' type='text' value={this.state.validation.minDate} onChange={this.updateValidationField.bind(this, 'minDate')} />
          <NamedInput name='Maximum Date' type='text' value={this.state.validation.maxDate} onChange={this.updateValidationField.bind(this, 'maxDate')} />
        </FormGroup>
      )
    }
  }

  render () {
    const title = (
      <NamedInput
        name={<b>Field {this.props.index + 1}</b>}
        placeholder='Field Name...'
        type='text'
        value={this.state.name}
        onChange={this.updateField.bind(this, 'name')}
      />
    )
    return (
      <Box
        type='success'
        title={title}
      >
        <div className='form-group'>
          <select className='form-control' value={this.state.type} onChange={this.updateField.bind(this, 'type')} disabled={this.state.hasOwnProperty('id')}>
            <option value='' disabled>Field Type</option>
            <option>String</option>
            <option>Number</option>
            <option>Email</option>
            <option>Date</option>
          </select>
          {!this.state.hasOwnProperty('id') ? '' : (
            <p className='help-block'>The type of an existing field cannot be changed.</p>
          ) }
        </div>
        {!['Number', 'String', 'Date'].includes(this.state.type) ? '' : this.getTypeValidationFields(this.state.type)}
        <div className='form-group'>
          <NamedInput
            name='Required Field?'
            type='checkbox'
            className='icheckbox_flat-green'
            inputRef={(input) => { this.requiredCheckbox = input }}
            defaultChecked={this.state.required}
          />
        </div>
        <button type='button' className='btn btn-danger' onClick={this.delete.bind(this)}>Delete Field</button>
      </Box>
    )
  }
}

export default CustomField
