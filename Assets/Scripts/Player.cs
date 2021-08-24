using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _spd = 5.0f;                 // Player Movement Speed
    [SerializeField] private float _fireRate = 1.0f;            //Fire Rate of weapon
    private float _cycleTime = 0.0f;                            //Cycle time for weapon
    private Vector3 _velocity;                                  //Velocity for player
    [SerializeField] private GameObject _bullet;                //Bullet Player Shoots
    //!!clamp transform.pos to 5.5,-3.5!!
    private void Update()
    {
        PlayerMovement();                                       // Move player
        PlayerShoot();                                          //Player Shoot
    }

    void PlayerMovement()
    {
        float hInput = Input.GetAxisRaw("Horizontal");          //Get input and use it to set direction
        float vInput = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(hInput, vInput, 0.0f);

        _velocity = direction * _spd;                           //Create velocity
        transform.Translate(_velocity * Time.deltaTime);        //Move player
    }

    void PlayerShoot()
    {
        if (Input.GetKeyDown(KeyCode.Space))                    //Check for space bar
        {
            if (Time.time > _cycleTime)                         //if game time is greater than weapon cycle time
            {
                _cycleTime = Time.time + _fireRate;             //Set next cycle time
                
                if (_bullet != null)                            //Fire bullet if it exists
                {
                    Instantiate(_bullet, transform.position, Quaternion.identity);
                }
                else
                {
                    Debug.LogError("The BULLET is null");
                }
            }
        }
    }
}
