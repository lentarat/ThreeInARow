using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    private enum FigureType
    {
        Normal,
        Count
    }

    [System.Serializable]
    private struct FigurePrefab
    {
        private FigureType type;
        private GameObject prefab;
    }

    public 
}
