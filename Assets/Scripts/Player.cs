using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float _spd = 5.0f;
    private Vector3 _velocity;

    private void Update()
    {
        PlayerMovement();
    }

    void PlayerMovement()
    {
        float hInput = Input.GetAxisRaw("Horizontal");
        float vInput = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(hInput, vInput, 0.0f);

        _velocity = direction * _spd;
        transform.Translate(_velocity * Time.deltaTime);
    }
}
