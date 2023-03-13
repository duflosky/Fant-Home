﻿using UnityEngine;
using UnityEngine.InputSystem;

namespace Entities
{
    public class PlayerShooter : MonoBehaviour
    {
        [SerializeField] private WeaponsSO weapon;
        [SerializeField] private bool _triggerShoot;

        private GameObject _bullet;
        private float _bulletSpeed;
        private float _reloadTime;
        private string _bulletKey;
        private bool _shootOrder;
        private float _lastShootTime;
        private float _bulletSpread;

        private void Start()
        {
            ChangeWeapon();
        }

        public void ChangeWeapon()
        {
            _bulletSpeed = weapon.bulletSpeed;
            _reloadTime = weapon.reloadTime;
            _bulletSpread = weapon.bulletSpread;
            _bulletKey = weapon.key.ToString();
        }

        private void OnRotate(InputValue value)
        {
            if (!_triggerShoot && _lastShootTime + _reloadTime < Time.fixedTime)
            {
                CheckFire(value.Get<Vector2>());
            }
        }

        private void CheckFire(Vector2 value)
        {
            if (value == Vector2.zero) return;
            _lastShootTime = Time.fixedTime;
            Shoot();
        }

        public void Fire()
        {
            if (!_triggerShoot) return;
            _shootOrder = !_shootOrder;
        }

        private void Update()
        {
            if (_shootOrder && _lastShootTime + _reloadTime < Time.fixedTime)
            {
                _lastShootTime = Time.fixedTime;

                Shoot();
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void Shoot()
        {
            var randomEuler = transform.eulerAngles;
            randomEuler.y += Random.Range(0.0f, _bulletSpread);
            
            _bullet = Pooler.instance.Pop(_bulletKey);
            _bullet.GetComponent<Bullet>().speed = _bulletSpeed;
            _bullet.GetComponent<Bullet>().key = _bulletKey;
            _bullet.GetComponent<Bullet>().StartTimer();
            _bullet.transform.eulerAngles = randomEuler;
            _bullet.transform.position = transform.position;
        }
    }
}