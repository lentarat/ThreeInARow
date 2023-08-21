using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private FigureSpawner _figureSpawner;

    [Header("Grid Size")]
    [SerializeField] private int _xDim;
    public int XDim
    {
        get => _xDim;
    }

    [SerializeField] private int _yDim;
    public int YDim
    {
        get => _yDim;
    }
        
    [SerializeField] private float _cellsOffsetMultiplier;

    [Header("Background Cell Prefab")]
    [SerializeField] private GameObject _backgroundCellPrefab;

    private Figure[,] _figures;
    public Figure[,] Figures
    {
        get => _figures;
    }
    private Vector3[] _spawnPointsPositions;
    private Vector2 _centeredGridInWorldPosition;

    private void Start()
    {
        CenterTheGrid();

        PrepareFigureSpawner();

        InitializeGridBackgroundCells();
        InitializeFigures();
        CreateSpawnPointsAboveTheGrid();

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

    public Vector3 GetSpawnPointPosition(int xArrayIndex)
    {
        return _spawnPointsPositions[xArrayIndex];  
    }

    private void CenterTheGrid()
    {
        _centeredGridInWorldPosition = new Vector2(transform.position.x - _xDim / 2f + 0.5f, transform.position.y - _yDim / 2f + 0.5f);
    }

    private void PrepareFigureSpawner()
    {
        //_figureSpawner.SetCenteredGridInWorldPosition(_centeredGridInWorldPosition);
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
        _figures = new Figure[_xDim, _yDim * 2]; // multiplication by two is caused due to extra space needed for figures above the grid

        for (int x = 0; x < _xDim; x++)
        {
            for (int y = 0; y < _yDim; y++)
            {
                Figure figure = _figureSpawner.SpawnAFigureAtPosition(new Vector2(x, y), _centeredGridInWorldPosition);
                _figures[x, y] = figure;
            }
        }
    }

    private void CreateSpawnPointsAboveTheGrid()
    {
        GameObject spawnPoint = new GameObject();
        Vector3 cellsOffset = new Vector3(0f, GetCellsOffset(), 0f);

        _spawnPointsPositions = new Vector3[_xDim];

        for (int x = 0; x < _xDim; x++)
        {
            GameObject createdSpawnPoint = Instantiate(spawnPoint, _figures[x, _yDim - 1].transform.position + cellsOffset, Quaternion.identity);
            _spawnPointsPositions[x] = createdSpawnPoint.transform.position;
        }

        Destroy(spawnPoint);
    }

    //private void RememberFiguresMaxYPositions()
    //{
    //    _figuresMaxYPositions = new float[_xDim];

    //    for (int x = 0; x < _xDim; x++)
    //    {
    //        _figuresMaxYPositions[x] = _figures[x, _yDim - 1].transform.position.y;
    //    }
    //}
    private void ResizeBoardAccordingToScreenSize()
    {
        gameObject.transform.localScale *= _cellsOffsetMultiplier;
    }
}