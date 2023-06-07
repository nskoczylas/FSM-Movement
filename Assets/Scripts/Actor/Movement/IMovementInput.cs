using System;
using UnityEngine;

namespace Actor.Movement
{
    public interface IMovementInput
    {
        public Vector2 View { get; }
        public Vector2 Move { get; }
        public Action<ActionStage> Jump { get; set; }
        public Action<ActionStage> Sprint { get; set; }
        public Action<ActionStage> Crouch { get; set; }
    }

    public enum ActionStage
    {
        Pressed,
        Released
    }
}