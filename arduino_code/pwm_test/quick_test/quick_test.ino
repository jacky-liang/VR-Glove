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
  digitalWrite(pins[0],HIGH);
     
  delay(1000);
}

