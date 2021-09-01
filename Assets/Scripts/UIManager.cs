using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private Player _player;                             //Player variable
    private Slider _healthBar;                          //Slider Varible
    [SerializeField] private Image _weaponImage;        //Make an array to show current weapon
    [SerializeField] private Text _scoreText;           //Score text to display score

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();         //Find's Player
        _healthBar = GetComponentInChildren<Slider>();                      //Find's healthbar
        //Score text to 0
    }

    private void Update()
    {
        //if player exists
        //update stats and Img
    }
}
