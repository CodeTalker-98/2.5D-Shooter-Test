using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AC130AI : Enemy, IDamagable
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private GameObject _shrapnelBulletPrefab;
    [SerializeField] private GameObject _damagePrefab;
    [SerializeField] private Transform _firingPosition;
    [SerializeField] private float _amplitude = 1.0f;
    [SerializeField] private float _frequency = 1.0f;
    [SerializeField] private AudioClip _firingSound;
    private enum AttackState
    {
        FirstState,
        SecondState,
        ThirdState
    }

    [SerializeField] private AttackState _attackState;
    private WaitForSeconds _cycleTime;
    private Animator _anim;
    private string _currentState;
    private int _maxHealth;
    private bool _firstState = true;
    private bool _secondState = false;
    private bool _thirdState = false;

    private const string UP = "AC130 Up";
    private const string DOWN = "AC130 Down";
    
    public int Health { get; set; }

    public override void Init()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _anim = GetComponent<Animator>();
        _audio = GetComponent<AudioSource>();
        _audio.clip = _firingSound;

        if(_player == null)
        {
            Destroy(this.gameObject);
        }

        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.HardModeValue())
            {
                _fireRate *= 0.75f;
                _spd *= 2.0f;
            }
        }

        Health = base._health;
        _maxHealth = Health;
        _attackState = AttackState.FirstState;
    }

    private void Start()
    {
        Init();
        _cycleTime = new WaitForSeconds(_fireRate);
        StartCoroutine(EnemyShoot());
    }

    public override void EnemyMovement()
    {
        switch (_attackState)
        {
            case AttackState.FirstState:
                float y = _amplitude * Mathf.Sin(Time.time * _frequency);
                Vector3 idle = new Vector3(0.0f, y, 0.0f);
                transform.position += idle * Time.deltaTime;
                
                if (y > 0)
                {
                    ChangeAnimation(UP);
                }
                else if (y < 0)
                {
                    ChangeAnimation(DOWN);
                }

                break;
            case AttackState.SecondState:
                float ySecond = _amplitude * Mathf.Sin(Time.time * _frequency);
                Vector3 idleSecond = new Vector3(0.0f, ySecond, 0.0f);
                transform.position += idleSecond * Time.deltaTime;

                if (ySecond > 0)
                {
                    ChangeAnimation(UP);
                }
                else if (ySecond < 0)
                {
                    ChangeAnimation(DOWN);
                }

                break;
            case AttackState.ThirdState:
                Vector3 bulletVelocity = Vector3.forward * _spd;
                transform.Translate(bulletVelocity * Time.deltaTime);
                break;
            default:
                break;
        }

        if (transform.position.x < -45.0f)
        {
            transform.position = new Vector3(45.0f, Random.Range(-17.0f, 12.0f), 0.0f);
        }
    }

    public void TakeDamage(int damage)
    {
        if (_isDead)
        {
            return;
        }

        Health -= damage;

        if ((Health < (_maxHealth * 0.66f)) && (Health > (_maxHealth * 0.33f)) && !_secondState)
        {
            _firstState = false;
            _secondState = true;
            _amplitude = 10.0f;
            GameObject fire = Instantiate(_damagePrefab, new Vector3(transform.position.x + 5.75f, transform.position.y, transform.position.z), Quaternion.identity);
            fire.transform.parent = this.transform;
            _attackState = AttackState.SecondState;
            _cycleTime = new WaitForSeconds(_fireRate * 1.5f);
        }
        else if (Health <= (_maxHealth * 0.33f) && !_thirdState)
        {
            _secondState = false;
            _thirdState = true;
            _spd = 30.0f;

            if (SFXManager.Instance.IsMuted())
            {
                _audio.clip = null;
            }
            else
            {
                _audio.clip = _movementSound;
                _audio.loop = true;
                _audio.Play();
            }

            GameObject fire = Instantiate(_damagePrefab, new Vector3(transform.position.x, transform.position.y - 1.5f, transform.position.z), Quaternion.identity);
            fire.transform.parent = this.transform;
            _attackState = AttackState.ThirdState;
            _cycleTime = new WaitForSeconds(_fireRate / 1.5f);
        }

        if (Health < 1)
        {
            _isDead = true;

            Instantiate(_deathPrefab, transform.position, Quaternion.identity);

            if(_player != null)
            {
                _player.UpdateScore(_scoreValue);
            }

            Destroy(this.gameObject);
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

    IEnumerator EnemyShoot()
    {
        while (!_isDead && _firstState)
        {
            BulletInstantiation(_bulletPrefab);
            yield return _cycleTime;
        }

        while (!_isDead && _secondState)
        {
            BulletInstantiation(_shrapnelBulletPrefab);
            yield return _cycleTime;
        }
    }

    private void BulletInstantiation(GameObject bulletPrefab)
    {
        if (_bulletPrefab != null)
        {
            GameObject bullets = Instantiate(bulletPrefab, _firingPosition.position, Quaternion.Euler(0.0f, 180.0f, 0.0f));
            Bullet[] bulletInstances = bullets.GetComponentsInChildren<Bullet>();

            if (SFXManager.Instance.IsMuted())
            {
                _audio.clip = null;
            }
            else
            {
                _audio.Play();
            }
            
            for (int i = 0; i < bulletInstances.Length; i++)
            {
                bulletInstances[i].IsEnemyBullet();
            }
        }
    }
}
