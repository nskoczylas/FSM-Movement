using Actor.Movement.Data.Grounded;
using UnityEngine;

namespace Actor.Movement.Data
{
    [CreateAssetMenu(fileName = "NewMovementData", menuName = "Actor/MovementData")]
    public class MovementData : ScriptableObject
    {
        [field: SerializeField] public IdleData Idle { get; private set; }
    }
}