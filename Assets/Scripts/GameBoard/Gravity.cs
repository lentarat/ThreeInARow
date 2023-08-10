using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    [Header("Dependecies")]
    [SerializeField] private Grid _grid;
    [SerializeField] private FigureDestroyer _figureDestroyer;
    [SerializeField] private FigureSpawner _figureSpawner;

    private List<Figure> _listOfFiguresToBeAffectedByGravity = new List<Figure>();

    private void OnEnable()
    {
        _figureDestroyer.OnFigureDestroyed += ApplyGravity;
    }

    private void OnDisable()
    {
        _figureDestroyer.OnFigureDestroyed -= ApplyGravity;
    }

    private void ApplyGravity()
    {
        GameManager.Instance.CurrentGameState = GameManager.GameState.FiguresFalling;

        int xMax = _grid.Figures.GetUpperBound(0);
        int yMax = _grid.Figures.GetUpperBound(1);

        for (int x = 0; x < xMax; x++)
        {
            for (int y = 0; y < yMax; y++)
            {
                if (IsOccupiedByFigure(x, y) == false)
                {
                    Figure figureToBeAffectedByGravity = GetTheClosestFigureOnY(x, y, yMax);                       
                }
                GetFigureIfOccupied(x, y);
                if (_grid.Figures[x, y] == null)
                {
                    Debug.Log("null is at " + x + " " + y);
                }
            }
        }

        GameManager.Instance.CurrentGameState = GameManager.GameState.Idle;
    }

    private bool IsOccupiedByFigure(int x, int y)
    {
        return _grid.Figures[x, y] != null;
    }

    private Figure GetTheClosestFigureOnY(int x, int y, int yMax)
    {
        for (int yIndex = y; yIndex <= yMax; yIndex++)
        {
            if (_grid.Figures[x, y] != null)
            {
                return _grid.Figures[x, y];
            }
        }

        return SpawnAFigureBeyondTheGrid();
    }

    private Figure SpawnAFigureBeyondTheGrid()
    {
        
    }
}
