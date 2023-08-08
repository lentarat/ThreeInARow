using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FigureSwapper : MonoBehaviour
{
    [SerializeField] private InputHandler _inputHandler;

    [SerializeField] private Grid _grid;

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
        if (LastTouchWasOnFigure() && !_areFiguresSwapping)
        {
            Debug.Log(mouseDelta.normalized);

            //_touchedFigure = null;
            //_areFiguresSwapping = true;

            //SwapFigures(_touchedFigureArrayIndex, mouseDelta);
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

    public void SwapFigures(Vector2 chosenFigureArrayIndex, Vector2 direction)
    {
        direction = Vector2.up;
        Vector2 figuredToBeSwappedIndex = new Vector2(chosenFigureArrayIndex.x + direction.x, chosenFigureArrayIndex.y + direction.y);

        GameObject figureChosen = _grid.Figures[(int)chosenFigureArrayIndex.x, (int)chosenFigureArrayIndex.y].Prefab;
        GameObject figureToBeSwapped = _grid.Figures[(int)figuredToBeSwappedIndex.x, (int)figuredToBeSwappedIndex.y].Prefab;

        StartCoroutine(SwapTheFigures(figureChosen, figureToBeSwapped, chosenFigureArrayIndex, figuredToBeSwappedIndex));

        OnFiguresSwapped?.Invoke();
    }

    private IEnumerator SwapTheFigures(GameObject figureChosen, GameObject figureToBeSwapped, Vector2 chosenFigureArrayIndex, Vector2 figuredToBeSwappedIndex)
    {
        Vector3 figureChosenPosition = figureChosen.transform.position;
        Vector3 figureToBeSwappedPosition = figureToBeSwapped.transform.position;

        float blendValue = 0f;

        while (figureChosen.transform.position != figureToBeSwappedPosition)
        {
            figureChosen.transform.position = Vector3.Lerp(figureChosenPosition, figureToBeSwappedPosition, blendValue * 0.5f);
            figureToBeSwapped.transform.position = Vector3.Lerp(figureToBeSwappedPosition, figureChosenPosition, blendValue * 0.5f);

            blendValue += Time.deltaTime;

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
}
