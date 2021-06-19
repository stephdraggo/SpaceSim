using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceSim.Ship
{
    public class ShipParticles : MonoBehaviour
    {
        #region Variables

        [SerializeField]
        private ParticleSystem aL, aR, bL, bR, cL, cR, dL, dR, eTL, eTR, eBL, eBR;

        private float pitch, yaw;

        #endregion

        private void Awake() { }

        private void Start() {
            aL.Stop();
            aR.Stop();
            bL.Stop();
            bR.Stop();
            cL.Stop();
            cR.Stop();
            dL.Stop();
            dR.Stop();
            eTL.Stop();
            eTR.Stop();
            eBL.Stop();
            eBR.Stop();
        }

        private void Update() {
            CheckInputs();
        }

        public void UpdateMouse(float pitch, float yaw) {
            this.pitch = pitch;
            this.yaw = yaw;
        }

        private void CheckInputs() {
            //forward and backward
            CheckKey(KeyCode.W, aL, true);
            CheckKey(KeyCode.W, aR, true);
            CheckKey(KeyCode.S, bL, true);
            CheckKey(KeyCode.S, bR, true);

            //sideways
            CheckKey(KeyCode.A, cR, true);
            CheckKey(KeyCode.D, cL, true);
            
            //angles
            MouseInput(pitch,true);
            MouseInput(yaw,false);
        }


        private void CheckKey(KeyCode key, ParticleSystem system, bool play) {
            if (Input.GetKeyDown(key)) {
                if (play) system.Play();
                else system.Stop();
            }
            else if (Input.GetKeyUp(key)) {
                if (play) system.Stop();
                else system.Play();
            }
        }

        private void MouseInput(float value, bool vertical) {
            if (value > 0.1f) {
                if (vertical) {
                    eBL.Stop();
                    eBR.Stop();
                    eTL.Play();
                    eTR.Play();
                }
                else {
                    dL.Play();
                    dR.Stop();
                }
            }
            else if (value < -0.1f) {
                if (vertical) {
                    eBL.Play();
                    eBR.Play();
                    eTL.Stop();
                    eTR.Stop();
                }
                else {
                    dL.Stop();
                    dR.Play();
                }
            }
            else {
                if (vertical) {
                    eBL.Stop();
                    eBR.Stop();
                    eTL.Stop();
                    eTR.Stop();
                }
                else {
                    dL.Stop();
                    dR.Stop();
                }
            }
        }
    }
}