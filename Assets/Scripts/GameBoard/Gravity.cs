using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    [Header("Dependecies")]
    [SerializeField] private Grid _grid;
    [SerializeField] private FigureSpawner _figureSpawner;

    [Header("Common")]
    [SerializeField] private float _fallSpeed;

    public static event System.Action OnFiguresFellDown;
    
    private List<Figure> _figuresToFall = new List<Figure>();
    //private float _cellOffset;
    private int xMax;
    private int yMax;

    private int _yArrayIndexOffset;

    private void Start()
    {
        //_cellOffset = ;

        xMax = _grid.XDim;
        yMax = (_grid.Figures.GetUpperBound(1) + 1) / 2;
    }

    private void OnEnable()
    {
        FigureDestroyer.OnFigureDestroyed += ApplyGravity;
    }

    private void OnDisable()
    {
        FigureDestroyer.OnFigureDestroyed -= ApplyGravity;
    }

    private void ApplyGravity()
    {
        GameManager.Instance.CurrentGameState = GameManager.GameState.FiguresFalling;

        bool startedAddingFiguresToFallList;
        for (int x = 0; x < xMax; x++)
        {
            _yArrayIndexOffset = 0;
            startedAddingFiguresToFallList = false;

            for (int y = 0; y < yMax; y++)
            {
                if (IsNotOccupiedByFigure(x, y))
                {
                    SpawnAFigureAtIndex(x, yMax);

                    startedAddingFiguresToFallList = true;
                    _yArrayIndexOffset++;
                }
                else
                {
                    if (startedAddingFiguresToFallList)
                    {
                        _figuresToFall.Add(_grid.Figures[x, y]);
                    }
                }
            }
        }

        if (_figuresToFall.Count > 0)
        {
            BubbleSortFiguresToFall();
            StartCoroutine(ApplyGravityToFigures());
        }
    }

    private void SpawnAFigureAtIndex(int x, int yMax)
    {
        Vector2 spawnLocation = _grid.GetSpawnPointPosition(x) + new Vector3(0f, _grid.CellsOffset * _yArrayIndexOffset, 0f);

        Figure spawnedFigureAboveTheGrid = _figureSpawner.SpawnAFigureAtPosition(spawnLocation, Vector2.zero);
        spawnedFigureAboveTheGrid.ArrayIndex.x = x;
        spawnedFigureAboveTheGrid.ArrayIndex.y = yMax + _yArrayIndexOffset;
        
        _grid.Figures[x, yMax + _yArrayIndexOffset] = spawnedFigureAboveTheGrid;

        _figuresToFall.Add(spawnedFigureAboveTheGrid);
    }

    private bool IsNotOccupiedByFigure(int x, int y)
    {
        return _grid.Figures[x, y] == null;
    }

    private void BubbleSortFiguresToFall()
    {
        bool swapped;
        int lastY;

        do
        {
            swapped = false;
            lastY = (int)_figuresToFall[0].ArrayIndex.y;

            for (int i = 1; i < _figuresToFall.Count; i++)
            {
                if (_figuresToFall[i].ArrayIndex.y < lastY)
                {
                    Figure tempFigure = _figuresToFall[i - 1];
                    _figuresToFall[i - 1] = _figuresToFall[i];
                    _figuresToFall[i] = tempFigure;

                    swapped = true;
                }

                lastY = (int)_figuresToFall[i].ArrayIndex.y;
            }
        } while (swapped == true);
    }

    private IEnumerator ApplyGravityToFigures()
    {
        float pathPassed = 0f;

        while (_figuresToFall.Count != 0)
        {
            while (pathPassed < _grid.CellsOffset)
            {
                TranslatePositionEachFigureInFallListDown();

                pathPassed += Time.deltaTime * _fallSpeed * _grid.CellsOffsetMultiplier;

                yield return null;
            }

            pathPassed = 0f;

            RearrangeFigureArrayIndexes();

            for (int i = 0; i < _figuresToFall.Count; i++)
            {
                if (HasReachedTheLowestPoint(_figuresToFall[i]) || (HasAnotherFigureBelow(_figuresToFall[i])))
                {
                    RemoveFigureFromFallList(_figuresToFall[i]);
                    i--;
                }
            }

            yield return null;
        }

        GameManager.Instance.CurrentGameState = GameManager.GameState.Idle;

        OnFiguresFellDown?.Invoke();

        yield return null;
    }

    private bool HasReachedTheLowestPoint(Figure figure) => (int)figure.ArrayIndex.y <= 0;

    private bool HasAnotherFigureBelow(Figure figure) 
    {
        Figure figureBelow = _grid.Figures[(int)figure.ArrayIndex.x, (int)figure.ArrayIndex.y - 1];

        if (figureBelow != null)
        {
            if (_figuresToFall.Contains(figureBelow) == false)
            {
                return true;
            }
        }
        return false;
    }
    private void RemoveFigureFromFallList(Figure figure)
    {
        _figuresToFall.Remove(figure);
    }

    private void TranslatePositionEachFigureInFallListDown()
    {
        foreach (Figure figure in _figuresToFall)
        {
            figure.transform.position += new Vector3(0f, -Time.deltaTime * _fallSpeed * _grid.CellsOffsetMultiplier, 0f);
        }
    }

    private void RearrangeFigureArrayIndexes()
    {
        foreach (Figure figure in _figuresToFall)
        {
            RearrangeRealArray(figure);

            figure.ArrayIndex.y--;
        }

        void RearrangeRealArray(Figure figure)
        {
            int x = (int)figure.ArrayIndex.x;
            int y = (int)figure.ArrayIndex.y;

            _grid.Figures[x, y - 1] = _grid.Figures[x, y];
            _grid.Figures[x, y] = null;
        }
    }
}
