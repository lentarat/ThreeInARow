using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiguresManager : MonoBehaviour
{
    public event System.Action OnFiguresSwapped;
    public event System.Action OnFiguresCollapsed;

    [SerializeField] private Grid _grid;

    public void MoveFigure(Vector2 chosenFigureArrayIndex, Vector2 direction)
    {
        Vector2 figuredToBeSwappedIndex = new Vector2 (chosenFigureArrayIndex.x + direction.x, chosenFigureArrayIndex.y + direction.y);

        GameObject figureChosen = _grid.Figures[(int)chosenFigureArrayIndex.x, (int)chosenFigureArrayIndex.y];
        GameObject figureToBeSwapped = _grid.Figures[(int)figuredToBeSwappedIndex.x, (int)figuredToBeSwappedIndex.y];

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
        
        yield return null;
    }

    private void SwapTheFiguresInArray(Vector2 chosenFigureArrayIndex, Vector2 figuredToBeSwappedIndex)
    {
        GameObject temporary = _grid.Figures[(int)chosenFigureArrayIndex.x, (int)chosenFigureArrayIndex.y];
        _grid.Figures[(int)chosenFigureArrayIndex.x, (int)chosenFigureArrayIndex.y] = _grid.Figures[(int)figuredToBeSwappedIndex.x, (int)figuredToBeSwappedIndex.y];
        _grid.Figures[(int)figuredToBeSwappedIndex.x, (int)figuredToBeSwappedIndex.y] = temporary;
    }
}
