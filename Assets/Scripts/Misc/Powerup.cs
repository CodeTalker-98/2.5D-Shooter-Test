using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField] private float _spd = 1.0f;                 //Speed to move at
    [SerializeField] private AudioClip _powerupSound;
    private Mesh _mesh;
    private Collider _collider;
    private AudioSource _audio;

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
        _audio.clip = _powerupSound;
        _mesh = GetComponent<MeshFilter>().mesh;
        _collider = GetComponent<Collider>();
    }

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (SFXManager.Instance.IsMuted())
            {
                _audio.clip = null;
            }
            else
            {
                _audio.Play();
            }

            _mesh.Clear();
            Destroy(_collider);
        }
    }
}

