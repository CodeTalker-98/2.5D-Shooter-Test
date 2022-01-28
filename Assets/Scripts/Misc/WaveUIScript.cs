using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveUIScript : MonoBehaviour
{
    private int _waveNumber = 0;
    private Text _uiText;

    private void Start()
    {
        _uiText = GetComponent<Text>();
    }

    private void Update()
    {
        _uiText.text = "Wave: " + _waveNumber.ToString();
    }

    private void OnEnable()
    {
        _waveNumber++;

        if(_waveNumber == 9)
        {
            GameManager.Instance.SetWaveNumber(_waveNumber);
        }
    }

    public void LoadWave()
    {
        if (GameManager.Instance.ReachedCheckpoint())
        {
            _waveNumber = 9;
        }
    }

}
