import React from 'react'
import PropTypes from 'prop-types'

class Wrapper extends React.Component {
  render () {
    let title = this.props.title || ''
    let description = this.props.description || ''
    return (
      <div className='content-wrapper' style={{ height: '100%' }}>
        <section className='content-header' style={{ height: '100%' }}>
          <h1>{title}<small>{description}</small></h1>
          <ol className='breadcrumb'>
            {this.props.breadcrumbs ? this.props.breadcrumbs.map((crumb, i) => <li key={i}>{crumb}</li>) : ''}
          </ol>
        </section>
        <section className='content'>
          {this.props.children}
        </section>
      </div>
    )
  }
}

Wrapper.propTypes = {
  title: PropTypes.string.isRequired,
  description: PropTypes.string.isRequired,
  breadcrumbs: PropTypes.node
}

export default Wrapper
