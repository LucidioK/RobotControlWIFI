#include "wifi.h"
#include "display.h"
#include "accellerometer.h"
char ssid[] = "ROBOT";        // your network SSID (name)
char pass[] = "PASSWORD";    // your network password (use for WPA, or use as key for WEP)

Display display;
Wifi wifi;
AccellerometerGyroscopeThermometer agt;

void wifi_exec(String currentLine)
{
  // Check to see if the client request was "GET /H" or "GET /L":
  if (currentLine.endsWith("GET /H")) {
    digitalWrite(led, HIGH);               // GET /H turns the LED on
    display.display_at(3, String("ON"));         
  }
  if (currentLine.endsWith("GET /L")) {
    digitalWrite(led, LOW);                // GET /L turns the LED off
    display.display_at(3, String("OFF"));         
  }  
  if (currentLine.endsWith("GET /M")) { // GET /M changes motor speed
    digitalWrite(led, LOW);       
  }
  float x,y,z,temp;
  agt.get_accellerations(&x, &y, &z);
  display.display_at(4, "A: "+ String(x,1)+" "+String(y,1)+" "+String(z,1));
  agt.get_gyroscope(&x, &y, &z);
  display.display_at(5, "G: " + String(x,1)+" "+String(y,1)+" "+String(z,1));
  temp = agt.get_temp_C();
  display.display_at(6, "T: " + String(temp,1));
}

void setup() 
{
  // put your setup code here, to run once:
  Serial.begin(74880);
  while (!Serial) {}
  Serial.println("Access Point Web Server 14");

  pinMode(led, OUTPUT);      // set the LED pin mode
  wifi.initialize(ssid, pass);
  wifi.printWiFiStatus();
  display.initialize();
  agt.initialize();
  display.display_at(0, String("Connect to WIFI:" + wifi.SSID()));
  display.display_at(1, String("http://"+wifi.local_ip()));
  display.display_at(2, String("Bubu"));
}

void loop() 
{
  wifi.wifi_loop(wifi_exec);
}


