var http = require("http");
var url = require("url");

function start(route, handle){

    function onRequest(request, response){
        var postData = "";      
        var pathname = url.parse(request.url).pathname;
        
        console.log("Request for "+pathname+" received.");
        
        request.setEncoding("utf8");
        
        var url_parts = url.parse(request.url, true);
        var query = url_parts.query;
        
        route(handle,pathname,response,query);  
    }
    
    http.createServer(onRequest).listen(80);
    console.log("Web Server Started");
}

exports.start = start;