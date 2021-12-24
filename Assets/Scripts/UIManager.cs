using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private Player _player;                             //Player variable
    private Slider _healthBar;                          //Slider Varible
    private Image _currentWeaponImage;                  //Current Displayed Image
    [SerializeField] private Image[] _weaponImage;      //Make an array to show current weapon
    [SerializeField] private Text _scoreText;           //Score text to display score
    [SerializeField] private GameObject _pausedPanel;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private GameObject _levelCompletePanel;
    private int _playerMaxHealth = 5;                   // Sets Max player Health

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();         //Find's Player
        _healthBar = GetComponentInChildren<Slider>();                      //Find's healthbar
        _healthBar.maxValue = _playerMaxHealth;                             //Healthbar Max is equal to Max Health

        if (GameManager.Instance == null)
        {
            Debug.LogError("UI MANAGER cannot find GAME MANAGER");
        }
    }

    private void Update()
    {
        if (_player != null)                                                //If player exists
        {
            int score = _player.SendPlayerScore();                          //Call method to grab score
            _scoreText.text = score.ToString().PadLeft(6, '0');             //Update score to look retro

            //return info gotten from weapon that is being used in weapon class 
        }
    }

    public void UpdateHealthBar(int value)
    {
        _healthBar.value = value;
    }

    public void NextLevel()
    {
        _levelCompletePanel.SetActive(true);
    }

    public void Continue()
    {
        GameManager.Instance.ContinueGame();
        _levelCompletePanel.SetActive(false);
    }

    public void PlayerDeath()
    {
        _gameOverPanel.SetActive(true);
    }

    public void Retry()
    {
        GameManager.Instance.RetryLevel();
        _gameOverPanel.SetActive(false);
    }

    public void PausedGame()
    {
        _pausedPanel.SetActive(true);
    }

    public void Resume()
    {
        GameManager.Instance.Resume();
        _pausedPanel.SetActive(false);
    }

    public void Quit()
    {
        GameManager.Instance.QuitGame();
    }
}
