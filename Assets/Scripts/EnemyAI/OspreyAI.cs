using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OspreyAI : Enemy, IDamagable
{
    [SerializeField] private float _fireRate = 1.0f;
    [SerializeField] private float _wait = 3.0f;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private GameObject _homingMissilePrefab;
    [SerializeField] private Transform _firingPosition;
    [SerializeField] private Transform _lungePosition;
    [SerializeField] private Transform _startingPosition;
    [SerializeField] private Transform _topFiringPosition;
    [SerializeField] private Transform _bottomFiringPosition;
    [SerializeField] private Transform _target;
    [SerializeField] private float _amplitude = 1.0f;
    [SerializeField] private float _frequency = 1.0f;
    private int _maxHealth;
    [SerializeField] private bool _atTop = false;
    [SerializeField] private bool _atBottom = false;
    [SerializeField] private bool _atMiddle = false;
    [SerializeField] private bool _lowHealth = false;
    [SerializeField] private bool _healthMonitor = false;
    private bool _canRandomize = true;
    private bool _canWait = false;
    [SerializeField] private bool _goingHome = false;
    private WaitForSeconds _cycleTime;
    private WaitForSeconds _waitTime;

    public int Health { get; set; }
   
    public override void Init()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _target = GameObject.Find("Target").GetComponent<Transform>();

        if (_player == null)
        {
            Destroy(this.gameObject);
        }

        Health = base._health;
        _maxHealth = Health;
    }

    private void Start()
    {
        Init();
        _fireRate = 1.0f;
        _cycleTime = new WaitForSeconds(_fireRate);
        _waitTime = new WaitForSeconds(_wait);
        StartCoroutine(EnemyShoot());
    }

    public override void EnemyMovement()
    {
        float y = _amplitude * Mathf.Sin(Time.time * _frequency);
        Vector3 idleVelocity = new Vector3(0.0f, y, 0.0f);

        if (Health > _maxHealth * .5f)
        {
            transform.Translate(idleVelocity * Time.deltaTime);
        }
        else
        {
            Randomize();

            if (transform.position == _target.position && _canWait && !_goingHome)
            {
                StartCoroutine(Wait());
            }
            else if (_target.position == _startingPosition.position && transform.position == _target.position && _goingHome)
            {
                _goingHome = false;
                _canRandomize = true;
            }

            transform.position = Vector3.MoveTowards(transform.position, _target.position, _spd * Time.deltaTime);
        }
    }

    public void TakeDamage(int damage)
    {
        if (_isDead)
        {
            return;
        }

        Health -= damage;

        if (Health <= _maxHealth * 0.5f && !_lowHealth)
        {
            _lowHealth = true;
            _fireRate = 5.0f;
            _cycleTime = new WaitForSeconds(_fireRate);
            StartCoroutine(LaunchHomingMissile());
        }

        if (Health < 1)
        {
            _isDead = true;

            if (_player != null)
            {
                _player.UpdateScore(_scoreValue);
            }

            //play anim

            Destroy(this.gameObject);
        }
    }

    IEnumerator EnemyShoot()
    {
        while (!_isDead && (Health > _maxHealth * 0.5f))
        {
            if (_bulletPrefab != null)
            {
                GameObject enemyBullets = Instantiate(_bulletPrefab, _firingPosition.position, Quaternion.identity);
                Bullet[] bullets = enemyBullets.GetComponentsInChildren<Bullet>();

                for (int i = 0; i < bullets.Length; i++)
                {
                    bullets[i].IsEnemyBullet();
                }
            }

            yield return _cycleTime;
        }
    }

    IEnumerator LaunchHomingMissile()
    {
        while (!_isDead)
        {
            if (_homingMissilePrefab != null)
            {
                GameObject enemyHomingMissiles = Instantiate(_homingMissilePrefab, _firingPosition.position, Quaternion.identity);
                HomingMissileAI[] missiles = enemyHomingMissiles.GetComponents<HomingMissileAI>();

                for (int i = 0; i < missiles.Length; i++)
                {
                    missiles[i].IsEnemyBullet();
                }
            }
            yield return _cycleTime;
        }
    }

    private void Randomize()
    {
        if (_canRandomize)
        {
            int randomInt = Random.Range(0, 3);

            Debug.Log("Random Int: " + randomInt);

            switch (randomInt)
            {
                case 0:
                    _atTop = true;
                    _goingHome = false;
                    _target.position = _topFiringPosition.position;
                    break;

                case 1:
                    _atMiddle = true;
                    _goingHome = false;
                    _target.position = _lungePosition.position;
                    break;

                case 2:
                    _atBottom = true;
                    _goingHome = false;
                    _target.position = _bottomFiringPosition.position;
                    break;

                default:
                    break;
            }
            _canRandomize = false;
            _canWait = true;
        }
        else
        {
            return;
        }
    }

    IEnumerator Wait()
    {
        _canWait = false;
        yield return _waitTime;
        _atTop = false;
        _atMiddle = false;
        _atBottom = false;
        _goingHome = true;
        _target.position = _startingPosition.position;
    }
}