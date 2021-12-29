using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _spd = 15.0f;                // Speed of the Bullet
    [SerializeField] private int _damageValue = 1;
    private bool _isEnemyBullet = false;
    private bool _isBomberBullet = false;
    private bool _isAaBullet = false;
    private bool _isBossBullet = false;
    
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
        else if (_isBossBullet)
        {
            BulletMoveUp();
        }
    }

    public void BulletMoveUp()
    {
        Vector3 bulletVelocity = Vector3.up * _spd;
        transform.Translate(bulletVelocity * Time.deltaTime);

        if (transform.position.y > 5.0f)
        {
            Destroy(this.gameObject);
        }
    }

    private void BulletMoveRight()
    {
        Vector3 bulletVelocity = Vector3.right * _spd;          //Set Bullet Direction
        transform.Translate(bulletVelocity * Time.deltaTime);   // Move Bullet

        if (transform.position.x > 10.75f)
        {
            Destroy(this.gameObject);
        }
    }

    private void BulletMoveLeft()
    {
        Vector3 bulletVelocity = Vector3.left * _spd;           //Set Bullet Direction
        transform.Translate(bulletVelocity * Time.deltaTime);   // Move Bullet

        if (transform.position.x < -10.75f)
        {
            Destroy(this.gameObject);
        }
    }

    private void BulletMoveDown()
    {
        Vector3 bulletVelocity = Vector3.down * _spd;
        transform.Translate(bulletVelocity * Time.deltaTime);

        if (transform.position.y < -5.0f)
        {
            Destroy(this.gameObject);
        }
    }

    private void BulletMoveDiagonalLeft()
    {
        Vector3 bulletVelocity = new Vector3(-1, 1, 0.0f).normalized * _spd;
        transform.Translate(bulletVelocity * Time.deltaTime);

        if(transform.position.x < -10.75f || transform.position.y > 7.0f)
        {
            Destroy(this.gameObject);
        }
    }

    public void IsEnemyBullet()
    {
        _isEnemyBullet = true;
    }

    public void IsAABullet()
    {
        _isAaBullet = true;
    }

    public void IsBomberBullet()
    {
        _isBomberBullet = true;
    }

    public void IsBossBullet()
    {
        _isBossBullet = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && _isEnemyBullet ||other.tag == "Player" && _isAaBullet || other.tag == "Player" && _isBomberBullet || other.tag == "Player" && _isBossBullet)
        {
            IDamagable hit = other.GetComponent<IDamagable>();

            if (hit != null)
            {
                hit.TakeDamage(_damageValue);
                Destroy(this.gameObject);
            }
        } else if (other.tag == "Enemy" && !_isEnemyBullet && !_isAaBullet && !_isBomberBullet && !_isBossBullet)
        {
            IDamagable hit = other.GetComponent<IDamagable>();

            if (hit != null)
            {
                hit.TakeDamage(_damageValue);
                Destroy(this.gameObject);
            }
        }       
    }
}
