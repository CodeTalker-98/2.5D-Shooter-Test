﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VtolAI : Enemy, IDamagable
{
    [SerializeField] private float _fireRate = 1.0f;
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
        Vector3 vtolVelocity = new Vector3(-_spd, y, 0.0f);
        transform.Translate(vtolVelocity * Time.deltaTime);

        if (transform.position.x < -12.5f)
        {
            Destroy(this.gameObject);
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

            // play anim

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
                    bullets[i].IsEnemyBullet();
                }
            }
            yield return _cycleTime;
        }
    }
}
