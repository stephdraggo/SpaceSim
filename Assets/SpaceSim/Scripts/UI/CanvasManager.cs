using System;
using System.Collections;
using System.Collections.Generic;
using SpaceSim.Mining;
using SpaceSim.Ship;
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
        private TMP_Text copperDisplay, ironDisplay, diamondDisplay, viewText;
        [SerializeField]
        private GameObject optionsPanel;

        //-----------private---------------
        private int copperCount, ironCount, diamondCount;

        #endregion

        private void Awake() {
            Instance = this;
        }

        // Start is called before the first frame update
        void Start() {
            HideOptions();
            UpdateDisplay();
        }

        public void ShowOptions(bool yes = true) {
            ShipInput.CanMove = !yes;
            optionsPanel.SetActive(yes);
        }

        public void HideOptions() {
            ShowOptions(false);
        }

        public void UpdateView(string text) {
            viewText.text = text;
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