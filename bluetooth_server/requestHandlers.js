var exec = require("child_process").exec;

function send(response, postData){
    console.log("Send called with post data: "+postData);
    response.end();
}

exports.send = send;