#include "wifi.h"
#include "simple_SH110G_128X64_display.h"
char ssid[] = "ROBOT";        // your network SSID (name)
char pass[] = "PASSWORD";    // your network password (use for WPA, or use as key for WEP)

void wifi_exec(String currentLine)
{
  // Check to see if the client request was "GET /H" or "GET /L":
  if (currentLine.endsWith("GET /H")) {
    digitalWrite(led, HIGH);               // GET /H turns the LED on
  }
  if (currentLine.endsWith("GET /L")) {
    digitalWrite(led, LOW);                // GET /L turns the LED off
  }  
}

void setup() 
{
  // put your setup code here, to run once:
  Serial.begin(9600);
  while (!Serial) {}
  Serial.println("Access Point Web Server 14");

  pinMode(led, OUTPUT);      // set the LED pin mode
  init_wifi(ssid, pass);
  init_SH1106G_128X64_display();
  println_on_display("Connect to WIFI:" + String(WiFi.SSID()));
  println_on_display("http://"+IpAddressToString(WiFi.localIP()));
}

void loop() 
{
  wifi_loop(wifi_exec);
}


