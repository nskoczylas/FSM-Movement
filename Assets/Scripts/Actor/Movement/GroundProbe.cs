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
        
        // Time since grounded:
        public float TimeSinceGrounded => _timeSinceGrounded;
        private float _timeSinceGrounded;

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
        public Vector3 GroundNormalFromSphere => _groundNormalFromSphere;
        private Vector3 _groundNormalFromSphere;

        public Vector3 GroundNormalFromRay => _groundNormalFromRay;
        private Vector3 _groundNormalFromRay;

        #endregion

        #region Settings

        [SerializeField] private Vector3 _originOffset;
        [SerializeField] private LayerMask _layerMask;

        [SerializeField] private float _sphereRadius;
        [SerializeField] private float _sphereMaxDistance;

        #endregion

        private Ray _probeRay;
        private RaycastHit _sphereHit;
        private RaycastHit _rayHit;

        private void Update()
        {
            _probeRay = new Ray(transform.position + _originOffset, Vector3.down);
            
            ProbeUsingSphere();
            ProbeUsingRay();
            
            SetTimeSinceGrounded();
        }

        private void SetTimeSinceGrounded()
        {
            if (_isGrounded)
            {
                _timeSinceGrounded = 0f;
                return;
            }

            _timeSinceGrounded += Time.deltaTime;
        }

        private void ProbeUsingSphere()
        {
            if (Physics.SphereCast(_probeRay, _sphereRadius, out _sphereHit, _sphereMaxDistance, _layerMask))
            {
                _isGrounded = true;
                _groundRotationFromSphere = Quaternion.FromToRotation(Vector3.up, _sphereHit.normal);
                _groundAngleFromSphere = Vector3.Angle(_sphereHit.normal, Vector3.up);
                _groundNormalFromSphere = _sphereHit.normal;
            }
            else
            {
                _isGrounded = false;
                _groundRotationFromRay = Quaternion.identity;
                _groundAngleFromSphere = 0f;
                _groundNormalFromSphere = Vector3.up;
            }
        }

        private void ProbeUsingRay()
        {
            if (Physics.Raycast(_probeRay, out _rayHit, Mathf.Infinity, _layerMask))
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

        #region Gizmos

        [Header("Debug / Gizmos")]
        [SerializeField] private bool _drawRayOrigin;
        [SerializeField] private bool _drawRayProbe;
        [SerializeField] private bool _drawSphereProbe;

        private void OnDrawGizmos()
        {
            DrawRayOrigin();
            DrawRayProbe();
            DrawSphereProbe();
        }

        private void DrawRayOrigin()
        {
            if (!_drawRayOrigin) return;
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_probeRay.origin, 0.1f);
        }

        private void DrawRayProbe()
        {
            if (!_drawRayProbe) return;
            Gizmos.color = Color.red;
            Gizmos.DrawRay(_probeRay);
        }

        private void DrawSphereProbe()
        {
            if (!_drawSphereProbe) return;

            var gizmoSphereCenter = _probeRay.origin + Vector3.down * _sphereMaxDistance;
            
            Gizmos.color = _isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(gizmoSphereCenter, _sphereRadius);
        }

        #endregion
    }
}