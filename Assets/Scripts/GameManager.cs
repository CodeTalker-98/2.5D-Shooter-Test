﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("The GameManager is null");
            }
            return _instance;
        }
    }

    [Header("Main Menu")]
    [SerializeField] private GameObject _startPanel;
    [SerializeField] private GameObject _optionsPanel;
    [SerializeField] private Toggle _hardModeToggle;
    [SerializeField] private Slider _brightnessSlider;

    private int _highScore;
    private bool _hardMode;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        _hardModeToggle.isOn = false;
        _brightnessSlider.value = 0.5f;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(2);
    }

    public void OptionsMenu()
    {
        _startPanel.SetActive(false);
        _optionsPanel.SetActive(true);
    }

    public void BackButton()
    {
        _optionsPanel.SetActive(false);
        _startPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
