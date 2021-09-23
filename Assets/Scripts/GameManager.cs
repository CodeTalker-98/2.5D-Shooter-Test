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
    private int _highScore;
    private bool _hardMode;

    private void Awake()
    {
        _instance = this;
    }

    public void StartGame()
    {
        //Load scene async
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
