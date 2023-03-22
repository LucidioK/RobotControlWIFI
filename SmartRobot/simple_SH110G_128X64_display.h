#ifndef _SIMPLE_SH110G_128X64_DISPLAY_H_
#define _SIMPLE_SH110G_128X64_DISPLAY_H_
#include <SPI.h>
#include <Wire.h>
#include <Adafruit_GFX.h>
#include <Adafruit_SH110X.h>

/* Uncomment the initialize the I2C address , uncomment only one, If you get a totally blank screen try the other*/
#define i2c_Address 0x3c //initialize with the I2C addr 0x3C Typically eBay OLED's
//#define i2c_Address 0x3d //initialize with the I2C addr 0x3D Typically Adafruit OLED's

#define SCREEN_WIDTH 128 // OLED display width, in pixels
#define SCREEN_HEIGHT 64 // OLED display height, in pixels
#define OLED_RESET -1   //   QT-PY / XIAO
Adafruit_SH1106G display = Adafruit_SH1106G(SCREEN_WIDTH, SCREEN_HEIGHT, &Wire, OLED_RESET);

void init_SH1106G_128X64_display()
{
  delay(250); // wait for the OLED to power up
  display.begin(i2c_Address, true); // Address 0x3C default
  display.display();
  display.clearDisplay();
  display.setTextSize(1);
  display.setTextColor(SH110X_WHITE);
 
}

void print_on_display(int x, int y, String text)
{
  Serial.println("About to print on display:");
  Serial.println(text);
  display.setTextSize(1);
  display.setTextColor(SH110X_WHITE);
  display.setCursor(x, y);
  display.println(text);
  display.display();
}

void println_on_display(String text)
{
  Serial.println("About to printLN on display:");
  Serial.println(text);
  display.println(text);
  display.display();
}

#endif