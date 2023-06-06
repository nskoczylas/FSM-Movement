using System;
using UnityEngine;

namespace Actor.Movement.Data.Grounded
{
    [Serializable]
    public class WalkData
    {
        public float Speed;
        public float Acceleration;
        public float Deceleration;
        public float DownForce;
    }
}