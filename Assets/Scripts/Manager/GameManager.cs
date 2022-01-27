using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("The GameManager is null");
            }
            return _instance;
        }
    }

    [Header("Main Menu")]
    [SerializeField] private GameObject _optionsPanel;
    [SerializeField] private Toggle _hardModeToggle;
    [SerializeField] private Slider _brightnessSlider;
    private Light _directionalLight;

    private int _highScore = 0;
    private int _previousScore = 0;
    private int _sceneIndex;
    private int _waveNumber = 1;
    private float _brightness = 1.0f;
    private bool _hardMode;

    private Scene _scene;

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
        _directionalLight = GameObject.Find("Directional Light").GetComponent<Light>();
        
        if (_brightnessSlider != null)
        {
            _brightnessSlider.value = PlayerPrefs.GetFloat("Brightness", 1.0f);//_brightnessSlider.maxValue * 0.5f;
            UpdateBrightness();
        }

        _highScore = PlayerPrefs.GetInt("Highscore", 0);
        _brightness = PlayerPrefs.GetFloat("Brightness", 1.0f);
    }
    private void Update()
    {
        _scene = SceneManager.GetActiveScene();
        _sceneIndex = _scene.buildIndex;

        if (_directionalLight == null)
        {
            _directionalLight = GameObject.Find("Directional Light").GetComponent<Light>();
            UpdateBrightness();
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void DisplayHighScore()
    {

    }

    public void RetryLevel()
    {
        SceneManager.LoadScene(_sceneIndex);
    }

    public void Pause()
    {
        Time.timeScale = 0.0f;
    }

    public void Resume()
    {
        Time.timeScale = 1.0f;
    }

    public void ContinueGame()
    {
        int sceneCount = SceneManager.sceneCountInBuildSettings;

        if(_sceneIndex < (sceneCount - 1))
        {
            Debug.Log(_sceneIndex + "< " + sceneCount);
            SceneManager.LoadScene(_sceneIndex + 1);
        }
        else
        {
            SceneManager.LoadScene(0);
            Destroy(GameObject.Find("SFXManager").gameObject);
            Destroy(GameObject.Find("MusicManager").gameObject);
            Destroy(this.gameObject);
        }
    }

    public void UpdateBrightness()
    {
        if(_brightnessSlider != null)
        {
            _brightness = _brightnessSlider.value;
        }

        PlayerPrefs.SetFloat("Brightness", _brightness);
        _directionalLight.intensity = _brightness;
    }

    public void HighScore(int score)
    {
        _previousScore = score;

        if (_previousScore > _highScore)
        {
            _highScore = _previousScore;

            PlayerPrefs.SetInt("Highscore", _highScore);

            UIManager ui = GameObject.Find("UI").GetComponentInChildren<UIManager>();

            if (ui != null)
            {
                ui.DisplayHighScore(_highScore);
            }
            else
            {
                Debug.LogError("THE UI IS NULL");
            }
        }
    }

    public float SetBrightness()
    {
        return _brightness;
    }

    public void HardMode()
    {
        _hardMode = _hardModeToggle.isOn;
        Debug.Log("Hard mode: " + _hardMode);
    }

    public bool HardModeValue()
    {
        return _hardMode;
    }

    public void OptionsMenu()
    {
        _optionsPanel.SetActive(true);
    }

    public void BackButton()
    {
        _optionsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
