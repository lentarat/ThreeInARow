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
                    Figure spawnedFigureAboveTheGrid = SpawnAFigureAboveTheGridAtPosition(_grid.GetSpawnPointPosition(x));
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
            StartCoroutine(ApplyGravityToFigures());
        }

        GameManager.Instance.CurrentGameState = GameManager.GameState.Idle;
    }

    private bool IsOccupiedByFigure(int x, int y)
    {
        return _grid.Figures[x, y] != null;
    }

    //returns true if figures were created while adding gravitation to upper figures
    private bool AddFiguresWhichAreAboveToFallList(int x, int y, int yMax)
    {
        bool wereFiguresCreated = false;

        float figureAboveTheGridOffset = _cellOffset;

        Debug.Log(Time.time);
        
        for (int yIndex = y + 1; yIndex <= yMax; yIndex++)
        {
            if (_grid.Figures[x, yIndex] == null)
            {
                Figure spawnedFigureAboveTheGrid = SpawnAFigureAboveTheGridAtPosition(_grid.GetSpawnPointPosition(x) + new Vector3(0f, figureAboveTheGridOffset, 0f));
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

    private Figure SpawnAFigureAboveTheGridAtPosition(Vector2 position)
    {
        return _figureSpawner.SpawnAFigureAtPosition(position, Vector2.zero);
    }

    private IEnumerator ApplyGravityToFigures()
    {
        float pathPassed = 0;

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
}
