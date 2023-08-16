using System.Collections.Generic;
using UnityEngine;

public class FigureDestroyer : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private FigureSwapper _figureSwapper;
    [SerializeField] private Grid _grid;

    public event System.Action OnFigureDestroyed;

    private void Awake()
    {
        _figureSwapper.OnFiguresSwapped += HandleFiguresSwapped;    
    }

    private void HandleFiguresSwapped()
    {
        DestroyFiguresIfThreeOrMoreInARow();
    }

    private void DestroyFiguresIfThreeOrMoreInARow()
    {
        List<Figure> _figuresInARowToDestroy = new List<Figure>();

        for (int x = 0; x <= _grid.Figures.GetUpperBound(0); x++)
        {
            for (int y = 0; y <= _grid.Figures.GetUpperBound(1); y++)
            {
                if (IsEnoughToDestroy(x, y, _figuresInARowToDestroy))
                {
                    foreach (Figure figure in _figuresInARowToDestroy)
                    {
                        if (figure != null)
                        {
                            Destroy(figure.gameObject);
                            Vector2 figureArrayIndex = figure.ArrayIndex;
                            _grid.Figures[(int)figureArrayIndex.x, (int)figureArrayIndex.y] = null;
                        }
                    }

                    OnFigureDestroyed?.Invoke();

                    return;
                }
            }
        }
    }

    private bool IsEnoughToDestroy(int x, int y, List<Figure> _figuresInARowToDestroy)
    {
        int inARow = 1;

        int xMax = _grid.Figures.GetUpperBound(0);

        for (int i = x; i < xMax; i++)
        {
            if (_grid.Figures[i, y].FigureType == _grid.Figures[i + 1, y].FigureType)
            {
                inARow++;
            }
            else
            {
                break;
            }
        }
        if (inARow >= 3)
        {
            for (int j = 0; j < inARow; j++)
            {
                _figuresInARowToDestroy.Add(_grid.Figures[x + j, y]);
            }
            return true;
        }

        inARow = 1;

        int yMax = _grid.Figures.GetUpperBound(1);

        for (int i = y; i < yMax; i++)
        {
            if (_grid.Figures[x, i].FigureType == _grid.Figures[x, i + 1].FigureType)
            {
                inARow++;
            }
            else
            {
                break;
            }
        }

        if (inARow >= 3)
        {
            for (int j = 0; j < inARow; j++)
            {
                _figuresInARowToDestroy.Add(_grid.Figures[x, y + j]);
            }
            return true;
        }

        return false;
    }
}
