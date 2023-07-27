using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class GameBoardInitializator : MonoBehaviour
{
    [Header("Figures Positions")]
    [SerializeField] private Transform _figuresPositionsParent;
    [SerializeField, Range(0.1f, 1f)] private float _positionsOffset;

    [Header("Board Background")]
    [SerializeField] private RectTransform _gameBoardBackRectTransform;
    [SerializeField, Range(0.1f, 1f)] private float _screenSizeRelativeToWidth;

    private void Start()
    {
        Debug.Log("start");

        SetFigurePositions();
        AdjustBackground();
    }

    private void SetFigurePositions()
    {
        Level currentLevel = LevelsManager.Instance.GetCurrentLevel();

        for (int i = 0; i < currentLevel.Height; i++)
        {
            for (int j = 0; i < currentLevel.Width; j++)
            {

            }
        }
    }

    private void AdjustBackground()
    {
        float adjustedScreenWidth = Screen.width * _screenSizeRelativeToWidth;
        _gameBoardBackRectTransform.sizeDelta = new Vector2(adjustedScreenWidth, adjustedScreenWidth);
    }
}
