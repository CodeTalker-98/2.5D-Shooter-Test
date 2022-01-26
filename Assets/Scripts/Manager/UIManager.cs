using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private Player _player;                             //Player variable
    [SerializeField] private Image _healthBar;                          //Slider Varible
    [SerializeField] private Image _currentWeaponImage;                  //Current Displayed Image
    [SerializeField] private Image[] _weaponImage;      //Make an array to show current weapon
    [SerializeField] private Text _scoreText;           //Score text to display score
    [SerializeField] private GameObject _pausedPanel;
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private GameObject _levelCompletePanel;
    private float _playerMaxHealth;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();         //Find's Player


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
            _scoreText.text = "Score: " + score.ToString().PadLeft(6, '0');             //Update score to look retro

            //return info gotten from weapon that is being used in weapon class
            int imageIndex = _player.SendPlayerHealth() - 1;
            _currentWeaponImage.sprite = _weaponImage[imageIndex].sprite;
        }
    }

    public void UpdateHealthBar(float value)
    {
        if (_player == null)
        {
            _player = GameObject.Find("Player").GetComponent<Player>();
        }
        else
        {
            _playerMaxHealth = _player.PlayerMaxHealth();
        }
        _healthBar.fillAmount = value / _playerMaxHealth;
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

    public void PausedGame(bool isPaused)
    {
        _pausedPanel.SetActive(isPaused);
    }

    public void Resume()
    {
        GameManager.Instance.Resume();
        _player.PauseState();
        PausedGame(false);
    }

    public void Quit()
    {
        GameManager.Instance.QuitGame();
    }
}
