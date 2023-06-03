using System;
using UnityEngine;

namespace Actor.Movement.Data.Grounded
{
    [Serializable]
    public class WalkData
    {
        [SerializeField] public float Speed;
        [SerializeField] public float Acceleration;
        [SerializeField] public float Deceleration;
    }
}