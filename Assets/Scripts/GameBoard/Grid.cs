using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    private enum FigureType
    {
        Octagon,
        Pentagon,
        Triangle,
        Quad,
        Circle,
        Count
    }

    [System.Serializable]
    private struct FigurePrefab
    {
        public FigureType Type;
        public GameObject Prefab;
    }

    [Header("Grid Size")]
    [SerializeField] private int _xDim;
    [SerializeField] private int _yDim;

    private Dictionary<FigureType, GameObject> _piecePrefabDictionary = new Dictionary<FigureType, GameObject>();
    [Header("Figure & Background Cell Prefabs")]
    [SerializeField] private FigurePrefab[] _figurePrefabs;
    [SerializeField] private GameObject _backgroundCellPrefab;

    private Figure[,] _figures;
    public Figure[,] Figures
    {
        get => _figures;
    }
    private Vector2 _centeredGridInWorldPosition;

    private void Awake()
    {
        CenterTheGrid();
        CopyFigurePrefabsArrayToDictionary();
        InitializeGridBackgroundCells();
        InitializeFigures();
    }

    private void CenterTheGrid()
    {
        _centeredGridInWorldPosition = new Vector2(transform.position.x - _xDim / 2f + 0.5f, transform.position.y - _yDim / 2f + 0.5f);
    }

    private void CopyFigurePrefabsArrayToDictionary()
    {
        for (int i = 0; i < _figurePrefabs.Length; i++)
        {
            if (_piecePrefabDictionary.ContainsKey(_figurePrefabs[i].Type) == false)
            {
                _piecePrefabDictionary.Add(_figurePrefabs[i].Type, _figurePrefabs[i].Prefab);
            }
        }
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
        _figures = new Figure[_xDim, _yDim];

        for (int x = 0; x < _xDim; x++)
        {
            for (int y = 0; y < _yDim; y++)
            {
                Vector2 figureOffset = new Vector3(x, y);

                GameObject randomFigureToBeInstantiated;
                _piecePrefabDictionary.TryGetValue(GetRandomFigureType(), out randomFigureToBeInstantiated);

                GameObject instantiatedFigure = Instantiate(randomFigureToBeInstantiated, _centeredGridInWorldPosition + figureOffset, Quaternion.identity, transform);

                instantiatedFigure.name = $"Figure at x:{x} y:{y}";

                Figure figure = instantiatedFigure.GetComponent<Figure>();

                figure.Prefab = instantiatedFigure;
                figure.ArrayIndex = new Vector2(x, y);

                _figures[x, y] = figure;
            }
        }
    }

    private FigureType GetRandomFigureType()
    {
        int randomFigureTypeNumber = Random.Range(0, (int)FigureType.Count);
        return (FigureType)randomFigureTypeNumber;
    }
}






//using System.Collections.Generic;
//using UnityEngine;

//public class Grid : MonoBehaviour
//{
//    private enum FigureType
//    {
//        Octagon,
//        Pentagon,
//        Triangle,
//        Quad,
//        Circle,
//        Count
//    }

//    [System.Serializable]
//    private struct FigurePrefab
//    {
//        public FigureType Type;
//        public GameObject Prefab;
//    }

//    [Header("Grid Size")]
//    [SerializeField] private int _xDim;
//    [SerializeField] private int _yDim;

//    private Dictionary<FigureType, GameObject> _piecePrefabDictionary = new Dictionary<FigureType, GameObject>();
//    [Header("Figure & Background Cell Prefabs")]
//    [SerializeField] private FigurePrefab[] _figurePrefabs;
//    [SerializeField] private GameObject _backgroundCellPrefab;

//    private GameObject[,] _figures;
//    public GameObject[,] Figures 
//    {
//        get => _figures;
//        //set => _figures = value;
//    }
//    private Vector2 _centeredGridInWorldPosition;

//    private void Awake()
//    {
//        CenterTheGrid();
//        CopyFigureArrayToDictionary();
//        InitializeGridBackgroundCells();
//        InitializeFigures();
//    }

//    private void CenterTheGrid()
//    {
//        _centeredGridInWorldPosition = new Vector2(transform.position.x - _xDim / 2f + 0.5f, transform.position.y - _yDim / 2f + 0.5f);
//    }

//    private void CopyFigureArrayToDictionary() 
//    {
//        for (int i = 0; i < _figurePrefabs.Length; i++)
//        {
//            if (_piecePrefabDictionary.ContainsKey(_figurePrefabs[i].Type) == false)
//            {
//                _piecePrefabDictionary.Add(_figurePrefabs[i].Type, _figurePrefabs[i].Prefab);
//            }
//        }
//    }

//    private void InitializeGridBackgroundCells()
//    {
//        for (int x = 0; x < _xDim; x++)
//        {
//            for (int y = 0; y < _yDim; y++)
//            {
//                Vector2 backgroundCellOffset = new Vector3(x, y);
//                Instantiate(_backgroundCellPrefab, _centeredGridInWorldPosition + backgroundCellOffset, Quaternion.identity, transform);
//            }
//        }
//    }

//    private void InitializeFigures()
//    {
//        _figures = new GameObject[_xDim, _yDim];

//        for (int x = 0; x < _xDim; x++)
//        {
//            for (int y = 0; y < _yDim; y++)
//            {
//                Vector2 figureOffset = new Vector3(x, y);

//                GameObject randomFigureToBeInstantiated;
//                _piecePrefabDictionary.TryGetValue( GetRandomFigureType(), out randomFigureToBeInstantiated);

//                _figures [x, y] = Instantiate(randomFigureToBeInstantiated, _centeredGridInWorldPosition + figureOffset, Quaternion.identity, transform);
//                _figures[x, y].name = $"Figure at x:{x} y:{y}"; 
//            }
//        }
//    }

//    private FigureType GetRandomFigureType()
//    {
//        int randomFigureTypeNumber = Random.Range(0, (int)FigureType.Count);
//        return (FigureType)randomFigureTypeNumber;
//    }
//}
