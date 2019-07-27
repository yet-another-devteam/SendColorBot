'use strict';
const express = require('express');
const ecstatic = require('ecstatic');
const http = require('http'); 
const app = express();
app.use(ecstatic({
  root: `${__dirname}/images`,
  showdir: true,
})); 
http.createServer(app).listen();
console.log('Listening on :8080');