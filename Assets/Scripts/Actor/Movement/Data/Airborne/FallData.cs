using System;

namespace Actor.Movement.Data.Airborne
{
    [Serializable]
    public class FallData
    {
        public float Gravity;
        public float InitialGravityKick;
        public float DecelerationForce;
        public float MaxFallSpeed;
    }
}