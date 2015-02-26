var exec = require("child_process").exec;

function send(response, query){
    console.log("Send called with post data: "+query.data);
    response.end();
}

exports.send = send;