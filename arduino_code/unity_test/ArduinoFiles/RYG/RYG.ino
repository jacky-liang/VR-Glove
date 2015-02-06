
int gLed = 10;
int yLed = 11;
int rLed = 12;

void setup() {                
   pinMode(gLed, OUTPUT);    
   pinMode(yLed, OUTPUT);   
   pinMode(rLed, OUTPUT);   
  
   digitalWrite(gLed, LOW);
   digitalWrite(yLed, LOW);
   digitalWrite(rLed, LOW); 
}


void loop() {
  
  digitalWrite(gLed, HIGH);   
  delay(1000);              
  digitalWrite(gLed, LOW);    
  digitalWrite(yLed, HIGH);   
  delay(1000);       
  digitalWrite(yLed, LOW);   
  digitalWrite(rLed, HIGH);  
  delay(1000);  
  digitalWrite(rLed, LOW);   
}
