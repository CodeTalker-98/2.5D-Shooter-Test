using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    [SerializeField] private float _time = 2.0f;
    [SerializeField] private AudioClip _explosion;
    private AudioSource _audio;

    void Start()
    {
        _audio = GetComponent<AudioSource>();
        _audio.clip = _explosion;

        if (SFXManager.Instance.IsMuted())
        {
            _audio.clip = null;
        }
        else
        {
            _audio.Play();
        }

        Destroy(this.gameObject, _time);
    }
}
