using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    private static MusicManager _instance;
    public static MusicManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("The Music Manager is NULL");
            }
            return _instance;
        }
    }

    private AudioSource _audio;

    [Header("Options Menu")]
    [SerializeField] private Toggle _musicToggle;

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

    public void UpdateMusic()
    {
        if (_musicToggle.isOn)
        {
            Debug.Log("Music is on");
            _audio.mute = false;
        }
        else
        {
            Debug.Log("Music is off");
            _audio.mute = true;
        }
    }
}
