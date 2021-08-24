using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _spd = 1.0f;     //Sets nemy's movement speed
    [SerializeField] private int _health = 1;       //Sets enemy health

    private void Update()
    {
        if (_health > 0)                            //Checks health before doing anything
        {
            EnemyMovement();                        //Calls Movement if it is alive
        }
        else
        {
            Destroy(this.gameObject);               //Destroys object if _health <= 0 
        }
    }

    private void EnemyMovement()
    {
        Vector3 enemyVelocity = Vector3.left * _spd;                //Sets direction for enemy to move
        transform.Translate(enemyVelocity * Time.deltaTime);        //moves enemy
    }

    public void Damage(int damage)
    {
        _health -= damage;                          //Subtract health from enemy based on damage taken
    }
}
