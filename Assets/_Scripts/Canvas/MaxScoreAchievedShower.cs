using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxScoreAchievedShower : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI _maxScore;

    private void Awake()
    {
        int maxScore = SaveLoadSystem.GetInt(SaveLoadSystem.VariablesNameTypes.MaxScore);
        _maxScore.text = $"Best Score { maxScore }";
    }
}
