#include <stdlib.h>

int temperaturePin = 0;
int lightPin = 1;
int pingPin = 11;
int gatewaySizeInFeet = 6;
int countToSend=0;

unsigned int duration,inches;

void setup()
{
  Serial.begin(9600);
}

void loop()
{
  float temperature = getTemp();
  int lightLevel = getLight();
  int tempInt = (int)temperature;
  ProcessCounter();
  //char tempF[4];
  //dtostrf(temperature,4,2,tempF);
  String data = "";
  data.concat(tempInt);
  data +=",";
  data.concat(lightLevel);
  data +=",";
  data.concat(countToSend);
  Serial.println(data);
  delay(500);
}

float getTemp()
{
  float voltage = (analogRead(temperaturePin) * .004882814);
  float temperature = (((voltage - .5) *100) * 1.8) + 32;
  return temperature;
}

int getLight()
{
  int lightLevel = analogRead(lightPin);
  lightLevel = map(lightLevel,0,900,0,255);
  lightLevel = constrain(lightLevel,0,255);
  return lightLevel;
}

boolean isBlocked= false;
void ProcessCounter()
{
  int distanceInInches = getInches();
  if(distanceInInches <(gatewaySizeInFeet * 12)){
    isBlocked = true;
  }else if(isBlocked){
    isBlocked = false;
    countToSend = 1;
    return;
  }
  countToSend = 0;
}

unsigned int getInches()
{
  pinMode(pingPin, OUTPUT);          // Set pin to OUTPUT
  digitalWrite(pingPin, LOW);        // Ensure pin is low
  delayMicroseconds(2);
  digitalWrite(pingPin, HIGH);       // Start ranging
  delayMicroseconds(5);              //   with 5 microsecond burst
  digitalWrite(pingPin, LOW);        // End ranging
  pinMode(pingPin, INPUT);           // Set pin to INPUT
  duration = pulseIn(pingPin, HIGH); // Read echo pulse
  inches = duration / 74 / 2;        // Convert to inches
  //Serial.println(inches);  // Display result
  return inches;
  //delay(200);		    
}
