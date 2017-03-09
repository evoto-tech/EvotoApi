class Session {
  constructor (options) {
    this.options = Object.assign({
      refreshUrl: '/Token',
      prefix: 'evoto',
      accessTokenName: 'AccessToken',
      refreshTokenName: 'RefreshToken'
    }, options)
    this.options.accessTokenName =
      this.options.prefix + this.options.accessTokenName
    this.options.refreshTokenName =
      this.options.prefix + this.options.refreshTokenName
  }

  storeTokens (access, refresh, expiry, remove) {
    this.storeToken(`${this.options.accessTokenName}`, access, expiry, remove)
    this.storeToken(`${this.options.refreshTokenName}`, refresh, 'Fri, 31 Dec 9999 23:59:59 GMT', remove)
  }

  storeToken (name, token, expiry, remove) {
    expiry = remove ? 'Thu, 01 Jan 1970 00:00:00 UTC' : expiry
    document.cookie = `${name}=${token}; expires=${expiry}`
  }

  getAccessTokenCookie () {
    return this.getTokenCookie(this.options.accessTokenName)
  }

  getRefreshTokenCookie () {
    return this.getTokenCookie(this.options.refreshTokenName)
  }

  getTokenCookie (name) {
    var regex = new RegExp(`(?:(?:^|.*;\s*)${name}\s*\=\s*([^;]*).*$)|^.*$`)
    return document.cookie.replace(regex, '$1')
  }

  getStoredToken () {
    return new Promise((resolve, reject) => {
      let accessToken = this.getAccessTokenCookie()
      if (accessToken) return resolve(accessToken)
      this.refresh().then(resolve)
    })
  }

  makeSignedRequest (url, options) {
    this.getStoredaccessToken()
      .then((token) => {
        options = Object.assign({}, options)
        options.headers = options.headers || {}
        options.headers['Authorization'] = `Bearer ${token}`
        return fetch(url, options)
          .then((data) => data, console.error)
      })
  }

  refresh () {
    let refreshToken = this.getRefreshTokenCookie()
      , form = [
        `refresh_token=${encodeURIComponent(refreshToken)}`,
        `grant_type=refresh_token`
      ].join('&')

    return fetch(this.options.refreshUrl,
        { method: 'POST',
          body: form,
          headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/x-www-form-urlencoded; charset=utf-8'
          }
        })
        .then((data) => {
          return data.json()
        }, console.error)
        .then((json) => {
          this.storeAccessToken(json['access_token'])
          return json['access_token']
        }, console.error)
  }

  remove () {
    this.storeTokens('', '', '', true)
  }
}

export default Session
