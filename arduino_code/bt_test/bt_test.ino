//import libraries
//Bluetooth
#include <SoftwareSerial.h>
#include <String.h>

//Bluetooth Pins
#define RxD 0
#define TxD 1

SoftwareSerial BT(RxD, TxD);
String readCache = "";
String message = "";
int midMsg;
boolean btConncted = false;

void setup(){
    Serial.begin(9600);
    while(!Serial);
    
    Serial.println("Initialized Serial");
    
    /* Init Bluetooth */
    BT.begin(9600);
    //setupBT();
}

void loop(){
   if (BT.available())
    Serial.write(BT.read());
   /* if (BT.available()) {
        Serial.println("incoming message");
        while(BT.available()) { //keeps reading the bluetooth
            readCache += (char)BT.read();
        }
        message = readCache;
        readCache = ""; //reset read cache
        Serial.println("Message received " + message);
    }
    delay(100);  */   
}

/* Bluetooth */
//ensures the bluetooth module starts with a name and pass
void setupBT(){
    Serial.println("Performing BT Setup");
    delay(1000);
    BT.write("AT+NAMEVR-Glove");
    delay(1000);
    BT.write("AT+PIN4321");
}
