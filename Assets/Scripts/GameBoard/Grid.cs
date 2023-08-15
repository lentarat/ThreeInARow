using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private FigureSpawner _figureSpawner;

    [Header("Grid Size")]
    [SerializeField] private int _xDim;
    [SerializeField] private int _yDim;
    [SerializeField] private float _cellsOffsetMultiplier;

    [Header("Background Cell Prefab")]
    [SerializeField] private GameObject _backgroundCellPrefab;

    private Figure[,] _figures;
    public Figure[,] Figures
    {
        get => _figures;
    }
    private float[] _figuresMaxYPositions;
    private Vector2 _centeredGridInWorldPosition;

    private void Start()
    {
        CenterTheGrid();

        PrepareFigureSpawner();

        InitializeGridBackgroundCells();
        InitializeFigures();
        RememberFiguresMaxYPositions();

        ResizeBoardAccordingToScreenSize();
    }
    //public float GetTheHighestCellYPosition()
    //{
    //    return (_yDim / 2f + 0.5f - 1f) * _cellsOffsetMultiplier;
    //}

    public float GetCellsOffset()
    {
        if (_xDim >= 2)
        {
            return Mathf.Abs((_figures[0, 0].transform.position - _figures[1, 0].transform.position).x);
        }
        else
        {
            if (_yDim >= 2)
            {
                return Mathf.Abs((_figures[0, 0].transform.position - _figures[0, 1].transform.position).y);
            }
            else
            {
                return 0f;
            }
        }
    }

    public float GetFigureMaxYPositionAt(int xArrayIndex)
    {
        return _figuresMaxYPositions[xArrayIndex];
    }

    private void CenterTheGrid()
    {
        _centeredGridInWorldPosition = new Vector2(transform.position.x - _xDim / 2f + 0.5f, transform.position.y - _yDim / 2f + 0.5f);
    }

    private void PrepareFigureSpawner()
    {
        _figureSpawner.SetCenteredGridInWorldPosition(_centeredGridInWorldPosition);
        _figureSpawner.SetGridTransform(transform);
    }

    private void InitializeGridBackgroundCells()
    {
        for (int x = 0; x < _xDim; x++)
        {
            for (int y = 0; y < _yDim; y++)
            {
                Vector2 backgroundCellOffset = new Vector3(x, y);
                Instantiate(_backgroundCellPrefab, _centeredGridInWorldPosition + backgroundCellOffset, Quaternion.identity, transform);
            }
        }
    }

    private void InitializeFigures()
    {
        _figures = new Figure[_xDim, _yDim];

        for (int x = 0; x < _xDim; x++)
        {
            for (int y = 0; y < _yDim; y++)
            {
                Figure figure = _figureSpawner.SpawnAFigureAtPosition(x, y);
                _figures[x, y] = figure;
            }
        }
    }

    private void RememberFiguresMaxYPositions()
    {
        _figuresMaxYPositions = new float[_xDim];

        for (int x = 0; x < _xDim; x++)
        {
            _figuresMaxYPositions[x] = _figures[x, _yDim - 1].transform.position.y;
        }
    }
    private void ResizeBoardAccordingToScreenSize()
    {
        gameObject.transform.localScale *= _cellsOffsetMultiplier;
    }
}