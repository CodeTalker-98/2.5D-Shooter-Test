using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrapnel : Bullet
{
    private void Update()
    {
        BulletMovement();
    }

    private void BulletMovement()
    {
        transform.position += -transform.up * Time.deltaTime * _spd; 

        if (transform.position.x < -30.0f || transform.position.x > 30.0f || transform.position.y < -30.0f || transform.position.y > 20.0f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            DealDamage(other);
        }
    }

    private void DealDamage(Collider other)
    {
        IDamagable hit = other.GetComponent<IDamagable>();

        if (hit != null) 
        {
            hit.TakeDamage(_damageValue);
            //play anim
            Destroy(this.gameObject);
        }
    }
}
