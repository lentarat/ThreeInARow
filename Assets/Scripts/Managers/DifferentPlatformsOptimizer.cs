using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifferentPlatformsOptimizer : MonoBehaviour
{
    void Awake()
    {
        SetCellsOffsetMultiplierRelativeScreen();
    }

    private void SetCellsOffsetMultiplierRelativeScreen()
    {
        Debug.Log(Screen.dpi);
    }

    private void SetApplicationFPS()
    {
        Application.targetFrameRate = 60;
    }
}
