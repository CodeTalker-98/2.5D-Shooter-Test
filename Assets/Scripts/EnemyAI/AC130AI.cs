using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AC130AI : Enemy, IDamagable
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private GameObject _shrapnelBulletPrefab;
    [SerializeField] private Transform _firingPosition;
    [SerializeField] private float _amplitude = 1.0f;
    [SerializeField] private float _frequency = 1.0f;
    [SerializeField] private float _fireRate = 0.75f;
    private enum AttackState
    {
        FirstState,
        SecondState,
        ThirdState
    }

    [SerializeField] private AttackState _attackState;
    private WaitForSeconds _cycleTime;
    private int _maxHealth;
    private bool _firstState = true;
    private bool _secondState = false;
    private bool _thirdState = false;
    public int Health { get; set; }

    public override void Init()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        if(_player == null)
        {
            Destroy(this.gameObject);
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
                transform.Translate(idle * Time.deltaTime);
                break;
            case AttackState.SecondState:
                float ySecond = _amplitude * Mathf.Sin(Time.time * _frequency);
                Vector3 idleSecond = new Vector3(0.0f, ySecond, 0.0f);
                transform.Translate(idleSecond * Time.deltaTime);
                break;
            case AttackState.ThirdState:
                Vector3 bulletVelocity = Vector3.left * _spd;
                transform.Translate(bulletVelocity * Time.deltaTime);
                break;
            default:
                break;
        }

        if (transform.position.x < -15.0f)
        {
            transform.position = new Vector3(10.75f, Random.Range(-3.0f, 5.0f), 0.0f);
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
            _frequency = 5.0f;
            _attackState = AttackState.SecondState;
            _cycleTime = new WaitForSeconds(_fireRate * 1.5f);
        }
        else if (Health <= (_maxHealth * 0.33f) && !_thirdState)
        {
            _secondState = false;
            _thirdState = true;
            _spd = 10.0f;
            _attackState = AttackState.ThirdState;
            _cycleTime = new WaitForSeconds(_fireRate / 1.5f);
        }

        if (Health < 1)
        {
            _isDead = true;

            // play anim

            if(_player != null)
            {
                _player.UpdateScore(_scoreValue);
            }

            Destroy(this.gameObject);
        }
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
            GameObject bullets = Instantiate(bulletPrefab, _firingPosition.position, Quaternion.identity);
            Bullet[] bulletInstances = bullets.GetComponentsInChildren<Bullet>();
            
            for (int i = 0; i < bulletInstances.Length; i++)
            {
                bulletInstances[i].IsEnemyBullet();
            }
        }
    }
}
