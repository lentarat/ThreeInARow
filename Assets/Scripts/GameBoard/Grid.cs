using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private FigureSpawner _figureSpawner;

    [Header("Grid Size")]
    [SerializeField] private int _xDim;
    public int XDim => _xDim;

    [SerializeField] private int _yDim;
    public int YDim => _yDim;
        
    [SerializeField] private float _cellsOffsetMultiplier;
    public float CellsOffsetMultiplier => _cellsOffsetMultiplier;

    [Header("Background Cell Prefab")]
    [SerializeField] private GameObject _backgroundCellPrefab;

    [Header("Hierarchy Parents")]
    [SerializeField] Transform _backgroundCellsParent;
    [SerializeField] Transform _spawnPointsCellParent;

    private Figure[,] _figures;
    public Figure[,] Figures => _figures;

    private Vector2[,] _cellsPositions;
    public Vector2[,] CellsPositions => _cellsPositions;

    private float _cellsOffset;
    public float CellsOffset => _cellsOffset;

    public static event System.Action OnGridReady;

    private Vector3[] _spawnPointsPositions;
    private Vector2 _centeredGridInWorldPosition;

    private void Start()
    {
        CenterTheGrid();

        InitializeGridBackgroundCells();
        InitializeFigures();

        FindCellsOffset();

        ResizeBoardAccordingToScreenSize();

        FindCellsOffset();
        FindCellsPositions();
        CreateSpawnPointsAboveTheGrid();

        OnGridReady?.Invoke();
    }

    public Vector3 GetSpawnPointPosition(int xArrayIndex)
    {
        return _spawnPointsPositions[xArrayIndex];  
    }

    private void CenterTheGrid()
    {
        _centeredGridInWorldPosition = new Vector2(transform.position.x - XDim / 2f + 0.5f, transform.position.y - YDim / 2f + 0.5f);
    } 

    private void InitializeGridBackgroundCells()
    {
        for (int x = 0; x < XDim; x++)
        {
            for (int y = 0; y < YDim; y++)
            {
                Vector2 backgroundCellOffset = new Vector3(x, y);
                GameObject gridCell = Instantiate(_backgroundCellPrefab, _centeredGridInWorldPosition + backgroundCellOffset, Quaternion.identity, _backgroundCellsParent);
                gridCell.isStatic = true;
            }
        }
    }

    private void InitializeFigures()
    {
        _figures = new Figure[XDim, YDim * 2]; // multiplication by two is caused due to extra space needed for figures above the grid

        for (int x = 0; x < XDim; x++)
        {
            for (int y = 0; y < YDim; y++)
            {
                Figure figure = _figureSpawner.SpawnAFigureAtPosition(new Vector2(x, y), _centeredGridInWorldPosition);
                _figures[x, y] = figure;
            }
        }
    }

    private void FindCellsOffset()
    {
        if (XDim >= 2)
        {
            _cellsOffset = Mathf.Abs((_figures[0, 0].transform.position - _figures[1, 0].transform.position).x);
        }
        else
        {
            if (YDim >= 2)
            {
                _cellsOffset = Mathf.Abs((_figures[0, 0].transform.position - _figures[0, 1].transform.position).y);
            }
            else
            {
                _cellsOffset = 0f;
            }
        }
    }

    private void FindCellsPositions()
    {
        _cellsPositions = new Vector2[XDim, YDim * 2];

        for (int x = 0; x < XDim; x++)
        {
            int y = 0;
            for (; y < YDim; y++)
            {
                _cellsPositions[x, y] = _figures[x, y].transform.position;
            }
            Debug.Log(y);
            Vector2 lastPosition = _figures[x, YDim - 1].transform.position + new Vector3(0f,CellsOffset,0f);
            for (; y < YDim * 2; y++)
            {
                _cellsPositions[x, y] = lastPosition;
                lastPosition += new Vector2(0f, CellsOffset);
            }
        }
    }

    private void CreateSpawnPointsAboveTheGrid()
    {
        GameObject spawnPoint = new GameObject();
        Vector3 cellsOffset = new Vector3(0f, CellsOffset, 0f);

        _spawnPointsPositions = new Vector3[XDim];

        for (int x = 0; x < XDim; x++)
        {
            GameObject createdSpawnPoint = Instantiate(spawnPoint, _figures[x, YDim - 1].transform.position + cellsOffset, Quaternion.identity, _spawnPointsCellParent);
            createdSpawnPoint.name = "Spawn Point " + x;
            _spawnPointsPositions[x] = createdSpawnPoint.transform.position;
        }

        Destroy(spawnPoint);
    }

    private void ResizeBoardAccordingToScreenSize()
    {
        gameObject.transform.localScale *= _cellsOffsetMultiplier;
    }
}