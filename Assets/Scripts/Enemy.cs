using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _spd = 1.0f;                 //Sets enemy's movement speed
    [SerializeField] private int _health = 1;                   //Sets enemy health
    [SerializeField] private int _scoreValue = 100;             //Generic score value for enemy
    [SerializeField] private int _damageOutput = 1;             //Sets damage enemy can deal
    [SerializeField] private GameObject _powerupPrefab;         //Powerup to spawn
    [SerializeField] private bool _canSpawnPrefab = false;      //Check if we can spawn prefab;

    private void Start()
    {
        CanSpawnPrefab();                                               //Run method to determine if we can spawn a powerup

        if (_canSpawnPrefab)                                            //If we can
        {
            MeshRenderer renderer = GetComponent<MeshRenderer>();       //Get the enemy's mesh renderer component
            renderer.material.color = Color.blue;                       //Set it equal to blue to differentiate
        }
    }

    private void Update()
    {
        if (_health > 0)                                //Checks health before doing anything
        {
            EnemyMovement();                            //Calls Movement if it is alive
        }
        else
        {
            Destroy(this.gameObject);                   //Destroys object if _health <= 0 
        }
    }

    private void EnemyMovement()
    {
        Vector3 enemyVelocity = Vector3.left * _spd;                //Sets direction for enemy to move
        transform.Translate(enemyVelocity * Time.deltaTime);        //moves enemy

        if (transform.position.x < -12.5f)                          //Removes enemy if offscreen
        {
            Destroy(this.gameObject);
        }
    }

    public void EnemyDamage(int damage)
    {
        if (_health > 0)                                //If _health > 0
        {
            _health -= damage;                          //Subtract health from enemy based on damage taken
        }
        else
        {
            Destroy(this.gameObject);                   //Otherwise destroy object
        }
    }

    private void CanSpawnPrefab()                       //Determines if we can spawn prefab
    {
        int randomInt = Random.Range(0, 2);             //Generates random number between max and min value
        
        if (randomInt == 1)                             //If random number equals set value
        {
            _canSpawnPrefab = true;                     //We are capable of spawning a prefab
        }
    }

    private void OnTriggerEnter(Collider other)    //Checks for trigger collision
    {
        Player player = GameObject.Find("Player").GetComponent<Player>();

        if(player != null)
        {
            if (other.tag == "Bullet")                 //If bullet destroy bullet  and self
            {
                Destroy(other.gameObject);          
                player.UpdateScore(_scoreValue);       //Call Update Score method
                //Animate Damage

                if (_canSpawnPrefab)                   //If we can spawn a powerup     
                {
                    Instantiate(_powerupPrefab, transform.position, Quaternion.identity);    //Spawn it where we die w/o rotation
                }

                Destroy(this.gameObject);              //Add time later
            }
            else if (other.tag == "Player")
            {
                player.DamagePlayer(_damageOutput);   //Damage Player
                //Animate Damage
                Destroy(this.gameObject);              //Destroy self
            }
        }            
    }
}
