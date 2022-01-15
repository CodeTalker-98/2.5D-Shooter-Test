using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VtolAI : Enemy, IDamagable
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _firingPosition;
    [SerializeField] private float _amplitude = 1.0f;
    [SerializeField] private float _frequency = 1.0f;
    private WaitForSeconds _cycleTime;

    public int Health { get; set; }

    public override void Init()
    {
        base.Init();
        Health = base._health;
    }

    private void Start()
    {
        Init();
        _cycleTime = new WaitForSeconds(_fireRate);
        StartCoroutine(EnemyShoot());
    }

    public override void EnemyMovement()
    {
        float y = _amplitude * Mathf.Sin(Time.time * _frequency);
        Vector3 vtolVelocity = new Vector3(0.0f, y, _spd);
        transform.Translate(vtolVelocity * Time.deltaTime);

        if (transform.position.x < -37.0f)
        {
            Destroy(this.gameObject);
        }


        Debug.Log("Health: " + Health);
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

            Instantiate(_deathPrefab, transform.position, Quaternion.identity);

            if (_canSpawnPrefab)
            {
                Instantiate(_powerupPrefab, transform.position, Quaternion.Euler(-90.0f, 0.0f, 0.0f));
            }

            Destroy(this.gameObject);
        }
    }

    IEnumerator EnemyShoot()
    {
        while (!_isDead)
        {
            if (_bulletPrefab != null)
            {
                GameObject enemyBullets = Instantiate(_bulletPrefab, _firingPosition.position, Quaternion.Euler(0.0f, 180.0f, 0.0f));
                Bullet[] bullets = enemyBullets.GetComponents<Bullet>();
                
                for (int i = 0; i < bullets.Length; i++)
                {
                    bullets[i].IsEnemyBullet();
                }
            }
            yield return _cycleTime;
        }
    }
}
