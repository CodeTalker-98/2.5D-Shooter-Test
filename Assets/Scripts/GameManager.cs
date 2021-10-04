using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if(_instance == null)
            {
                Debug.LogError("The GameManager is null");
            }
            return _instance;
        }
    }

    [SerializeField] private GameObject _startPanel;
    [SerializeField] private GameObject _optionsPanel;
    //_playSFX Bool
    //_playMusic Bool
    //Brightness slider
    //Sound Slider
    private int _highScore;
    private bool _hardMode;

    private void Awake()
    {
        _instance = this;
    }

    public void SayHi()
    {
        Debug.Log("Hi! I'm the Game Manager");
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
