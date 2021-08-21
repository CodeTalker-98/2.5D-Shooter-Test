using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _spd = 15.0f; // Speed of the Bullet
    
    private void Update()
    {
        BulletMovement();  // Call Bullet Method
    }

    private void BulletMovement()
    {
        Vector3 bulletVelocity = Vector3.right * _spd;      //Set Bullet Direction
        transform.Translate(bulletVelocity * Time.deltaTime); // Move Bullet
    }
}
