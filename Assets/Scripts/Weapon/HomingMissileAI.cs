using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingMissileAI : Bullet
{
    [SerializeField] private float _delay = 0.5f;
    [SerializeField] private float _fuseDelay = 5.0f;
    [SerializeField] private float _rotateSpd = 1.0f;
    private Transform _target;
    private Transform _localTransform;
    private Rigidbody _rb;
    private WaitForSeconds _acquisitionDelay;

    private void Start()
    {
        _acquisitionDelay = new WaitForSeconds(_delay);
        _rb = GetComponent<Rigidbody>();
        StartCoroutine(AcquireTarget());
        _localTransform = GetComponent<Transform>();
    }

    private void Update()
    {
        Destroy(this.gameObject, _fuseDelay);
    }
    private void FixedUpdate()
    {
        HomingMissileBehavior();
    }

    private void HomingMissileBehavior()
    {
        if (_target != null)
        {
            _rb.velocity = _localTransform.forward * _spd * Time.deltaTime;
            Quaternion rotation = Quaternion.LookRotation(_target.position - _localTransform.position);
            _rb.MoveRotation(Quaternion.RotateTowards(_localTransform.rotation, rotation, _rotateSpd));
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
        else if (other.tag != "Enemy")
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
        yield return _acquisitionDelay;
        if (_isEnemyBullet)
        {
            _target = GameObject.Find("Player").GetComponent<Transform>();
        }
        else
        {
            _target = GameObject.FindGameObjectWithTag("Enemy").transform;
        }
    }
}
