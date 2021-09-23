using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _spd = 5.0f;                 // Player Movement Speed
    [SerializeField] private float _fireRate = 1.0f;            //Fire Rate of weapon
    private float _cycleTime = 0.0f;                            //Cycle time for weapon
    private Vector3 _velocity;                                  //Velocity for player
    [SerializeField] private GameObject _bullet;                //Bullet Player Shoots
    [SerializeField] private int _currentHealth = 1;            //Current health value
    private int _score = 0;                                     //Current score
    private UIManager _uiManager;                               //Comm with UI Manager

    private void Start()
    {
        _uiManager = GameObject.Find("UI").GetComponentInChildren<UIManager>();

        if (_uiManager != null)
        {
            _uiManager.UpdateHealthBar(_currentHealth);
        }
    }

    private void Update()
    {
        PlayerMovement();                                       // Move player
        PlayerShoot();                                          //Player Shoot
    }

    void PlayerMovement()
    {
        float hInput = Input.GetAxisRaw("Horizontal");          //Get input and use it to set direction
        float vInput = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(hInput, vInput, 0.0f).normalized;

        _velocity = direction * _spd;                           //Create velocity
        transform.Translate(_velocity * Time.deltaTime);        //Move player

        float xClamp = Mathf.Clamp(transform.position.x, -8.75f, 8.75f);    //Resrict x and y movement
        float yClamp = Mathf.Clamp(transform.position.y, -3.5f, 5.5f);
        transform.position = new Vector3(xClamp, yClamp, 0.0f);             //Ensure position does not exceed restrictions
    }

    void PlayerShoot()
    {
        if (Input.GetKeyDown(KeyCode.Space))                    //Check for space bar
        {
            if (Time.time > _cycleTime)                         //if game time is greater than weapon cycle time
            {
                _cycleTime = Time.time + _fireRate;             //Set next cycle time
                
                if (_bullet != null)                            //Fire bullet if it exists
                {
                    Instantiate(_bullet, transform.position, Quaternion.identity);
                }
                else
                {
                    Debug.LogError("The BULLET is null");
                }
            }
        }
    }
    public void DamagePlayer(int damageAmount)
    {
        _currentHealth -= damageAmount;                                                                     //Deals enemy specific damage
        _uiManager.UpdateHealthBar(_currentHealth);                                                         //Updates the Health bar on the HUD

        if (_currentHealth < 1)                                                                             //If we are dead ( health < 1)
        {
            //Animate Death
            SpawnManager spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();       //Find spawn manager communicate with it
            if (spawnManager != null)                                                                       //If spawn manager exists
            {
                spawnManager.OnPlayerDeath();                                                               //Call method to stop spawning
                Destroy(this.gameObject);                                                                   //Destroy ourselves
            }
        }
    }

    public void UpdateScore(int scoreValue)
    {
        _score += scoreValue;
    }

    public int SendPlayerScore()
    {
        return _score;
    }

    private void OnTriggerEnter(Collider other)                         //Use to check for trigger collisions
    {
        if (other.tag == "Powerup")                                     //If we collide with powerup
        {
            if(_currentHealth < 5)
            {
                _currentHealth++;                                       // Add 1 to health if current health is less than 5
            }
            _uiManager.UpdateHealthBar(_currentHealth);                 //Update healthbar with current health
            Debug.Log("Current Health: " + _currentHealth);             //Display message to show it works
            Destroy(other.gameObject);                                  //Destroy the powerup
        }
    }
}
