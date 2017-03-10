class Session {
  constructor (options) {
    this.options = Object.assign({
      cookieName: '.AspNet.ApplicationCookie'
    }, options)
  }

  getCookie (name) {
    var regex = new RegExp(`(?:(?:^|.*;\s*)${this.options.cookieName}\s*\=\s*([^;]*).*$)|^.*$`)
    return document.cookie.replace(regex, '$1')
  }

  remove () {
    document.cookie = `${this.options.cookieName}=; expires='Thu, 01 Jan 1970 00:00:00 UTC'`
  }
}

export default Session
