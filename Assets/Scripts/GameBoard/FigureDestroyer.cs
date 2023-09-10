using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FigureDestroyer : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Grid _grid;

    [Header("Common")]
    [SerializeField] private float _animationBeforeDestroyingDuration;
    
    public static event System.Action OnFigureDestroyed;

    private List<Figure> _figuresToDestroy = new List<Figure>();

    private void OnEnable()
    {
        FigureSwapper.OnFiguresSwapped += HandleFiguresMoved;
        Gravity.OnFiguresFellDown += HandleFiguresMoved;
    }

    private void OnDisable()
    {
        FigureSwapper.OnFiguresSwapped -= HandleFiguresMoved;
        Gravity.OnFiguresFellDown -= HandleFiguresMoved;
    }

    private void HandleFiguresMoved()
    {
        

        AddFiguresToDestroyList();

        if (_figuresToDestroy.Count > 0)
        {
            StartCoroutine(DestroyFigures());
        }

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
            }
        }
    }

    private IEnumerator DestroyFigures()
    {
        GameManager.Instance.CurrentGameState = GameManager.GameState.FiguresDestroying;

        ShowDeathAnimation();

        //float counter = 0f;

        //while (counter < _animationBeforeDestroyingDuration)
        //{
        //    counter += Time.deltaTime;

        //    Debug.Log("Destroyer " + GameManager.Instance.CurrentGameState);

        //    if (GameManager.Instance.CurrentGameState == GameManager.GameState.FiguresDestroying)
        //    {
            
        //    }

        //    yield return null;
        //}


        yield return new WaitForSeconds(_animationBeforeDestroyingDuration);

        RemoveFiguresInArray();
        DestroyEachFigure();

        GameManager.Instance.CurrentGameState = GameManager.GameState.Idle;

        OnFigureDestroyed?.Invoke();
    }

    private void ShowDeathAnimation()
    {
        foreach (Figure figureToBeDestroyedAfterAnimation in _figuresToDestroy)
        {
            figureToBeDestroyedAfterAnimation.transform.DOScale(0f, _animationBeforeDestroyingDuration);
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

    private void DestroyEachFigure()
    {
        foreach (Figure figureToBeDestroyed in _figuresToDestroy)
        {
            Destroy(figureToBeDestroyed.gameObject);
        }
    }
}
