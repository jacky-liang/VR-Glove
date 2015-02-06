const float max_v = 4.3;
const float max_motor_v = 3.9;
const int pins[2] = {3,5};
int pinIds[2] = {0,1};

void setup()
{
  //Set all motor pins to output
  for(int i = 0;i<sizeof(pins);i++){
    pinMode(i,OUTPUT);
  }
  Serial.begin(9600);
}

//Returns motor pin based on user defined motor id
int getMotorPin(int id){
  return pins[id];
}

//Calculates cycle percent to be sent to control
int getCyclePercent(float V)
{
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

//Vibrates motors
void testCycle(float level, int period, int pinIds []){
  //Find Duty Cycle given level from 0.0 to 1.0
  float dutyCycle = getCyclePercent(getMotorV(level));
  
  //Debug
  Serial.print("\nOutput Duty Cycle is: ");
  Serial.print(dutyCycle);
  Serial.print("\n");
  
  //Output Control
  for(int i = 0;i<sizeof(pinIds);i++)
    analogWrite(getMotorPin(pinIds[i]), dutyCycle);
  
  delay(period);
  
  for(int i = 0;i<sizeof(pinIds);i++)
    analogWrite(getMotorPin(pinIds[i]), 0);
}

void loop()
{
  Serial.print("\nInput Vibration Intensity from 0.0 to 1.0: ");
  while(Serial.available() == 0);
  float level = Serial.parseFloat();
  Serial.print(level);
  
  //for testing
  if(level>9){
    testCycle(0.8,5000,pinIds);
  }
  else{
    testCycle(level,1000,pinIds);    
  }
  delay(20);
}

