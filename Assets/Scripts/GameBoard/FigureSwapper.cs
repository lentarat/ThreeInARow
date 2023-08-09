using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FigureSwapper : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private InputHandler _inputHandler;
    [SerializeField] private Grid _grid;

    [Header("Common")]
    [SerializeField] private float _swapSpeed;

    public event System.Action OnFiguresSwapped;

    private Figure _touchedFigure;
    private Vector2 _touchedFigureArrayIndex;

    private bool _areFiguresSwapping;

    private void OnEnable()
    {
        _inputHandler.OnTouch += OnTouchHandler;
        _inputHandler.OnDelta += OnDeltaHandler;
    }

    private void OnDisable()
    {
        _inputHandler.OnTouch -= OnTouchHandler;
        _inputHandler.OnDelta += OnDeltaHandler;
    }

    private void OnTouchHandler(Vector2 mousePosition)
    {
        if (IsRaycastOnFigure(mousePosition))
        {
            _touchedFigureArrayIndex = GetFigureArrayIndex();
        }
        else
        {
            _touchedFigure = null;
        }
    }

    private bool IsRaycastOnFigure(Vector2 mousePosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            if (raycastHit.collider.TryGetComponent<Figure>(out _touchedFigure))
            {
                return true;
            }
        }

        return false;
    }

    private Vector2 GetFigureArrayIndex()
    {
        return _touchedFigure.ArrayIndex;
    }

    private void OnDeltaHandler(Vector2 mouseDelta)
    {
        Vector2 figureOffsetToBeSwappedIndex = GetFigureOffsetToBeSwappedArrayIndex(mouseDelta);

        if (LastTouchWasOnFigure() && FitsInArrayBoundaries(figureOffsetToBeSwappedIndex) && !_areFiguresSwapping)
        {
            _touchedFigure = null;
            _areFiguresSwapping = true;

            SwapFigures(_touchedFigureArrayIndex, figureOffsetToBeSwappedIndex);
        }
    }

    private Vector2 GetFigureOffsetToBeSwappedArrayIndex(Vector2 mouseDelta)
    {
        if (mouseDelta.x > 0f)
        {
            return Vector2.right;
        }
        else if (mouseDelta.x < 0f)
        {
            return Vector2.left;
        }
        else if (mouseDelta.y > 0f)
        {
            return Vector2.up;
        }
        else if (mouseDelta.y < 0f)
        {
            return Vector2.down;
        }
        else
        {
            return Vector2.zero;
        }
    }

    private bool LastTouchWasOnFigure()
    {
        if (_touchedFigure != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool FitsInArrayBoundaries(Vector2 pullDirection)
    {
        if (pullDirection.x > 0f && _touchedFigureArrayIndex.x >= _grid.Figures.GetUpperBound(0))
        {
            return false;
        }
        else if (pullDirection.x < 0f && _touchedFigureArrayIndex.x <= 0)
        {
            return false;
        }
        else if (pullDirection.y > 0f && _touchedFigureArrayIndex.y >= _grid.Figures.GetUpperBound(1))
        {
            return false;
        }
        else if (pullDirection.y < 0f && _touchedFigureArrayIndex.y <= 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void SwapFigures(Vector2 chosenFigureArrayIndex, Vector2 figureOffsetToBeSwappedIndex)
    {
        Vector2 figureToBeSwappedIndex = new Vector2(chosenFigureArrayIndex.x + figureOffsetToBeSwappedIndex.x, chosenFigureArrayIndex.y + figureOffsetToBeSwappedIndex.y);

        GameObject figureChosen = _grid.Figures[(int)chosenFigureArrayIndex.x, (int)chosenFigureArrayIndex.y].Prefab;
        GameObject figureToBeSwapped = _grid.Figures[(int)figureToBeSwappedIndex.x, (int)figureToBeSwappedIndex.y].Prefab;

        StartCoroutine(SwapTheFigures(figureChosen, figureToBeSwapped, chosenFigureArrayIndex, figureToBeSwappedIndex));

        OnFiguresSwapped?.Invoke();
    }

    private IEnumerator SwapTheFigures(GameObject figureChosen, GameObject figureToBeSwapped, Vector2 chosenFigureArrayIndex, Vector2 figuredToBeSwappedIndex)
    {
        Vector3 figureChosenPosition = figureChosen.transform.position;
        Vector3 figureToBeSwappedPosition = figureToBeSwapped.transform.position;

        float blendValue = 0f;

        while (figureChosen.transform.position != figureToBeSwappedPosition)
        {
            figureChosen.transform.position = Vector3.Lerp(figureChosenPosition, figureToBeSwappedPosition, blendValue);
            figureToBeSwapped.transform.position = Vector3.Lerp(figureToBeSwappedPosition, figureChosenPosition, blendValue);

            blendValue += _swapSpeed * Time.deltaTime;

            yield return null;
        }

        SwapTheFiguresInArray(chosenFigureArrayIndex, figuredToBeSwappedIndex);

        _areFiguresSwapping = false;

        yield return null;
    }

    private void SwapTheFiguresInArray(Vector2 chosenFigureArrayIndex, Vector2 figuredToBeSwappedIndex)
    {
        Figure tempFigure = _grid.Figures[(int)chosenFigureArrayIndex.x, (int)chosenFigureArrayIndex.y];
        _grid.Figures[(int)chosenFigureArrayIndex.x, (int)chosenFigureArrayIndex.y] = _grid.Figures[(int)figuredToBeSwappedIndex.x, (int)figuredToBeSwappedIndex.y];
        _grid.Figures[(int)figuredToBeSwappedIndex.x, (int)figuredToBeSwappedIndex.y] = tempFigure;

        Vector2 tempArrayIndex = _grid.Figures[(int)chosenFigureArrayIndex.x, (int)chosenFigureArrayIndex.y].ArrayIndex;
        _grid.Figures[(int)chosenFigureArrayIndex.x, (int)chosenFigureArrayIndex.y].ArrayIndex = _grid.Figures[(int)figuredToBeSwappedIndex.x, (int)figuredToBeSwappedIndex.y].ArrayIndex;
        _grid.Figures[(int)figuredToBeSwappedIndex.x, (int)figuredToBeSwappedIndex.y].ArrayIndex = tempArrayIndex;
    }


    //private Vector2 GetPullDirection(Vector2 mouseDelta)
    //{
    //    float cellOffset = _grid.GetCellsOffset();

    //    if (mouseDelta.x > 0f)
    //    {
    //        return new Vector2(cellOffset, 0f);
    //    }
    //    else if (mouseDelta.x < 0f)
    //    {
    //        return new Vector2(-cellOffset, 0f);
    //    }
    //    else if (mouseDelta.y > 0f)
    //    {
    //        return new Vector2(0f, cellOffset);
    //    }
    //    else if (mouseDelta.y < 0f)
    //    {
    //        return new Vector2(0f, -cellOffset);
    //    }
    //    else
    //    {
    //        return Vector2.zero;
    //    }
    //}
}
