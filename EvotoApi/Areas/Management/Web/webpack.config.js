var path = require('path');
var webpack = require('webpack');

module.exports = {
  entry: './index.jsx',
  output: { path: '../../../Scripts', filename: 'react.js' },
  module: {
    loaders: [
      {
        test: /.jsx?$/,
        loader: 'babel-loader',
        exclude: /node_modules/,
        query: {
          presets: ['es2015', 'react']
        }
      }
    ]
  }
};
