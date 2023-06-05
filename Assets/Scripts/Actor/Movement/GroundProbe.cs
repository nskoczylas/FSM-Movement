using System;
using UnityEngine;

namespace Actor.Movement
{
    public class GroundProbe : MonoBehaviour
    {
        #region Properties

        // IsGrounded:
        public bool IsGrounded => _isGrounded;
        private bool _isGrounded;

        // Distance to ground:
        public float DistanceToGround => _distanceToGround;
        private float _distanceToGround;
        
        // Ground rotation:
        public Quaternion GroundRotationFromSphere => _groundRotationFromSphere;
        private Quaternion _groundRotationFromSphere;

        public Quaternion GroundRotationFromRay => _groundRotationFromRay;
        private Quaternion _groundRotationFromRay;
        
        // Ground angle:
        public float GroundAngleFromSphere => _groundAngleFromSphere;
        private float _groundAngleFromSphere;

        public float GroundAngleFromRay => _groundAngleFromRay;
        private float _groundAngleFromRay;
        
        // Ground normal:
        private Vector3 _groundNormalFromSphere;
        private Vector3 _groundNormalFromRay;

        #endregion

        #region Settings

        [SerializeField] private Vector3 _originOffset;
        [SerializeField] private LayerMask _layerMask;

        [SerializeField] private float _sphereRadius;
        [SerializeField] private float _sphereMaxDistance;
        [SerializeField] private float _rayMaxDistance;

        #endregion

        private Ray _probeRay;
        private RaycastHit _sphereHit;
        private RaycastHit _rayHit;

        private void Update()
        {
            _probeRay = new Ray(transform.position + _originOffset, Vector3.down);
            
            ProbeUsingSphere();
            ProbeUsingRay();
        }

        private void ProbeUsingSphere()
        {
            //if (Physics.SphereCast(_probeRay))
        }

        private void ProbeUsingRay()
        {
            if (Physics.Raycast(_probeRay, out _rayHit, _rayMaxDistance, _layerMask))
            {
                _groundRotationFromRay = Quaternion.FromToRotation(Vector3.up, _rayHit.normal);
                _groundAngleFromRay = Vector3.Angle(_rayHit.normal, Vector3.up);
                _distanceToGround = Vector3.Distance(transform.position + _originOffset, _rayHit.point);
                _groundNormalFromRay = _rayHit.normal;
            }
            else
            {
                _groundRotationFromRay = Quaternion.identity;
                _groundAngleFromRay = 0f;
                _distanceToGround = 0f;
            }
        }
    }
}