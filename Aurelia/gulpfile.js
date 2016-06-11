var gulp = require('gulp');
var sync = require('browser-sync');

gulp.task('serve', function(done) {
    sync({
        open: false,
        port: 9000,
        server: {
            baseDir: ['.'],
            middleware: function(req, res, next) {
                res.setHeader('Access-Control-Allow-Origin', '*');
                next();
            }
        }
    }, done);
});