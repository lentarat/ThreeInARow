using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FigureSpawner : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private Grid _grid;

    [Header("Hierarchy Parent")]
    [SerializeField] Transform _figuresParent;

    public enum FigureType
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

    private Dictionary<FigureType, GameObject> _piecePrefabDictionary = new Dictionary<FigureType, GameObject>();

    [SerializeField] private FigurePrefab[] _figurePrefabs;

    //private Transform _gridTransform;
    //private float _cellsOffsetMultiplier;

    //private void OnEnable()
    //{
    //    Grid.OnGridReady += SetupFigureSpawner;
    //}

    private void Awake()
    {
        CopyFigurePrefabsArrayToDictionary();
    }

    public Figure SpawnAFigureAtPosition(Vector2 position, Vector2 centeredGridOffset)
    {
        GameObject randomFigureToBeInstantiated;
        FigureType randomFigureType = GetRandomFigureType();
        _piecePrefabDictionary.TryGetValue(randomFigureType, out randomFigureToBeInstantiated);

        Vector3 figureSpawnPosition = (position + centeredGridOffset);
        GameObject instantiatedFigureGameObject = Instantiate(randomFigureToBeInstantiated, figureSpawnPosition, Quaternion.identity, _figuresParent);

        Figure instantiatedFigure = instantiatedFigureGameObject.GetComponent<Figure>();

        instantiatedFigure.FigureType = randomFigureType;
        instantiatedFigure.ArrayIndex = new Vector2(position.x, position.y);

        return instantiatedFigure;
    }

    private FigureType GetRandomFigureType()
    {
        int randomFigureTypeNumber = Random.Range(0, (int)FigureType.Count);
        return (FigureType)randomFigureTypeNumber;
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
}
