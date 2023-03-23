#ifndef _MOTOR_H_
#define _MOTOR_H_
#include <L298NX2.h>
const unsigned int EN_A  = 9,
                   IN1_A = 8,
                   IN2_A = 7,
                  
                   IN1_B = 4,
                   IN2_B = 5,
                   EN_B  = 3;
                   
int                lPower = 0, 
                   rPower = 0;

// Initialize both motors
L298NX2                       motors(EN_A, IN1_A, IN2_A, EN_B, IN1_B, IN2_B);

void controlMotors(int l, int r)
{
  lPower = l; rPower = r;
  if (l == 0 && r == 0)
  {
    motors.stop();
    return;
  }

  (l > 0) ?  motors.forwardA() : motors.backwardA();
  motors.setSpeedA(abs(l));
    
  (r > 0) ? motors.forwardB() : motors.backwardB();
  motors.setSpeedB(abs(r));  
}

void stop()
{
  controlMotors(0, 0);
}

#endif