using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamagable
{
    [SerializeField] private float _spd = 5.0f;                 // Player Movement Speed
    [SerializeField] private float _fireRate = 1.0f;            //Fire Rate of weapon
    [SerializeField] private Transform _firingPosition;
    private float _cycleTime = 0.0f;                            //Cycle time for weapon
    private Vector3 _velocityChange;
    private Vector3 _velocity;                                  //Velocity for player
    [SerializeField] private GameObject _bullet;                //Bullet Player Shoots
    private int _score = 0;                                     //Current score
    private UIManager _uiManager;                               //Comm with UI Manager
    private bool _isPaused = false;
    [SerializeField] private bool _isDead;
    private int _collisionDamage = 1;                               //For collsions
    public int Health { get; set; }
    
    private void Init()
    {
        _uiManager = GameObject.Find("UI").GetComponentInChildren<UIManager>();
        
        Health = 5;
        if (_uiManager != null)
        {
            _uiManager.UpdateHealthBar(Health);
        }
        
    }
    private void Start()
    {
        Init();
    }

    private void Update()
    {
        PlayerMovement();                                       // Move player
        PlayerShoot();                                          //Player Shoot

        if (Input.GetKeyDown(KeyCode.Escape) && !_isPaused)
        {
            _uiManager.PausedGame();
            GameManager.Instance.Pause();
            _isPaused = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && _isPaused)
        {
            GameManager.Instance.Resume();
            _isPaused = false;
        }

        //////////////////////TESTING FIX FOR END OF LEVEL/////////////////////////
        if (Input.GetKeyDown(KeyCode.Q))
        {
            _uiManager.NextLevel();
        }
        //////////////////////TESTING FIX FOR END OF LEVEL/////////////////////////
    }

    void PlayerMovement()
    {
        float hInput = Input.GetAxisRaw("Horizontal");          //Get input and use it to set direction
        float vInput = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(hInput, vInput, 0.0f).normalized;
        _velocityChange = new Vector3(Mathf.Abs(hInput), 0.0f, 0.0f) * 1.25f;

        _velocity = direction * _spd + _velocityChange;      //Create velocity
        transform.Translate(_velocity * Time.deltaTime);     //Move player
        
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
                    Instantiate(_bullet, _firingPosition.position, Quaternion.identity);
                }
                else
                {
                    Debug.LogError("The BULLET is null");
                }
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
            if(Health < 5)
            {
                Health++;                                       // Add 1 to health if current health is less than 5
            }
            _uiManager.UpdateHealthBar(Health);                 //Update healthbar with current health
            Debug.Log("Current Health: " + Health);             //Display message to show it works
            Destroy(other.gameObject);                                  //Destroy the powerup
        }

        if(other.tag == "Enemy")
        {
            Enemy enemy = other.GetComponent<Enemy>();
            
            if(enemy != null)
            {
                TakeDamage(enemy.CollisionDamage());
            }
            
            IDamagable hit = other.GetComponent<IDamagable>();
            
            if (hit != null)
            {
                hit.TakeDamage(_collisionDamage);
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (_isDead)
        {
            return;
        }

        Health -= damage;
        _uiManager.UpdateHealthBar(Health);

        if (Health < 1)
        {
            _isDead = true;
            //play anim
            _uiManager.PlayerDeath();                                                                       //Updates the Health bar on the HUD
            Destroy(this.gameObject);
            //SpawnManager spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();       //Find spawn manager communicate with it
            /*if (spawnManager != null)                                                                       //If spawn manager exists
            {
                spawnManager.OnPlayerDeath();                                                               //Call method to stop spawning
                                                                                 //Destroy ourselves
            }*/
        }
    }
}