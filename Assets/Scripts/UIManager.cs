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
    private int _playerMaxHealth = 5;                   // Sets Max player Health

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();         //Find's Player
        _healthBar = GetComponentInChildren<Slider>();                      //Find's healthbar
        _healthBar.maxValue = _playerMaxHealth;                             //Healthbar Max is equal to Max Health
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
}
