using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] private float _spd = 1.0f;                 //Speed to move at

    private void Update()
    {
        Movement();                                             //Call method to move
    }

    private void Movement()
    {
        Vector3 direction = Vector3.left;                       //Direction to move
        Vector3 velocity = direction * _spd;                    //Direction * speed
        transform.position += velocity * Time.deltaTime;       //Implement moving
    }
}

