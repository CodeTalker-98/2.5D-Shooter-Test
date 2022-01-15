using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    [SerializeField] private float _time = 2.0f;

    void Start()
    {
        Destroy(this.gameObject, _time);
    }
}
