using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceSim.Ship
{
    public class ShipInput : MonoBehaviour
    {
        [Range(-1, 1)] public float pitch, yaw, roll, strafe;

        [Range(0, 1), Tooltip("Accelerator")] public float throttle;

        [SerializeField] private float throttleSpeed = 0.5f, rollSpeed = 5;

        void Update()
        {
            strafe = Input.GetAxis("Horizontal");
            
            SetAngleDirectionMouse();

            UpdateThrottleMouseWheel();
            UpdateThrottleKeyboard(KeyCode.W, KeyCode.S);
            UpdateRollKeyboard(KeyCode.Q, KeyCode.E);
        }

        #region throttle

        /// <summary>
        /// Get mouse wheel for accelerator and clamp value
        /// </summary>
        private void UpdateThrottleMouseWheel()
        {
            throttle += Input.GetAxis("Mouse ScrollWheel");
            throttle = Mathf.Clamp01(throttle);
        }

        /// <summary>
        /// Get mouse keys for accelerator
        /// </summary>
        private void UpdateThrottleKeyboard(KeyCode increaseKey, KeyCode decreaseKey)
        {
            float target = throttle;
            if (Input.GetKey(increaseKey))
            {
                target = 1;
            }
            else if (Input.GetKey(decreaseKey))
            {
                target = 0;
            }

            throttle = Mathf.MoveTowards(throttle, target, Time.deltaTime * throttleSpeed);
        }

        #endregion

        private void SetAngleDirectionMouse()
        {
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
        private void UpdateRollKeyboard(KeyCode increaseKey, KeyCode decreaseKey)
        {
            float target = 0;
            if (Input.GetKey(increaseKey))
            {
                target = 1;
            }
            else if (Input.GetKey(decreaseKey))
            {
                target = -1;
            }

            roll = Mathf.MoveTowards(roll, target, Time.deltaTime * rollSpeed);
        }
    }
}