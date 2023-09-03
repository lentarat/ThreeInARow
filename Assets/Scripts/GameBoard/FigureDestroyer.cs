using System.Collections.Generic;
using UnityEngine;

public class FigureDestroyer : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Grid _grid;

    public static event System.Action OnFigureDestroyed;

    private List<Figure> _figuresToDestroy = new List<Figure>();

    private void OnEnable()
    {
        FigureSwapper.OnFiguresSwapped += HandleFiguresMoved;
        Gravity.OnFiguresFellDown += HandleFiguresMoved;
        //Grid.OnGridReady += HandleFiguresMoved;
    }

    private void OnDisable()
    {
        FigureSwapper.OnFiguresSwapped -= HandleFiguresMoved;
        Gravity.OnFiguresFellDown -= HandleFiguresMoved;
        //Grid.OnGridReady -= HandleFiguresMoved;
    }

    private void HandleFiguresMoved()
    {
        AddFiguresToDestroyList();
    }

    private void AddFiguresToDestroyList()
    {
        int xMax = _grid.XDim;
        int yMax = _grid.YDim;

        _figuresToDestroy.Clear();

        for (int x = 0; x < xMax; x++)
        {
            for (int y = 0; y < yMax; y++)
            {
                if (IsOccupiedByFigure(x, y))
                {
                    AddToListThreeOrMoreFiguresConsistent(x, y);
                }
            }
        }

        if (_figuresToDestroy.Count > 0)
        {
            RemoveFiguresInArray();

            DestroyFigures();

            OnFigureDestroyed?.Invoke();
        }
    }

    private bool IsOccupiedByFigure(int x, int y)
    {
        return _grid.Figures[x, y] != null;
    }

    private void AddToListThreeOrMoreFiguresConsistent(int x, int y)
    {
        int inARow = 1;

        int xMax = _grid.XDim - 1;

        for (int i = x; i < xMax; i++)
        {
            Figure figureOnTheRight = _grid.Figures[i + 1, y];

            if (figureOnTheRight == null || _grid.Figures[i, y].FigureType != figureOnTheRight.FigureType)
            {
                break;
            }
            else
            {
                inARow++;
            }
        }
        if (inARow >= 3)
        {
            for (int j = 0; j < inARow; j++)
            {
                Figure figure = _grid.Figures[x + j, y];
                if (_figuresToDestroy.Contains(figure) == false)
                {
                    _figuresToDestroy.Add(figure);
                }
                //_grid.Figures[x + j, y].ToBeDestroyed = true;
            }
        }

        inARow = 1;

        int yMax = _grid.YDim - 1;

        for (int i = y; i < yMax; i++)
        {
            Figure figureFromAbove = _grid.Figures[x, i + 1];

            if (figureFromAbove == null || _grid.Figures[x, i].FigureType != figureFromAbove.FigureType)
            {
                break;
            }
            else
            {
                inARow++;
            }
        }

        if (inARow >= 3)
        {
            for (int j = 0; j < inARow; j++)
            {
                Figure figure = _grid.Figures[x, y + j];
                if (_figuresToDestroy.Contains(figure) == false)
                {
                    _figuresToDestroy.Add(figure);
                }
                //_grid.Figures[x, y + j].ToBeDestroyed = true;
            }
        }
    }

    private void DestroyFigures()
    {
        foreach(Figure figureToBeDestroyed in _figuresToDestroy)
        {
            Destroy(figureToBeDestroyed.gameObject);
        }
    }
    private void RemoveFiguresInArray()
    {
        foreach (Figure figureToBeDestroyed in _figuresToDestroy)
        {
            int x = (int)figureToBeDestroyed.ArrayIndex.x;
            int y = (int)figureToBeDestroyed.ArrayIndex.y;

            _grid.Figures[x, y] = null;
        }
    }
}
