var createError = require('http-errors');
var express = require('express');
var session = require('express-session');
var csrfSyncProtection = require('./utils/csrfConfiguration');
var path = require('path');
var cookieParser = require('cookie-parser');
var logger = require('morgan');

var indexRouter = require('./routes/index');
var uploadRouter = require('./routes/upload');

const cookieSecret = process.env['COOKIE_SECRET'];

var app = express();

// Setup app-wide constants
app.set('dalUri', 'http://localhost:5262');
app.set('uploadEndpoint', 'upload');
app.set('reportsEndpoint', 'reports');
app.set('healthcheckEndpoint', 'healthcheck');
app.set('cookieSecret', cookieSecret);

// view engine setup
app.set('views', path.join(__dirname, 'views'));
app.set('view engine', 'pug');

app.use(logger('dev'));
app.use(session);
app.use(express.json());
app.use(express.urlencoded({ extended: false }));
app.use(cookieParser(cookieSecret));
app.use(express.static(path.join(__dirname, 'public')));

app.use('/', indexRouter);

// Enable CSRF protection for the upload endpoints.
app.use('/upload', csrfSyncProtection, uploadRouter);

// catch 404 and forward to error handler
app.use(function (req, res, next) {
  next(createError(404));
});

// error handler
app.use(function (err, req, res) {
  // set locals, only providing error in development
  res.locals.message = err.message;
  res.locals.error = req.app.get('env') === 'development' ? err : {};

  // render the error page
  res.status(err.status || 500);
  res.render('error');
});

module.exports = app;
