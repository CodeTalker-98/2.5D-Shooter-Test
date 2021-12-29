using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OspreyAI : Enemy, IDamagable
{
    [SerializeField] private float _fireRate = 1.0f;
    [SerializeField] private float _wait = 3.0f;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _firingPosition;
    [SerializeField] private Transform _lungePosition;
    [SerializeField] private Transform _startingPosition;
    [SerializeField] private Transform _topFiringPosition;
    [SerializeField] private Transform _bottomFiringPosition;
    [SerializeField] private float _amplitude = 1.0f;
    [SerializeField] private float _frequency = 1.0f;
    private int _maxHealth;
    private bool _atTop = false;
    private bool _atBottom = false;
    private bool _atMiddle = false;
    private bool _canRandomize = true;
    [SerializeField] private bool _switching = false;
    private Transform _target;
    private WaitForSeconds _cycleTime;
    private WaitForSeconds _waitTime;

    public int Health { get; set; }

    public override void Init()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

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
        _cycleTime = new WaitForSeconds(_fireRate);
        _waitTime = new WaitForSeconds(_wait);
        transform.position = _startingPosition.position;
        StartCoroutine(EnemyShoot());
    }

    public override void EnemyMovement()
    {
        float y = _amplitude * Mathf.Sin(Time.time * _frequency);
        Vector3 idleVelocity = new Vector3(0.0f, y, 0.0f);
        //Vector3 enemyMoveVelocity = Vector3.left * _spd;

        if (Health > _maxHealth * .5f)
        {
            transform.Translate(idleVelocity * Time.deltaTime);
        }
        else
        {
            if (_canRandomize)
            {
                Randomize();
            }
        }
    }

    private void MovePosition()
    {
        _canRandomize = false;
        if (!_switching)
        {
            if (_atTop)
            {
                _target.position = _topFiringPosition.position;
                transform.position = Vector3.Lerp(transform.position, _target.position, _spd * Time.deltaTime);
                if (transform.position == _target.position)
                {
                    StartCoroutine(Wait());
                }
            }
            else if (_atMiddle)
            {
                _target.position = _lungePosition.position;
            }
            else if (_atBottom)
            {
                _target.position = _bottomFiringPosition.position;
            }
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, _startingPosition.position, _spd * Time.deltaTime);
            if (transform.position == _startingPosition.position)
            {
                _canRandomize = true;
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
        while (!_isDead && (Health > _maxHealth * 0.5f) || !_isDead && _atMiddle)
        {
            if (_bulletPrefab != null)
            {
                GameObject enemyBullets = Instantiate(_bulletPrefab, _firingPosition.position, Quaternion.identity);
                Bullet[] bullets = enemyBullets.GetComponentsInChildren<Bullet>();

                for(int i = 0; i < bullets.Length; i++)
                {
                    bullets[i].IsEnemyBullet();
                }
            }

            yield return _cycleTime;
        }

        //Shoot down from Top

        while (!_isDead && _atTop)
        {
            if (_bulletPrefab != null)
            {
                GameObject enemyBullets = Instantiate(_bulletPrefab, _firingPosition.position, Quaternion.identity);
                Bullet[] bullets = enemyBullets.GetComponentsInChildren<Bullet>();

                for (int i = 0; i < bullets.Length; i++)
                {
                    bullets[i].IsBomberBullet();
                }
            }
        }

        //Shoot up from Bottom

        while (!_isDead && _atBottom)
        {
            if (_bulletPrefab != null)
            {
                GameObject enemyBullets = Instantiate(_bulletPrefab, _firingPosition.position, Quaternion.identity);
                Bullet[] bullets = enemyBullets.GetComponentsInChildren<Bullet>();

                for (int i = 0; i < bullets.Length; i++)
                {
                    bullets[i].IsBossBullet();
                }
            }
        }
    }

    private void Randomize()
    {
        _canRandomize = false;
        int randomInt = Random.Range(0, 1);

        switch (randomInt)
        {
            case 0:
                _atTop = true;
                _atMiddle = false;
                _atBottom = false;
                break;
            case 1:
                _atTop = false;
                _atMiddle = true;
                _atBottom = false;
                break;
            case 2:
                _atTop = false;
                _atMiddle = false;
                _atBottom = true;
                break;

            default:
                break;
        }

        MovePosition();
    }

    IEnumerator Wait()
    {
        yield return _waitTime;
        _atTop = false;
        _atMiddle = false;
        _atBottom = false;
        _switching = true;
    }
}
