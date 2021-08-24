using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _enemies;
    private Vector3 _spawnPos;
    private Player _player;
    private WaitForSeconds _spawnTimer;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _spawnTimer = new WaitForSeconds(1.0f);
    }

    private void Update()
    {
        if (_player != null)
        {
            StartCoroutine(SpawnEnemies());
        }
    }

    private void GenerateSpawnPos()
    {
        float pos = Random.Range(-3.5f, 5.5f);
        _spawnPos = new Vector3(13.0f, pos, 0.0f);
    }

    IEnumerator SpawnEnemies()
    {
       yield return _spawnTimer;
        Instantiate(_enemies[0], _spawnPos, Quaternion.identity);
    }
}
