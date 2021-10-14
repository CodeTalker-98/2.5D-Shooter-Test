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
        _sfxToggle.isOn = true;
        _soundSlider.value = 0.5f;
    }

    public void UpdateSFX()
    {
        _sfxToggle.isOn = !_sfxToggle.isOn;

        if (_sfxToggle.isOn)
        {
            _audio.mute = false;
        }
        else
        {
            _audio.mute = true;
        }
    }

    public void UpdateVolume()
    {
        _audio.volume = _soundSlider.value;
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
