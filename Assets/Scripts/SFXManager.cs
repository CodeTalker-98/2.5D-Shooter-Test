using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    [Header("Main Menu")]
    [SerializeField] private Toggle _sfxToggle;
    [SerializeField] private Slider _soundSlider;

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
        _soundSlider.value = 0.5f;
        _sfxToggle.isOn = true;
    }

    public void UpdateSFX()
    {
        if (_sfxToggle.isOn)
        {
            Debug.Log("SFX are on");
            _audio.mute = false;
        }
        else
        {
            Debug.Log("SFX are off");
            _audio.mute = true;
        }
    }

    public void UpdateVolume()
    {
        _audio.volume = _soundSlider.value;
    }

    public void SoundConfirmMenu()
    {
        Debug.Log("Confirm Sound Triggered");
        _audio.clip = _menuConfirm;
        _audio.Play();
    }

    public void SoundHoverMenu()
    {
        Debug.Log("Hover Sound Triggered");
        _audio.clip = _menuHover;
        _audio.Play();
    }

    public void SoundBackMenu()
    {
        _audio.clip = _menuBack;
        _audio.Play();
    }
}
