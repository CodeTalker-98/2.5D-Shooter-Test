using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingAI : Enemy, IDamagable
{
    [SerializeField] protected float _fireRate = 1.0f;
    [SerializeField] protected GameObject _bulletPrefab;
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
            _isDead = true;

            if (_player != null)
            {
                _player.UpdateScore(_scoreValue);
            }

            //play anim

            if (_canSpawnPrefab)
            {
                Instantiate(_powerupPrefab, _firingPosition.position, Quaternion.identity);
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
                GameObject enemyBullet = Instantiate(_bulletPrefab, _firingPosition.position, Quaternion.identity);
                Bullet[] bullets = enemyBullet.GetComponents<Bullet>();

                for (int i = 0; i < bullets.Length; i++)
                {
                    bullets[i].IsEnemyBullet();
                }
            }
            yield return _actionCycle;
        }
    }
}