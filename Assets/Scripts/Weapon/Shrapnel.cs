using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrapnel : Bullet
{
    private void Start()
    {
        Debug.DrawRay(transform.position, transform.right, Color.green, 1.0f);;
        _spd = 1.0f;
    }

    private void Update()
    {
        BulletMovement();
    }

    private void BulletMovement()
    {
        Debug.Log("In Bullet Movement");
        Vector3 bulletVelocity = transform.up * _spd;
        bulletVelocity = transform.TransformDirection(bulletVelocity);
        transform.Translate(bulletVelocity * Time.deltaTime);
        Debug.Log("Moving");

        if (transform.position.x < -10.75f || transform.position.x > 10.75f || transform.position.y < -7.0f || transform.position.y > 7.0f)
        {
            Debug.Log("Destroying because of bounds");
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Collided with Player");
            DealDamage(other);
        }
    }

    private void DealDamage(Collider other)
    {
        Debug.Log("Dealing Damage");
        IDamagable hit = other.GetComponent<IDamagable>();

        if (hit != null) 
        {
            hit.TakeDamage(_damageValue);
            //play anim
            Destroy(this.gameObject);
        }
    }
}
