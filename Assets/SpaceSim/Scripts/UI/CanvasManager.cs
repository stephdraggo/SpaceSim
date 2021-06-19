using System;
using System.Collections;
using System.Collections.Generic;
using SpaceSim.Mining;
using TMPro;
using UnityEngine;

namespace SpaceSim.UI
{

    public class CanvasManager : MonoBehaviour
    {
        #region Variables
        //-----------static----------------
        public static CanvasManager Instance;

        //-----------serialised-------------
        [SerializeField]
        private TMP_Text copperDisplay, ironDisplay, diamondDisplay;
        
        //-----------private---------------
        private int copperCount, ironCount, diamondCount;

        #endregion

        private void Awake() {
            Instance = this;
        }

        // Start is called before the first frame update
        void Start() {
            
            UpdateDisplay();
        }


        public void UpdateResource(ResourceType type, int difference) {
            switch (type) {
                case ResourceType.Copper:
                    copperCount += difference;
                    break;
                case ResourceType.Iron:
                    ironCount += difference;
                    break;
                case ResourceType.Diamond:
                    diamondCount += difference;
                    break;
                case ResourceType.Light:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            UpdateDisplay();
        }

        private void UpdateDisplay() {
            copperDisplay.text = "Copper: " + copperCount;
            ironDisplay.text = "Iron: " + ironCount;
            diamondDisplay.text = "Diamond: " + diamondCount;
        }
    }
}