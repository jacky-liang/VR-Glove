//constants
const boolean DEBUG = true;

/*BLUETOOTH*/
//Bluetooth Libraries
#include <SoftwareSerial.h>
#include <String.h>

//Bluetooth Pins
#define RxD 0
#define TxD 1

SoftwareSerial BT(RxD, TxD);
String readCache = "";
String message = ""; 

/*GLOVE CONTROL*/
//safety voltage caps
const float max_v = 4.8;
const float max_motor_v = 4.7;

//pin setup
const int num_motors = 6;
const int pins[6] = {3,5,6,9,10,11};
int outputPins[6] = {0,0,0,0,0,0};
float outputLevels[6] = {0,0,0,0,0,0};

//data setup
int durationUnit = 50; //in miliseconds
char tempChar = ' ';
int const messageSize = 17;

void setup()
{
  Serial.begin(9600);
  debug("Began Serial",0);
  
  /*glove control setup*/
  debug("Setting output pins...",0);
  //Set all motor pins to output
  for(int i = 0;i<num_motors;i++)
    pinMode(i,OUTPUT);
  resetOutputs();
  
  /*bluetooth setup*/
  debug("Setting Bluetooth...",0);
  pinMode(1, INPUT_PULLUP);
  /* Init Bluetooth */
  BT.begin(9600);
  setupBT();
}


//for testing on 1: B110100000500000E
void loop()
{
  if (BT.available()) {
      //Reading input bt stream
      tempChar = (char)BT.read();
      if(tempChar == 'B'){
        readCache += tempChar;
        while(BT.available()) {
          tempChar = (char)BT.read();
          readCache += tempChar;
          if (tempChar == 'E')
            break;
        }
          
        message = readCache;
        readCache = ""; //reset read cache
        
        debug("Received Message: ",0);
        debug(message,1); 
        
        if(message.length() == messageSize) {
          //calculating appropriate values
          int dType = toInt(message[1]);
          int dData = toInt(message[2])*durationUnit;
          int iType = toInt(message[3]);
          
          debug("duration type is: ",1);
          Serial.println(dType);
          debug("duration data is: ",1);
          Serial.println(dData);
          debug("interval type is: ",1);
          Serial.println(iType);
          
          //activating corresponding pins
          for (int i = 4;i<10;i++)
            outputPins[i-4] = toInt(message[i]);
            
          //Setting corresponding levels
          for (int i = 10;i<16;i++)
            outputLevels[i-10] = ((float)toInt(message[i]))/10.0;
            
          boolean isRanged = iType == 1;
          boolean isDefinite = dType == 1;
          
          //Running Motors
          //indefinite
          if(!isDefinite)
             toggle(dData, isRanged); 
          //definite
          if(isDefinite)
             cycle(dData);  
          
          resetOutputs();
        }
      }      
  }
  
  //for sending serial commands. 
  if (Serial.available())
    BT.write(Serial.read());
  delay(100);
}

/*BLUETOOTH HELPER FUNCTIONS*/
void setupBT(){
    debug("Sending AT Commands to change BT name and password", 1);
    delay(100);
    BT.write("AT+NAMEVR-Glove");
    delay(100);
    BT.write("AT+PIN4321");
}

/*GLOVE CONTROL HELPER FUNCTIONS*/

//Returns motor pin based on user defined motor id
int getMotorPin(int id){
  return pins[id];
}

//Calculates cycle percent to be sent to control
int getCyclePercent(float V){
  return 255/max_v*V;
}

//Get the desired motor voltage based on input level from 0 to 1
float getMotorV(float level){
  if(level>1)
    return max_motor_v;
  if(level<0)
    return 0;
  return level*max_motor_v;
}

void resetOutputPin(int i){
  outputPins[i] = 0;
}

void resetOutputLevel(int i){
  outputLevels[i] = 0;
}

void resetOutputs(){
  for(int i = 0;i<num_motors;i++){
    resetOutputPin(i);
    resetOutputLevel(i);
  }
}

//Vibrates motors
void cycle(int period){
   
  debug("Cycling with period: "+period,1);
  
  //Output Control
  for(int i = 0;i<num_motors;i++)
    if(outputPins[i] == 1)
      analogWrite(getMotorPin(i), getCyclePercent(getMotorV(outputLevels[i])));
    
  delay(period);

  for(int i = 0;i<num_motors;i++)
     analogWrite(getMotorPin(i), 0);

}

void toggle(int dData,boolean isRanged){  
  for(int i = 0;i<num_motors;i++)
    if(outputPins[i] == 1)
      analogWrite(getMotorPin(i), getCyclePercent(getMotorV(outputLevels[i]))*(float)dData);
}

int toInt(char k){
  return k-48;
}
  
void debug(String x, int indent) {
  if (DEBUG) {
    String msg = x;
    for (int i = 0; i< indent; i++)
      msg = "  " + msg;
    Serial.println(x);    
  }
}
