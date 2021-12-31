using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissileAI : Bullet
{
    [SerializeField] private float _delay = 0.5f;
    [SerializeField] private float _fuseDelay = 5.0f;
    [SerializeField] private float _rotateSpd = 1.0f;
    private Transform _target;
    private Rigidbody _rb;
    private WaitForSeconds _acquisitionDelay;

    private void Start()
    {
        _acquisitionDelay = new WaitForSeconds(_delay);
        StartCoroutine(AcquireTarget());
    }

    private void Update()
    {
        HomingMissileBehavior();

        if (_target == null)
        {
            Destroy(this.gameObject);
        }

        Destroy(this.gameObject, _fuseDelay);
    }

    private void HomingMissileBehavior()
    {
        if (_target != null)
        {
            transform.LookAt(_target.transform);
            StartCoroutine(LaunchMissile());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && _isEnemyBullet)
        {
            IDamagable hit = other.GetComponent<IDamagable>();

            if (hit != null)
            {
                hit.TakeDamage(_damageValue);
                //animate
                Destroy(this.gameObject);
            }
        }
        else if (other.tag == "Enemy")
        {
            IDamagable hit = other.GetComponent<IDamagable>();

            if (hit != null)
            {
                hit.TakeDamage(_damageValue);
                //play anim
                Destroy(this.gameObject);
            }
        }
    }

    IEnumerator AcquireTarget()
    {
        if (_isEnemyBullet)
        {
            _target = GameObject.Find("Player").GetComponent<Transform>();
        }
        else
        {
            _target = GameObject.FindGameObjectWithTag("Enemy").transform;
        }
        yield return _acquisitionDelay;
    }

    IEnumerator LaunchMissile()
    {
        yield return _acquisitionDelay;
        while (Vector3.Distance(_target.transform.position, transform.position) > 0.3f)
        {
            transform.position += (_target.transform.position - transform.position).normalized * _spd * Time.deltaTime;
            transform.LookAt(_target.transform);
            yield return null;
        }
        Destroy(this.gameObject);
    }
}
