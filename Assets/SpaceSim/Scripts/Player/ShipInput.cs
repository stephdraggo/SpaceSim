using System;
using System.Collections;
using System.Collections.Generic;
using SpaceSim.Mining;
using SpaceSim.UI;
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
        private float rayLength;

        [SerializeField]
        private LineRenderer renderLine;

        private Hittable currentHit;

        void Update() {
            Move();


            RaycastClick();
            
            RaycastView();
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

        private void RaycastClick() {
            if (Input.GetKey(KeyCode.Mouse0)) {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit, rayLength)) {
                    //RenderLine(renderLine.transform.position, hit.transform.position);

                    if (hit.collider.transform.parent.TryGetComponent(out Hittable hittable)) {
                        Debug.Log("the thing is hittable");
                        currentHit = hittable;
                        currentHit.OnHit();
                        if (hittable is Asteroid) {
                            //asteroid
                            Debug.Log(hittable.name + " is an asteroid");
                        }
                        else {
                            //enemy
                            Debug.Log(hittable.name + " is not an asteroid");
                        }
                    }
                    else NotHitting();
                }
                else NotHitting();
            }
            else NotHitting();
        }

        private void NotHitting() {
            if (currentHit == null) return;

            currentHit.OnLeave();

            currentHit = null;
        }

        private void RenderLine(Vector3 posA, Vector3 posB) {
            if (!renderLine.enabled) renderLine.enabled = true;
            renderLine.SetPosition(0, posA);
            renderLine.SetPosition(1, posB);
        }

        private void UnRenderLine() {
            if (!renderLine.enabled) return;
            renderLine.enabled = false;
        }

        #endregion

        #region view

        private void RaycastView() {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, rayLength)) {
                RenderLine(renderLine.transform.position, hit.transform.position);

                if (hit.collider.transform.parent.TryGetComponent(out Hittable hittable)) {
                    Debug.Log("the thing is hittable");

                    CanvasManager.Instance.UpdateView(hittable.Description);
                }
                else NotViewing();
            }
            else NotViewing();
        }

        private void NotViewing() {
            UnRenderLine();
            CanvasManager.Instance.UpdateView("");
        }

        #endregion
    }
}