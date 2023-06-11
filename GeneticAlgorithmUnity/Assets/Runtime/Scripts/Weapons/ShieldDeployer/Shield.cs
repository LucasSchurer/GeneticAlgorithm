using Game.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Weapons
{
    public class Shield : MonoBehaviour, IEventListener
    {
        private Rigidbody _rb;
        private ShieldDeployerData _data;
        private EntityEventController _eventController;

        [SerializeField]
        private Vector3 _groundCheckOffset;
        [SerializeField]
        private float _groundSphereRadius;
        [SerializeField]
        private LayerMask _groundLayer;
        [SerializeField]
        private Transform _timeBar;
        [SerializeField]
        private Transform _timeProgressBar;

        private float _currentDurability;
        private bool _isDeployed = false;

        public Rigidbody Rigidbody => _rb;

        public void Initialize(ShieldDeployerData data)
        {
            _data = data;
            _currentDurability = _data.Durability;

            if (_data.CanExpire)
            {
                _timeBar.gameObject.SetActive(true);
            } else
            {
                _timeBar.gameObject.SetActive(false);
            }

            _rb = GetComponent<Rigidbody>();

            GetComponent<MeshRenderer>().material = _data.ShieldMaterial;

            gameObject.layer = _data.ShieldLayer;
        }

        private void Awake()
        {
            _eventController = GetComponent<EntityEventController>();
        }

        private void Update()
        {
            if (!_isDeployed)
            {
                DetectGround();
            }
        }
        
        private void OnHealthChange(ref EntityEventContext ctx)
        {
            if (_data.CanBreak && ctx.Damage != null)
            {
                _currentDurability -= ctx.Damage.Damage;

                ctx.HealthChange = new EntityEventContext.HealthChangePacket() { CurrentHealth = _currentDurability, MaxHealth = _data.Durability };

                if (_currentDurability <= 0)
                {
                    Break();
                }
            }
        }

        private void DetectGround()
        {
            if (Physics.CheckSphere(transform.position + _groundCheckOffset, _groundSphereRadius, _groundLayer))
            {
                _isDeployed = true;
                Deploy();
            }
        }

        private void Deploy()
        {
            _rb.isKinematic = true;
            GetComponent<MeshCollider>().convex = false;

            if (_data.CanExpire)
            {
                StartCoroutine(ExpireCoroutine());
            }
        }

        private IEnumerator ExpireCoroutine()
        {
            float remainingTime = _data.ExpirationTime;
            Vector3 localScale = _timeProgressBar.localScale;

            while (remainingTime >= 0)
            {
                localScale.x = remainingTime / _data.ExpirationTime;
                _timeProgressBar.localScale = localScale;

                remainingTime -= Time.deltaTime;

                yield return null;
            }

            Break();
        }

        private void Break()
        {
            StopAllCoroutines();
            Destroy(gameObject);
        }

        private void OnEnable()
        {
            StartListening();
        }

        private void OnDisable()
        {
            StopListening();
        }

        public void StartListening()
        {
            if (_eventController)
            {
                _eventController.AddListener(EntityEventType.OnHealthChange, OnHealthChange);
            }
        }

        public void StopListening()
        {
            if (_eventController)
            {
                _eventController.RemoveListener(EntityEventType.OnHealthChange, OnHealthChange);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawSphere(transform.position + _groundCheckOffset, _groundSphereRadius);
        }
    } 
}
