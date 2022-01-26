﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] protected float _spd = 15.0f;                // Speed of the Bullet
    [SerializeField] protected int _damageValue = 1;
    [SerializeField] protected GameObject _impact;
    [SerializeField] protected AudioClip _firingSound;
    protected AudioSource _audio;
    protected bool _isEnemyBullet = false;
    private bool _isBomberBullet = false;
    private bool _isAaBullet = false;
    private bool _isShrapnel = false;
    private Player _player;

    private void Start()
    {
        if (!_isEnemyBullet && !_isAaBullet && !_isBomberBullet && !_isShrapnel)
        {
            _player = GameObject.Find("Player").GetComponent<Player>();

            if (_player != null)
            {
                switch (_player.SendPlayerHealth())
                {
                    case 0:
                        _damageValue = 0;
                        break;
                    case 1:
                        _damageValue = 1;
                        break;
                    case 2:
                        _damageValue = 2;
                        break;
                    case 3:
                        _damageValue = 3;
                        break;
                    case 4:
                        _damageValue = 4;
                        break;
                    case 5:
                        _damageValue = 2;
                        break;
                    default:
                        _damageValue = 1;
                        break;
                }
            }
        }
    }

    private void Update()
    {
        if (_isEnemyBullet && !_isAaBullet && !_isBomberBullet)
        {
            BulletMoveLeft();
        }
        else if (!_isEnemyBullet && !_isAaBullet && !_isBomberBullet)
        {
            BulletMoveRight();                                       // Call Bullet Method
        }
        else if (!_isEnemyBullet && !_isAaBullet && _isBomberBullet)
        {
            BulletMoveDown();
        }
        else if (!_isEnemyBullet && _isAaBullet && !_isBomberBullet)
        {
            BulletMoveDiagonalLeft();
        }
    }

    private void BulletMoveRight()
    {
        Vector3 bulletVelocity = Vector3.right * _spd;          //Set Bullet Direction
        transform.Translate(bulletVelocity * Time.deltaTime);   // Move Bullet

        if (transform.position.x > 37.0f)
        {
            Destroy(this.gameObject);
        }
    }

    private void BulletMoveLeft()
    {
        Vector3 bulletVelocity = Vector3.right * _spd;           //Set Bullet Direction
        transform.Translate(bulletVelocity * Time.deltaTime);   // Move Bullet

        if (transform.position.x < -37.0f)
        {
            Destroy(this.gameObject);
        }
    }

    private void BulletMoveDown()
    {
        Vector3 bulletVelocity = Vector3.right * _spd;
        transform.Translate(bulletVelocity * Time.deltaTime);

        if (transform.position.y < -30.0f)
        {
            Destroy(this.gameObject);
        }
    }

    private void BulletMoveDiagonalLeft()
    {
        Vector3 bulletVelocity = new Vector3(1.0f, 0.0f, 0.0f).normalized * _spd;
        transform.Translate(bulletVelocity * Time.deltaTime);

        if(transform.position.x < -30.0f || transform.position.y > 15f)
        {
            Destroy(this.gameObject);
        }
    }

    public void IsEnemyBullet()
    {
        _isEnemyBullet = true;
    }

    public void IsShrapnel()
    {
        _isShrapnel = true;
    }

    public void IsAABullet()
    {
        _isAaBullet = true;
    }

    public void IsBomberBullet()
    {
        _isBomberBullet = true;
        _damageValue = 2;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && _isEnemyBullet ||other.tag == "Player" && _isAaBullet || other.tag == "Player" && _isBomberBullet || other.tag == "Player")
        {
            IDamagable hit = other.GetComponent<IDamagable>();

            if (hit != null)
            {
                hit.TakeDamage(_damageValue);
                Instantiate(_impact, transform.position, Quaternion.identity);
                Destroy(this.gameObject);
            }
        } else if (other.tag == "Enemy" && !_isEnemyBullet && !_isAaBullet && !_isBomberBullet)
        {
            IDamagable hit = other.GetComponent<IDamagable>();

            if (hit != null)
            {
                hit.TakeDamage(_damageValue);
                Instantiate(_impact, transform.position, Quaternion.identity);
                Destroy(this.gameObject);
            }
        }       
    }
}
