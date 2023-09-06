using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrentFPSShower : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;

    private void Update()
    {

        _text.text = Mathf.FloorToInt(1f / Time.deltaTime).ToString();
    }
}
