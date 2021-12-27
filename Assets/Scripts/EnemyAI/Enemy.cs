using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [SerializeField] protected float _spd = 1.0f;                 //Sets enemy's movement speed
    [SerializeField] protected int _health = 1;                   //Sets enemy health
    [SerializeField] protected int _scoreValue = 100;             //Generic score value for enemy
    [SerializeField] protected int _damageValue = 1;             //Sets damage enemy can deal
    [SerializeField] protected int _collisionDamage = 1;
    [SerializeField] protected GameObject _powerupPrefab;         //Powerup to spawn
    [SerializeField] protected bool _canSpawnPrefab = false;        //Check if we can spawn prefab;
    protected Player _player;
    protected bool _isDead = false;

    public virtual void Init()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

        if (_player == null)
        {
            Destroy(this.gameObject);
        }

        CanSpawnPrefab();                                               //Run method to determine if we can spawn a powerup

        if (_canSpawnPrefab)                                            //If we can
        {
            MeshRenderer renderer = GetComponent<MeshRenderer>();       //Get the enemy's mesh renderer component
            renderer.material.color = Color.blue;                       //Set it equal to blue to differentiate
        }
    }

    private void Start()
    {
        Init();
    }

    public virtual void Update()
    {
        EnemyMovement();                            //Calls Movement if it is alive
    }

    public virtual void EnemyMovement()
    {
        Vector3 enemyVelocity = Vector3.left * _spd;                //Sets direction for enemy to move
        transform.Translate(enemyVelocity * Time.deltaTime);        //moves enemy

        if (transform.position.x < -12.5f)                          //Removes enemy if offscreen
        {
            Destroy(this.gameObject);
        }
    }

    public int CollisionDamage()
    {
        return _collisionDamage;
    }

    public virtual void CanSpawnPrefab()                       //Determines if we can spawn prefab
    {
        int randomInt = Random.Range(0, 3);             //Generates random number between max and min value
        
        if (randomInt == 1)                             //If random number equals set value
        {
            _canSpawnPrefab = true;                     //We are capable of spawning a prefab
        }
    }
}