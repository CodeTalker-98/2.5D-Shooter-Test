using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    private static SFXManager _instance;
    public static SFXManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("The SFX Manager is NULL");
            }
            return _instance;
        }
    }

    private AudioSource _audio;

    [Header("Menu SFX")]
    [SerializeField] private AudioClip _menuConfirm;
    [SerializeField] private AudioClip _menuBack;
    [SerializeField] private AudioClip _menuHover;

    [Header("Game SFX")]
    [SerializeField] private AudioClip _playerShoot;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        _audio = GetComponent<AudioSource>();
    }


    public void SoundHoverMenu()
    {
        _audio.clip = _menuHover;
        _audio.Play();
    }

    public void SoundConfirmMenu()
    {
        _audio.clip = _menuConfirm;
        _audio.Play();
    }

    public void SoundBackMenu()
    {
        _audio.clip = _menuBack;
        _audio.Play();
    }
}
