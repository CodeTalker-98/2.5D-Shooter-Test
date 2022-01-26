using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrapnelBullet : Bullet
{
    [SerializeField] private GameObject _shrapnelPrefab;
    [SerializeField] private int _numberOfBullets = 2;
    [SerializeField] private float _lifespan = 1.0f;
    [SerializeField] private float _radius = 3.0f;
    private WaitForSeconds _detonationTimer;


    private void Start()
    {
        _detonationTimer = new WaitForSeconds(_lifespan);
        _damageValue = _numberOfBullets;
        IsShrapnel();
        StartCoroutine(Detonate());
    }

    private void Update()
    {
        BulletMovement();
    }

    private void BulletMovement()
    {
        Vector3 bulletVelocity = Vector3.right * _spd;
        transform.Translate(bulletVelocity * Time.deltaTime);

        if (transform.position.x < -45.0f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && _isEnemyBullet)
        {
            DealDamage(other);
        }
        else if (other.tag == "Enemy" && !_isEnemyBullet)
        {
            DealDamage(other);
        }
    }

    private void DealDamage(Collider other)
    {
        IDamagable hit = other.GetComponent<IDamagable>();

        if (hit != null)
        {
            hit.TakeDamage(_damageValue);
            Destroy(this.gameObject);
        }
    }

    IEnumerator Detonate()
    {
        yield return _detonationTimer;
        //play anim
        for (int i = 0; i < _numberOfBullets; i++)
        {
            float sector = i * Mathf.PI * 2 / _numberOfBullets;
            float x = Mathf.Cos(sector) * _radius;
            float y = Mathf.Sin(sector) * _radius;
            Vector3 spawnPoint = transform.position + new Vector3(x, y, 0.0f);
            Vector3 direction = spawnPoint - transform.position;
            Debug.DrawRay(transform.position, direction, Color.green);
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion spawnRotation = Quaternion.Euler(0.0f, 0.0f, angle);
            Instantiate(_shrapnelPrefab, spawnPoint, spawnRotation);
        }
        Destroy(this.gameObject);
    }
}
