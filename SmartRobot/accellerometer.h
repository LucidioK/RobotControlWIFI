#ifndef _ACCELLEROMETER_H_
#define _ACCELLEROMETER_H_
#include <Adafruit_Sensor.h>

#include <Adafruit_LSM303_Accel.h>
Adafruit_LSM303_Accel_Unified  accel   = Adafruit_LSM303_Accel_Unified(54321);
bool                           accelOK = false;
bool                           accelInitialized = false;

bool getAccellerometerXYX(float *x, float *y, float *z)
{
  if (!accelInitialized)
  {
    accelOK = accel.begin();
    if (accelOK)
    {
      accel.setRange(LSM303_RANGE_4G);
    }    
  }

  accelInitialized = true;
  if (accelOK)
  {
    sensors_event_t event;
    accel.getEvent(&event);
    *x = event.acceleration.x;
    *y = event.acceleration.y;
    *z = event.acceleration.z;
  }

  return accelOK;
}

#endif