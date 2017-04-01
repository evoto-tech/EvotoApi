import React from 'react'
import Box from '../../../parts/Box.jsx'

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
    return {
      field: props.field || ''
    }
  }

  componentDidMount () {
    $(this.requiredCheckbox).on('ifChanged', (e) => {
      this.updateField('required', e, e.target.checked)
    })
  }

  componentWillReceiveProps (nextProps) {
    this.setState(this.stateFromProps(nextProps))
  }

  delete (e) {
    e.preventDefault()
    this.props.onDelete(this.props.index)
  }

  onUpdate () {
    this.props.onUpdate(this.props.index, this.state.field)
  }

  updateField (type, e, value) {
    e.preventDefault()
    value = value === undefined ? e.target.value : value
    const field = Object.assign({}, this.state.field, { [type]: value })
    this.setState({ field }, this.onUpdate)
  }

  getTypeValidationFields () {
    if (this.state.field.type === 'String') {
      return (
        <div className='form-group'>
          <div className='input-group' style={{ padding: '2px', width: '100%' }}>
            <span className='input-group-addon' style={{ color: '#000000', width: '130px' }}>Minimum Length</span>
            <input
              type='number'
              className='form-control'
              placeholder='Minimum Length...'
            />
          </div>
          <div className='input-group' style={{ padding: '2px', width: '100%' }}>
            <span className='input-group-addon' style={{ color: '#000000', width: '130px' }}>Maximum Length</span>
            <input
              type='number'
              className='form-control'
              placeholder='Maximum Length...'
            />
          </div>
          <div className='input-group' style={{ padding: '2px', width: '100%' }}>
            <span className='input-group-addon' style={{ color: '#000000', width: '130px' }}>Rule</span>
            <input
              type='text'
              className='form-control'
              placeholder='Rule...'
            />
          </div>
        </div>
      )
    } else if (this.state.field.type === 'Number') {
      return (
        <div className='form-group'>
          <div className='input-group' style={{ padding: '2px' }}>
            <span className='input-group-addon' style={{ color: '#000000' }}>Minimum</span>
            <input
              type='number'
              className='form-control'
              placeholder='Minimum...'
            />
          </div>
          <div className='input-group' style={{ padding: '2px' }}>
            <span className='input-group-addon' style={{ color: '#000000' }}>Maximum</span>
            <input
              type='number'
              className='form-control'
              placeholder='Maximum...'
            />
          </div>
        </div>
      )
    } else if (this.state.field.type === 'Date') {
      return (
        <div className='form-group'>
          <div className="input-group date" style={{ padding: '2px' }}>
            <div className="input-group-addon">
              Minimum Date
            </div>
            <input type="text" className="form-control pull-right" id="mindatepicker" />
          </div>
          <div className="input-group date" style={{ padding: '2px' }}>
            <div className="input-group-addon">
              Maximum Date
            </div>
            <input type="text" className="form-control pull-right" id="maxdatepicker" />
          </div>
        </div>
      )
    }
  }

  render () {
    const title = (
      <div className='input-group' style={{ padding: '2px' }}>
        <span className='input-group-addon' style={{ color: '#000000' }}><b>Field {this.props.index + 1}</b></span>
        <input
          type='text'
          className='form-control'
          placeholder='Field Name...'
          value={this.state.field.name}
          onChange={this.updateField.bind(this, 'name')}
        />
      </div>
    )
    return (
      <Box
        type='success'
        title={title}
      >
        <div className='form-group'>
          <select className='form-control' value={this.state.field.type} onChange={this.updateField.bind(this, 'type')}>
            <option value='' disabled>Field Type</option>
            <option>String</option>
            <option>Number</option>
            <option>Email</option>
            <option>Date</option>
          </select>
        </div>
        {!['Number', 'String', 'Date'].includes(this.state.field.type) ? '' : this.getTypeValidationFields()}
        <div className='form-group'>
          <label>
            <input
              type='checkbox'
              name='iCheck'
              className='icheckbox_flat-green'
              ref={(input) => this.requiredCheckbox = input}
              defaultChecked={this.state.field.required}
            />
            <span style={{ position: 'relative', left: '5px', top: '0.14em' }}>Required field</span>
          </label>
        </div>
        <button type='button' className='btn btn-danger' onClick={this.delete.bind(this)}>Delete Field</button>
      </Box>
    )
  }
}

export default CustomField
