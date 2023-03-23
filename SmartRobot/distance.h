#ifndef _DISTANCE_H_
#define _DISTANCE_H_
#include <VL53L0X.h>
VL53L0X                        distance;
float                          currentDistanceInCentimeters = 0;

float getDistanceInCentimeters()
{
  return distance.readRangeContinuousMillimeters() / 10;
}
#endif