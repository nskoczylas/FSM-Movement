using System;
using UnityEngine;

namespace Actor.Movement
{
    public interface IMovementInput
    {
        public Vector2 View { get; }
        public Vector2 Move { get; }
        public Action<ActionStage> Jump { get; }
        public Action<ActionStage> Sprint { get; }
        public Action<ActionStage> Crouch { get; }
    }

    public enum ActionStage
    {
        Pressed,
        Released
    }
}