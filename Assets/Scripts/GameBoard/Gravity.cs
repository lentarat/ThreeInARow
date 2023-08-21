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


    private int _yArrayIndexOffset = 1;
    
    private List<Figure> _figuresToFall = new List<Figure>();
    private float _cellOffset;

    private void Start()
    {
        _cellOffset = _grid.GetCellsOffset();
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

        _yArrayIndexOffset = 1;

        int xMax = _grid.XDim;
        int yMax = (_grid.Figures.GetUpperBound(1) + 1) / 2;

        for (int x = 0; x < xMax; x++)
        {
            for (int y = 0; y < yMax; y++)
            {
                if (IsNotOccupiedByFigure(x, y))
                {
                    Figure spawnedFigureAboveTheGrid = _figureSpawner.SpawnAFigureAtPosition(_grid.GetSpawnPointPosition(x), Vector2.zero);
                    spawnedFigureAboveTheGrid.ArrayIndex.x = x;
                    spawnedFigureAboveTheGrid.ArrayIndex.y = yMax;

                    _grid.Figures[x, yMax] = spawnedFigureAboveTheGrid;

                    _figuresToFall.Add(spawnedFigureAboveTheGrid);

                    if (AddFiguresWhichAreAboveToFallList(x, y, yMax))
                    {
                        break;
                    }
                }
            }
        }

        if (_figuresToFall.Count > 0)
        {
            BubbleSortFiguresToFall();
            StartCoroutine(ApplyGravityToFigures());
        }

        GameManager.Instance.CurrentGameState = GameManager.GameState.Idle;
    }

    private bool IsNotOccupiedByFigure(int x, int y)
    {
        return _grid.Figures[x, y] == null;
    }

    //returns true if figures were created while adding gravitation to upper figures
    private bool AddFiguresWhichAreAboveToFallList(int x, int y, int yMax)
    {
        bool wereFiguresCreated = false;

        float figureAboveTheGridOffset = _cellOffset;

        for (int yIndex = y + 1; yIndex < yMax; yIndex++)
        {
            if (IsNotOccupiedByFigure(x, yIndex))
            {
                Figure spawnedFigureAboveTheGrid = _figureSpawner.SpawnAFigureAtPosition(_grid.GetSpawnPointPosition(x) + new Vector3(0f, figureAboveTheGridOffset, 0f), Vector2.zero);
                spawnedFigureAboveTheGrid.ArrayIndex.x = x;
                spawnedFigureAboveTheGrid.ArrayIndex.y = yMax + _yArrayIndexOffset;

                _grid.Figures[x, yMax + _yArrayIndexOffset] = spawnedFigureAboveTheGrid;
                _yArrayIndexOffset++;

                _figuresToFall.Add(spawnedFigureAboveTheGrid);

                figureAboveTheGridOffset += _cellOffset;
                wereFiguresCreated = true;
            }
            else
            {
                _figuresToFall.Add(_grid.Figures[x, yIndex]);
            }
        }

        return wereFiguresCreated;
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
        float pathPassed = 0;

        while (_figuresToFall.Count != 0)
        {
            while (pathPassed < _cellOffset)
            {
                foreach (Figure figure in _figuresToFall)
                {
                    figure.transform.position += new Vector3(0f, -Time.deltaTime * _fallSpeed, 0f);
                }

                pathPassed += Time.deltaTime * _fallSpeed;

                yield return null;
            }

            yield return null;
        }
        
        yield return null;
    }
}
