using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsManager : MonoBehaviour
{
    private LevelsManager() { }

    private static LevelsManager _instance;
    public static LevelsManager Instance
    {
        get => _instance;
        private set => _instance = value;
    }

    [SerializeField] private Level[] _levels;

    private int _currentLevelNumber;

    private void Awake()
    {
        Instance = this;
    }

    public Level GetCurrentLevel()
    {
        return _levels[_currentLevelNumber];
    }

    public void NextLevel()
    {
        _currentLevelNumber++;
    }
}

[System.Serializable]
public struct Level
{
    [Header("Figures Amount By")]
    [SerializeField] private int _width;
    public int Width { get; }
    [SerializeField] private int _height;
    public int Height { get; }
}

