using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceSim.Ship
{
    [RequireComponent(typeof(Rigidbody))]
    public class ShipPhysics : MonoBehaviour
    {
        private Rigidbody rBody;

        [SerializeField, Tooltip("x: lateral-sideways, y: vertical, z: longitudinal-forward/backward")]
        private Vector3 linearForce = new Vector3(100, 100, 100);

        [SerializeField, Tooltip("x: pitch, y: yaw, z:roll")]
        private Vector3 angularForce = new Vector3(100, 100, 100);

        [SerializeField] private float forceMultiplier = 100, angleMultiplier = 0.1f;

        private Vector3 appliedLinearForce = Vector3.zero, appliedAngularForce = Vector3.zero;

        private void Awake()
        {
            rBody = GetComponent<Rigidbody>();
            if (rBody == null) Debug.LogError($"Ship Physics cannot locate rigidbody on {name}", gameObject);
        }

        private void FixedUpdate()
        {
            rBody.AddRelativeForce(appliedLinearForce * forceMultiplier, ForceMode.Force);
            rBody.AddRelativeTorque(appliedAngularForce * angleMultiplier, ForceMode.Force);
        }

        /// <summary>
        /// Linear and angular input gets multiplied and applied here.
        /// </summary>
        public void SetPhysicsInput(Vector3 linearInput, Vector3 angularInput)
        {
            appliedLinearForce = MultiplyVector3(linearInput, linearForce);
            appliedAngularForce = MultiplyVector3(angularInput, angularForce);
        }

        public static Vector3 MultiplyVector3(Vector3 a, Vector3 b)
        {
            float x = a.x * b.x;
            float y = a.y * b.y;
            float z = a.z * b.z;
            return new Vector3(x, y, z);
        }
    }
}