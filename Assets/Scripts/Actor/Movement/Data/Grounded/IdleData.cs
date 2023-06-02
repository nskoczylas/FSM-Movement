using System;
using UnityEngine;

namespace Actor.Movement.Data.Grounded
{
    [Serializable]
    public class IdleData
    {
        [SerializeField] public Vector2 CameraClamp;
        [SerializeField] public Vector2 CameraSensitivity;
    }
}