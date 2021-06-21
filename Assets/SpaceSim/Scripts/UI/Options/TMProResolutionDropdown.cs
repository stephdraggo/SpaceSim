using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace BigBoi.Menus.OptionsMenuSystem
{
    [AddComponentMenu("BigBoi/Options Menu System/Resolutions Dropdown (TMPro)")]
    [RequireComponent(typeof(TMP_Dropdown))]

    public class TMProResolutionDropdown : MonoBehaviour
    {
        private TMP_Dropdown dropdown;
        private string saveName;
        private Resolution[] resolutions;

        private void OnValidate()
        {
            saveName = "Resolution";

            dropdown = GetComponent<TMP_Dropdown>(); //connect to own dropdown

            dropdown.onValueChanged.AddListener(SetResolution); //add method to event group
        }

        private void Start()
        {
            //decided not to save an array of resolutions to playerprefs and instead generate the array each start
            resolutions = Screen.resolutions; //fill array with all possible resolutions for the current screen
            dropdown.ClearOptions(); //clear selection
            int index = 0; //index of current active resolution
            List<string> options = new List<string>(); //empty list of options
            for (int i = 0; i < resolutions.Length; i++) //for all resolutions in array
            {
                string option = resolutions[i].width + "x" + resolutions[i].height; //make string based on resolution
                options.Add(option); //add string to options list

                if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height) //if selected resolution is active resolution
                {
                    index = i; //set index to active resolution index
                }
            }

            if (PlayerPrefs.HasKey(saveName)) //if key saved
            {
                index = PlayerPrefs.GetInt(saveName); //get resolution index
                SetResolution(index); //load resolution
            }

            dropdown.AddOptions(options); //put list of options into dropdown
            dropdown.value = index; //select resolution

            dropdown.RefreshShownValue(); //refresh display
        }

        private void SetResolution(int _index)
        {
            Screen.SetResolution(resolutions[_index].width, resolutions[_index].height, Screen.fullScreenMode); //set selected resolution
            PlayerPrefs.SetInt(saveName, _index); //save resolution index
        }
    }
}