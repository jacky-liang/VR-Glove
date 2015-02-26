var OPO = "C0:EE:FB:27:CB:08";
var BTM = "30:14:12:17:26:73";

var btSerial = new (require('bluetooth-serial-port')).BluetoothSerialPort();
var connected = false;

function isTarget(address){
    var targetAddress = BTM;
    return targetAddress == address;
}
 
function sendMsg(msg){
    console.log("writing data: "+msg);
    if(connected){
        btSerial.write(new Buffer(msg, 'ascii'), function(err, bytesWritten) {
        if (err){
            console.log(err);
        }
    });
    }
    else{
        console.log("Can't write b/c device not connected");
    }
}
 
btSerial.on('found', function(address, name) {
    console.log("discovered " + name+" " + address);
    
    if(isTarget(address)){
        console.log("found target");
        
        btSerial.findSerialPortChannel(address, function(channel) {
            btSerial.connect(address, channel, function() {
                console.log('BT Connected with '+name);
                    
                connected = true;
                /*
                btSerial.on('data', function(buffer) {
                    console.log(buffer.toString('utf-8'));
                });*/

            }, function () {
                console.log('Can\'t connect to '+name);
            });
     
            // close the connection when you're ready 
            btSerial.close();
        }, function() {
            console.log('Can\'t find serial port channel of '+address);
        });
    }
});
 
//btSerial.inquire();
exports.btSerial = btSerial;
exports.sendMsg = sendMsg;