var server = require("./server");
var router = require("./router");
var requestHandlers = require("./requestHandlers");
var bt = require("./bt");

var handle={};
handle["/send"] = function send(response, query){
    console.log("Send called with data: "+query.data);
    bt.sendMsg(query.data);
    response.end();
};
    
bt.btSerial.inquire();
server.start(router.route,handle);