using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingAI : Enemy, IDamagable
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _firingPosition;
    private WaitForSeconds _actionCycle;
    public int Health {get; set;}

    public override void Init()
    {
        base.Init();
        Health = base._health;
    }

    void Start()
    {
        Init();
        _actionCycle = new WaitForSeconds(_fireRate);
        StartCoroutine(EnemyShoot());
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
            if (_player != null)
            {
                _player.UpdateScore(_scoreValue);
            }

            _isDead = true;

            //play anim

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
                GameObject enemyBullets = Instantiate(_bulletPrefab, _firingPosition.position, Quaternion.identity);
                Bullet[] bullets = enemyBullets.GetComponents<Bullet>();

                for (int i = 0; i < bullets.Length; i++)
                {
                    bullets[i].IsEnemyBullet();
                }
            }
            yield return _actionCycle;
        }
    }
}