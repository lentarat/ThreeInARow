using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    [Header("Dependecies")]
    [SerializeField] private Grid _grid;
    [SerializeField] private FigureDestroyer _figureDestroyer;
    [SerializeField] private FigureSpawner _figureSpawner;

    [Header("Common")]
    [SerializeField] private float _fallSpeed;

    private List<Figure> _figuresToFall = new List<Figure>();
    private float _cellOffset;

    private void Start()
    {
        _cellOffset = _grid.GetCellsOffset();
    }

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

        for (int x = 0; x <= xMax; x++)
        {
            for (int y = 0; y <= yMax; y++)
            {
                if (IsOccupiedByFigure(x, y) == false)
                {
                    Vector2 theUpperFigurePosition = new Vector2(x, _grid.GetFigureMaxYPositionAt(x) /*_grid.GetTheHighestCellYPosition()*/);

                    Figure spawnedFigureAboveTheGrid = SpawnAFigureAboveTheGridAtPosition(theUpperFigurePosition.x, theUpperFigurePosition.y + _cellOffset);
                    _figuresToFall.Add(spawnedFigureAboveTheGrid);

                    AddFiguresWhichAreAboveToFallList(x, y, yMax);
                }
                if (_grid.Figures[x, y] == null)
                {
                }
            }
        }

        if (_figuresToFall.Count > 0)
        {
            StartCoroutine(ApplyGravityToFigures());
        }

        GameManager.Instance.CurrentGameState = GameManager.GameState.Idle;
    }

    private bool IsOccupiedByFigure(int x, int y)
    {
        return _grid.Figures[x, y] != null;
    }

    private void AddFiguresWhichAreAboveToFallList(int x, int y, int yMax)
    {
        float figureAboveTheGridOffset = _cellOffset;
        Vector2 theUpperFigurePosition = new Vector2(x, _grid.GetFigureMaxYPositionAt(x)); 

        for (int yIndex = y + 1; yIndex <= yMax; yIndex++)
        {
            if (_grid.Figures[x, yIndex] == null)
            {
                figureAboveTheGridOffset += _cellOffset;


                Figure spawnedFigureAboveTheGrid = SpawnAFigureAboveTheGridAtPosition(theUpperFigurePosition.x, theUpperFigurePosition.y + figureAboveTheGridOffset);
                _figuresToFall.Add(spawnedFigureAboveTheGrid);
            }
            else
            {
                _figuresToFall.Add(_grid.Figures[x, yIndex]);
            }
        }
    }

    //private Figure GetTheClosestFigureOnY(int x, int y, int yMax)
    //{
    //    for (int yIndex = y + 1; yIndex <= yMax; yIndex++)
    //    {
    //        if (_grid.Figures[x, yIndex] != null)
    //        {
    //            return _grid.Figures[x, y];
    //        }
    //    }

    //    Vector3 upperFigurePosition = _grid.Figures[x, yMax].transform.position;

    //    return SpawnAFigureAboveTheGrid(upperFigurePosition.x, upperFigurePosition.y + _cellOffset);
    //}

    private Figure SpawnAFigureAboveTheGridAtPosition(float x, float y)
    {
        return _figureSpawner.SpawnAFigureAtPosition(x, y);
    }

    private IEnumerator ApplyGravityToFigures()
    {
        float pathPassed = 0;

        while (pathPassed < _cellOffset)
        {
            foreach (Figure figure in _figuresToFall)
            {
                figure.transform.position += new Vector3(0f, -Time.deltaTime * _fallSpeed, 0f);
                
                Debug.Log("Gravity " + pathPassed + " " + _figuresToFall.Count);
            }

            pathPassed += Time.deltaTime * _fallSpeed;

            yield return null;
        }

        yield return null;
    }
}
