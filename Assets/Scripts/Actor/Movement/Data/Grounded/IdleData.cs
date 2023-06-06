using System;
using UnityEngine;

namespace Actor.Movement.Data.Grounded
{
    [Serializable]
    public class IdleData
    {
        public Vector2 CameraClamp;
        public Vector2 CameraSensitivity; 
        public float DownForce;
    }
}