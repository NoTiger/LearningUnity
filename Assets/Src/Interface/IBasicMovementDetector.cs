using System;

namespace Learning.Interface
{
    public interface IBasicMovementDetector
    {
        float GetHorizontalMovement();

        float GetVerticalMovement();
    }
}
