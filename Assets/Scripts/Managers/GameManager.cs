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

    private void Awake()
    {
        Instance = this;

        LoadFirstLevel();
    }
    private void LoadFirstLevel()
    {
        
    }

    private void Update()
    {
        _timeLeft = _roundTime - Time.time;
    }

    private void LoadFirst()
    {
        
    }

    public float GetTimeLeft()
    {
        return _timeLeft;
    }
}
