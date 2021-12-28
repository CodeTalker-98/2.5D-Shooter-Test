using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kamikaze : Enemy, IDamagable
{
    public int Health { get; set; }

    public override void Init()
    {
        base.Init();
        Health = base._health;
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
                Instantiate(_powerupPrefab, transform.position, Quaternion.identity);
            }

            Destroy(this.gameObject); //after anim is over
        }
    }
}
