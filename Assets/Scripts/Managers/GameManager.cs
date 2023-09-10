using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameManager() { }

    private static GameManager _instance;
    public static GameManager Instance
    {
        get => _instance;
        private set => _instance = value;
    }

    public enum GameState
    {
        Idle,
        FiguresSwapping,
        FiguresDestroying,
        FiguresFalling
    }

    private GameState _currentGameState;
    public GameState CurrentGameState
    {
        get => _currentGameState;
        set => _currentGameState = value;
    }

    private void Awake()
    {
        Instance = this;
    }
}
