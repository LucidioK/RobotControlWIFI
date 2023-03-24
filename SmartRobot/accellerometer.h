#ifndef _ACCELLEROMETER_H_
#define _ACCELLEROMETER_H_
#include "SparkFunLSM6DS3.h"
#include "Wire.h"
#include "SPI.h"

LSM6DS3 __myIMU__; //Default constructor is I2C, addr 0x6B

class AccellerometerGyroscopeThermometer
{
public:
  void initialize()
  {
    __myIMU__.begin();
  }

  void get_accellerations(float *x, float *y, float *z)
  {
    *x = __myIMU__.readFloatAccelX();
    *y = __myIMU__.readFloatAccelY();
    *z = __myIMU__.readFloatAccelZ();
  }

  void get_gyroscope(float *x, float *y, float *z)
  {
    *x = __myIMU__.readFloatGyroX();
    *y = __myIMU__.readFloatGyroY();
    *z = __myIMU__.readFloatGyroZ();
  }

  float get_temp_C()
  {
    return __myIMU__.readTempC();
  }
};
#endif