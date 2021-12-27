using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _spd = 15.0f;                // Speed of the Bullet
    [SerializeField] private int _damageValue = 1;
    private bool _isEnemyBullet = false;
    
    private void Update()
    {
        if (_isEnemyBullet)
        {
            BulletMoveLeft();
        }
        else
        {
            BulletMoveRight();                                       // Call Bullet Method
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

    public void IsEnemyBullet()
    {
        _isEnemyBullet = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        //if(other.tag == "Enemy" && !_isEnemyBullet)
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
