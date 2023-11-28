using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifferentPlatformsOptimizer : MonoBehaviour
{
    [SerializeField] private Grid _grid;

    void Awake()
    {
        SetCellsOffsetMultiplierRelativeScreen();
    }

    private void SetCellsOffsetMultiplierRelativeScreen()
    {
        float screenWidthToHeightRatio = (float)Screen.width / (float)Screen.height;
        //Debug.Log("Width: " + Screen.width + " Height: " + Screen.height);
        //Debug.Log((float)Screen.width );
        //Debug.Log(Screen.dpi);
         Debug.Log(Camera.main.orthographicSize);
        //_grid.CellsOffsetMultiplier = Screen.dpi / 1000f;

    }

    private void SetApplicationFPS()
    {
        Application.targetFrameRate = 60;
    }
}
