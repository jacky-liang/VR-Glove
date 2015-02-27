//safety voltage caps
const float max_v = 4.8;
const float max_motor_v = 4.7;

//pin setup
const int num_motors = 6;
const int pins[6] = {3,5,6,9,10,11};
int outputPins[6] = {0,0,0,0,0,0};
float outputLevels[6] = {0,0,0,0,0,0};

//data setup
char inData[17];
int durationUnit = 50; //in miliseconds

int getNumMotors(){
  return num_motors;
}

void setup()
{
  //Set all motor pins to output
  for(int i = 0;i<getNumMotors();i++)
    pinMode(i,OUTPUT);
  resetOutputs();
  Serial.begin(9600);
}

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
  for(int i = 0;i<getNumMotors();i++){
    resetOutputPin(i);
    resetOutputLevel(i);
  }
}

//Vibrates motors
void cycle(int period){
   
  Serial.print("Cycling with period: ");
  Serial.println(period);
  
  //Output Control
  for(int i = 0;i<getNumMotors();i++)
    if(outputPins[i] == 1)
      analogWrite(getMotorPin(i), getCyclePercent(getMotorV(outputLevels[i])));
    
  delay(period);

  for(int i = 0;i<getNumMotors();i++)
     analogWrite(getMotorPin(i), 0);

}

void toggle(int dData,boolean isRanged){  
  for(int i = 0;i<getNumMotors();i++)
    if(outputPins[i] == 1)
      analogWrite(getMotorPin(i), getCyclePercent(getMotorV(outputLevels[i]))*(float)dData);
}

int toInt(char k){
  return k-48;
}

//for testing on 1: &110100000500000%

void loop()
{
  Serial.readBytesUntil('E', inData, 17);
  if (inData[0] == 'B'){
    
    int dType = toInt(inData[1]);
    int dData = toInt(inData[2])*durationUnit;
    int iType = toInt(inData[3]);
    
    //activating corresponding pins
    for (int i = 4;i<10;i++)
      outputPins[i-4] = toInt(inData[i]);
      
    //Setting corresponding levels
    for (int i = 10;i<16;i++)
      outputLevels[i-10] = ((float)toInt(inData[i]))/10.0;
      
    boolean isRanged = iType == 1;
    boolean isDefinite = dType == 1;
    //indefinite
    if(!isDefinite)
       toggle(dData, isRanged); 
    //definite
    if(isDefinite)
       cycle(dData);  
    
    resetOutputs();
  }
  
  //Reset Input Stream
  inData[0] = '#';
}
  
