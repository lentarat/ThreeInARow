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

    [Header("Common")]
    [SerializeField] private float _roundTime;

    private float _timeLeft;

    public enum GameState
    {
        Idle,
        FiguresSwapping,
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

    private void Update()
    {
        _timeLeft = _roundTime - Time.time;
    }

    

    public float GetTimeLeft()
    {
        return _timeLeft;
    }
}
