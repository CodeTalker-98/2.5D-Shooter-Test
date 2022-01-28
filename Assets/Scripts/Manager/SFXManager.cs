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

    [Header("Menu SFX")]
    [SerializeField] private AudioClip _menuConfirm;
    [SerializeField] private AudioClip _menuBack;
    [SerializeField] private AudioClip _menuHover;

    [Header("Main Menu")]
    [SerializeField] private Toggle _sfxToggle;
    [SerializeField] private Slider _soundSlider;

    [Header("Game SFX")]
    [SerializeField] private AudioClip _playerShoot;
    [SerializeField] private AudioClip _playerMinigun;
    [SerializeField] private AudioClip _enemyShoot;
    [SerializeField] private AudioClip _explosion;

    [Header("Enemies")]
    [SerializeField] private AudioClip _chopper;
    [SerializeField] private AudioClip _boat;
    [SerializeField] private AudioClip jet;
    [SerializeField] private AudioClip _stuka;
    [SerializeField] private AudioClip _warthog;
    [SerializeField] private AudioClip _warthogShoot;

    private AudioSource _audio;
    private AudioSource _musicAudio;

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
        _musicAudio = MusicManager.Instance.GetComponent<AudioSource>();
        if (_soundSlider != null)
        {
            _soundSlider.value = 0.5f;
            UpdateVolume();
        }
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
        _musicAudio.volume = _soundSlider.value;
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

    public bool IsMuted()
    {
        return _audio.mute;
    }

    public void PauseSound(bool isPaused)
    {
        _audio.mute = isPaused;
    }
}
