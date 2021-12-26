using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour
{
    [SerializeField] private Image _loadingBarImage;
    private WaitForEndOfFrame _frameUpdate;

    private void Start()
    {
        _frameUpdate = new WaitForEndOfFrame();
        StartCoroutine(LoadSceneAsync());
    }

    private IEnumerator LoadSceneAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Game");

        while (!asyncLoad.isDone)
        {
            _loadingBarImage.fillAmount = asyncLoad.progress;
            yield return _frameUpdate;
        }
        
    }
}
