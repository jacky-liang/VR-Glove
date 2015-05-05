//Bluetooth Libraries
#include <SoftwareSerial.h>
#include <String.h>

//Bluetooth Pins
#define RxD 0
#define TxD 1

SoftwareSerial BT(RxD, TxD);
String readCache = "";
String message = ""; 

void setup(){
    Serial.begin(9600);
    while(!Serial);
    
    pinMode(TxD, INPUT_PULLUP);
    
    Serial.println("Initialized Serial");
    /* Init Bluetooth */
    BT.begin(9600);
    setupBT();
}

void loop(){
  if (BT.available()) {
        Serial.println("incoming message");

        while(BT.available())
            readCache += (char)BT.read();

        message = readCache;
        readCache = ""; //reset read cachej
        Serial.println("Message received " + message);
  }
  
  if (Serial.available())
    BT.write(Serial.read());
  delay(100);   
}

/* Bluetooth */
void setupBT(){
    Serial.println("Performing BT Setup");
    delay(100);
    BT.write("AT+NAMEVR-Glove");
    delay(100);
    BT.write("AT+PIN4321");
    Serial.println("Finished BT Setup");
}
