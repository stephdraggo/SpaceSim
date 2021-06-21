using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiAliasing : MonoBehaviour
{
    public void ChangeAntiAliasing(int multiplier) {
        QualitySettings.antiAliasing = multiplier * 10;
    }

    public void AnistrophicFiltering(bool enabling) {
        switch (enabling) {
            case true:
                QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
                break;
            case false:
                QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
                break;
        }

        Debug.Log("Anistrophic filtering set to " + QualitySettings.anisotropicFiltering);
    }

    public void FrameRateLimit(string stringValue) {
        if (int.TryParse(stringValue, out int value) && value >= 1) {
            Application.targetFrameRate = value;
        }
    }
}