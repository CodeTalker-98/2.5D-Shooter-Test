using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitiaizeCamera : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;

    private float _camOffest = 0.2f;

    private void Start()
    {
        _canvas.worldCamera = Camera.main.gameObject.GetComponent<Camera>();
        _canvas.planeDistance = Camera.main.gameObject.GetComponent<Camera>().nearClipPlane + _camOffest;
    }
}
