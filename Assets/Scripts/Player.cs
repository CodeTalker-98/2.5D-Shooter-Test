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
    private int _maxHealth = 5;
    [SerializeField] private GameObject _deathPrefab;
    private string _currentState;
    private Animator _anim;
    [SerializeField] protected AudioClip _firingSoundReg;
    [SerializeField] protected AudioClip _firingSoundMinigun;
    protected AudioSource _audio;

    private const string FORWARD = "Player Forward";
    private const string BACKWARD = "Player Backward";
    private const string UP = "Player Up";
    private const string DOWN = "Player Down";
    private const string IDLE = "Player Idle";
    public int Health { get; set; }
    
    private void Init()
    {
        _uiManager = GameObject.Find("UI").GetComponentInChildren<UIManager>();
        _anim = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();

        Health = 5;
        Debug.Log("Called Start");


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

        switch (Health)
        {
            case 0:
                _fireRate = 1.0f;
                
                _audio.clip = _firingSoundReg;
                break;
            case 1:
                _fireRate = 1.0f;
                _audio.clip = _firingSoundReg;
                break;
            case 2:
                _fireRate = 1.0f;
                _audio.clip = _firingSoundReg;
                break;
            case 3:
                _fireRate = 0.5f;
                _audio.clip = _firingSoundReg;
                break;
            case 4:
                _fireRate = 0.5f;
                _audio.clip = _firingSoundReg;
                break;
            case 5:
                _fireRate = 0.125f;
                _audio.clip = _firingSoundMinigun;
                break;
            default:
                _fireRate = 1.0f;
                _audio.clip = _firingSoundReg;
                break;
        }

        Debug.Log("Health: " + Health);

        if (Input.GetKeyDown(KeyCode.Escape) && !_isPaused)
        {
            _uiManager.PausedGame(true);
            GameManager.Instance.Pause();
            _isPaused = true;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && _isPaused)
        {
            _uiManager.PausedGame(false);
            _uiManager.Resume();
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
        if (!_isPaused)
        {
            float hInput = Input.GetAxisRaw("Horizontal");          //Get input and use it to set direction
            float vInput = Input.GetAxisRaw("Vertical");
            Vector3 direction = new Vector3(hInput, vInput, 0.0f).normalized;
            _velocityChange = new Vector3(Mathf.Abs(hInput), 0.0f, 0.0f) * 1.25f;

            _velocity = direction * _spd + _velocityChange;      //Create velocity

            if (hInput == 0 && vInput == 0)
            {
                ChangeAnimation(IDLE);
            }
            else if (hInput > 0)
            {
                ChangeAnimation(FORWARD);
            }
            else if (hInput < 0)
            {
                ChangeAnimation(BACKWARD);
            }
            else if (vInput > 0)
            {
                ChangeAnimation(UP);
            }
            else if (vInput < 0)
            {
                ChangeAnimation(DOWN);
            }

            transform.position += _velocity * Time.deltaTime;

            float xClamp = Mathf.Clamp(transform.position.x, -24.0f, 26.0f);    //Resrict x and y movement
            float yClamp = Mathf.Clamp(transform.position.y, -17.0f, 12.0f);
            transform.position = new Vector3(xClamp, yClamp, 0.0f);             //Ensure position does not exceed restrictions
        }
    }

    void PlayerShoot()
    {
        if (Input.GetKey(KeyCode.Space))                    //Check for space bar
        {
            if (Time.time > _cycleTime)                         //if game time is greater than weapon cycle time
            {
                _cycleTime = Time.time + _fireRate;             //Set next cycle time
                
                if (_bullet != null)                            //Fire bullet if it exists
                {
                    Instantiate(_bullet, _firingPosition.position, Quaternion.identity);
                    
                    if (SFXManager.Instance.IsMuted())
                    {
                        _audio.clip = null;
                    }
                    else
                    {
                        _audio.Play();
                    }
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

    public int SendPlayerHealth()
    {
        return Health;
    }
    public int PlayerMaxHealth()
    {
        return _maxHealth;
    }

    public int SendPlayerScore()
    {
        return _score;
    }

    public void PauseState()
    {
        _isPaused = !_isPaused;
    }

    private void OnTriggerEnter(Collider other)                         //Use to check for trigger collisions
    {
        if (other.tag == "Powerup")                                     //If we collide with powerup
        {
            if(Health < _maxHealth)
            {
                Health++;                                       // Add 1 to health if current health is less than 5
            }
            _uiManager.UpdateHealthBar(Health);                 //Update healthbar with current health
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

    private void ChangeAnimation(string newState)
    {
        if (_currentState == newState)
        {
            return;
        }

        _anim.Play(newState);

        _currentState = newState;
    }

    public void TakeDamage(int damage)
    {
        if (_isDead)
        {
            return;
        }

        Health -= damage;

        _uiManager.UpdateHealthBar(Health);
        Debug.Log("Updated Health Bar");
        if (Health < 1)
        {
            _isDead = true;

            Instantiate(_deathPrefab, transform.position, Quaternion.identity);

            _uiManager.PlayerDeath();                                                                       //Updates the Health bar on the HUD
            Destroy(this.gameObject);
        }
    }
}