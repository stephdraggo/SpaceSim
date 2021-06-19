using System;
using System.Collections;
using System.Collections.Generic;
using SpaceSim.Mining;
using UnityEditor;
using UnityEngine;

namespace SpaceSim.Ship
{
    public class ShipInput : MonoBehaviour
    {
        [Range(-1, 1)]
        public float pitch, yaw, roll, strafe;

        [Range(0, 1), Tooltip("Accelerator")]
        public float throttle;

        [SerializeField]
        private float throttleSpeed = 0.5f, rollSpeed = 5;

        [SerializeField]
        private Transform shootPoint;

        private Ray ray;
        
        void Update() {
            Move();


            if (Input.GetKey(KeyCode.Mouse0)) {
                RaycastClick();
            }
        }

        #region move

        private void Move() {
            strafe = Input.GetAxis("Horizontal");

            SetAngleDirectionMouse();

            UpdateThrottleMouseWheel();
            UpdateThrottleKeyboard(KeyCode.W, KeyCode.S);
            UpdateRollKeyboard(KeyCode.Q, KeyCode.E);
        }

        /// <summary>
        /// Get mouse wheel for accelerator and clamp value
        /// </summary>
        private void UpdateThrottleMouseWheel() {
            throttle += Input.GetAxis("Mouse ScrollWheel");
            throttle = Mathf.Clamp01(throttle);
        }

        /// <summary>
        /// Get mouse keys for accelerator
        /// </summary>
        private void UpdateThrottleKeyboard(KeyCode increaseKey, KeyCode decreaseKey) {
            float target = throttle;
            if (Input.GetKey(increaseKey)) {
                target = 1;
            }
            else if (Input.GetKey(decreaseKey)) {
                target = 0;
            }

            throttle = Mathf.MoveTowards(throttle, target, Time.deltaTime * throttleSpeed);
        }


        private void SetAngleDirectionMouse() {
            Vector3 mousePos = Input.mousePosition;

            pitch = -(mousePos.y - (Screen.height * 0.5f)) / (Screen.height * 0.5f); //vertical
            yaw = (mousePos.x - (Screen.width * 0.5f)) / (Screen.width * 0.5f); //horizontal

            //clamp 'em
            pitch = Mathf.Clamp(pitch, -1, 1);
            yaw = Mathf.Clamp(yaw, -1, 1);
        }

        /// <summary>
        /// Get mouse keys for accelerator
        /// </summary>
        private void UpdateRollKeyboard(KeyCode increaseKey, KeyCode decreaseKey) {
            float target = 0;
            if (Input.GetKey(increaseKey)) {
                target = 1;
            }
            else if (Input.GetKey(decreaseKey)) {
                target = -1;
            }

            roll = Mathf.MoveTowards(roll, target, Time.deltaTime * rollSpeed);
        }

        #endregion

        #region interact

        //hecc
        private void RaycastClick() {
            Vector3 from = shootPoint.position;
            Vector3 to = from + shootPoint.forward * 10;
            ray = new Ray(from, to);
            if (Physics.Raycast(ray, out RaycastHit hit, 50)) {
                if (hit.collider.TryGetComponent(out Hittable hittable)) {
                    if (hittable is Asteroid) {
                        //asteroid
                        Debug.Log(hittable.name + " is an asteroid");
                    }
                    else {
                        //enemy
                    }
                }
            }
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(ray.origin,ray.direction);
        }

        #endregion
    }
}