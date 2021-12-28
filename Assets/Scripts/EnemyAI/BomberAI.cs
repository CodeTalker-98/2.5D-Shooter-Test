﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberAI : Enemy, IDamagable
{
    [SerializeField] private float _fireRate;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _firingPosition;
    private WaitForSeconds _actionCycle;
    public int Health { get; set; }

    public override void Init()
    {
        base.Init();
        Health = base._health;
    }

    private void Start()
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
            _isDead = true;

            //play anim

            if (_player != null)
            {
                _player.UpdateScore(_scoreValue);
            }

            if (_canSpawnPrefab)
            {
                Instantiate(_powerupPrefab, transform.position, Quaternion.identity);
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
                    bullets[i].IsBomberBullet();
                }
            }
            yield return _actionCycle;
        }
    }
}
