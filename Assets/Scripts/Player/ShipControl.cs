using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceSim.Ship
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(ShipPhysics))]
    [RequireComponent(typeof(ShipInput))]
    public class ShipControl : MonoBehaviour
    {
        private ShipInput input;
        private ShipPhysics physics;
        private ShipParticles particles;

        private Vector3 linear, angular;

        private void Awake() {
            input = GetComponent<ShipInput>();
            physics = GetComponent<ShipPhysics>();
            particles = GetComponent<ShipParticles>();

            if (input == null || physics == null || particles == null) Debug.LogError($"{name} is missing ship parts.");
            if (!input.enabled) input.enabled = true;
            if (!physics.enabled) physics.enabled = true;
        }

        void Update() {
            linear = new Vector3(input.strafe, 0, input.throttle);
            angular = new Vector3(input.pitch, input.yaw, input.roll);
            physics.SetPhysicsInput(linear, angular);
            particles.UpdateMouse(input.pitch, input.yaw);
        }
    }
}