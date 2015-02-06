const int pins[6] = {3};

void setup()
{
  //Set all motor pins to output
  for(int i = 0;i<sizeof(pins);i++)
    pinMode(i,OUTPUT);
  
  Serial.begin(9600);
}

void loop()
{
  float dutyCycle = 150;
    
  for(int i = 0;i<sizeof(pins);i++)
     analogWrite(pins[i], dutyCycle);
    
  delay(1000);
  
  for(int i = 0;i<sizeof(pins  );i++)
     analogWrite(pins[i], 0);
     
  delay(1000);
}

