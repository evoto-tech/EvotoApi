import React from 'react'

class VoteList extends React.Component {
  propTypes: {
    title: React.PropTypes.string.isRequired,
    name: React.PropTypes.string.isRequired,
    confirmText: React.PropTypes.string,
    cancelText: React.PropTypes.string,
    confirm: React.PropTypes.func,
    cancel: React.PropTypes.func,
    isOpen: React.PropTypes.bool,
    content: React.PropTypes.string
  }

  componentDidMount () {
    this.loadModal()
  }

  componentDidUpdate () {
    this.loadModal()
  }

  loadModal () {
    const show = this.props.isOpen || false
    const name = this.props.name
    $(function () {
      $(`#${name}`).modal({ show: show })
    })
  }

  show () {
    const name = this.props.name
    $(function () {
      $(`#${name}`).modal('show')
    })
  }

  hide () {
    const name = this.props.name
    $(function () {
      $(`#${name}`).modal('hide')
    })
  }

  handleConfirm () {
    this.props.confirm()
    this.hide()
  }

  handleCancel () {
    if (this.props.cancel) {
      this.props.cancel()
    }
    this.hide()
  }

  render () {
    const confirmButton = this.props.confirm ? (
      <button type='button' className='btn btn-primary' onClick={this.handleConfirm.bind(this)}>{this.props.cancelText || 'Confirm'}</button>
    ) : ''
    const modalContent = this.props.content ? (
      <div className='modal-content'>
        {this.props.content}
      </div>
    ) : ''
    return (
      <div className='modal fade' id={this.props.name} tabIndex='-1' role='dialog'>
        <div className='modal-dialog modal-sm' role='document'>
          <div className='modal-content'>
            <div className='modal-header'>
              <button type='button' className='close' data-dismiss='modal' aria-label='Close'><span aria-hidden='true'>&times;</span></button>
              <h4 className='modal-title'>{this.props.title}</h4>
            </div>
            {modalContent}
            <div className='modal-footer'>
              <button type='button' className='btn btn-default' data-dismiss='modal' onClick={this.handleCancel.bind(this)}>{this.props.cancelText || 'Cancel'}</button>
              {confirmButton}
            </div>
          </div>
        </div>
      </div>
    )
  }
}

export default VoteList
