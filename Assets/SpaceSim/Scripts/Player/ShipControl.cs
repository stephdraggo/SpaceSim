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


            try {
                input.enabled = true;
                physics.enabled = true;
            }
            catch {
                Debug.LogError($"{name} is missing input and/or physics.");
            }

            if (particles == null)
                Debug.LogError($"{name} is missing particle effects.");
        }

        void Update() {
            if (ShipInput.CanMove) {
                linear = new Vector3(input.strafe, 0, input.throttle);
                angular = new Vector3(input.pitch, input.yaw, input.roll);
                physics.SetPhysicsInput(linear, angular);
                particles.UpdateMouse(input.pitch, input.yaw);
            }
        }
    }
}