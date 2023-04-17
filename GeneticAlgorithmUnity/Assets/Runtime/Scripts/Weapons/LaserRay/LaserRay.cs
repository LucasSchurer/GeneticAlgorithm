using Game.Entities.Shared;
using Game.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Weapons
{
    [RequireComponent(typeof(LineRenderer))]
    public class LaserRay : Weapon<LaserRayData>
    {
        [SerializeField]
        private Transform _rayOrigin;
        [SerializeField]
        private LayerMask _rayCollidableLayerMask;

        private IEntityController _entityController;
        private LineRenderer _lineRenderer;

        private Vector3 _lookDirection;

        private bool _isPreparingToFire = false;

        protected override void Awake()
        {
            base.Awake();
            _entityController = GetComponent<IEntityController>();
            _lineRenderer = GetComponent<LineRenderer>();
        }

        private void DisplayRayPreview()
        {
            Vector3 endPosition = _rayOrigin.position + _lookDirection * _settings.HitRange;

            if (Raycast(_lookDirection, out RaycastHit hit))
            {
                endPosition = hit.point;
            }

            if (_lookDirection != Vector3.zero)
            {
                _lineRenderer.positionCount = 2;
                _lineRenderer.SetPosition(0, _rayOrigin.position);
                _lineRenderer.SetPosition(1, endPosition);
            } else
            {
                _lineRenderer.positionCount = 0;
            }
        }

        private void TryFire(ref EntityEventContext ctx)
        {
            if (_canUse && !_isPreparingToFire)
            {
                StartCoroutine(PrepareFireCoroutine());
            }
        }

        private void Fire(Vector3 direction)
        {
            if (Raycast(direction, out RaycastHit hit))
            {
                EntityEventController other = hit.transform.GetComponent<EntityEventController>();

                if (other != null)
                {
                    other.TriggerEvent(EntityEventType.OnHitTaken, new EntityEventContext() { Other = transform.gameObject, HealthModifier = -_settings.damage });
                    _eventController.TriggerEvent(EntityEventType.OnHitDealt, new EntityEventContext() { Other = other.gameObject, HealthModifier = -_settings.damage });
                }
            }

            StartCoroutine(Recharge());
        }

        private bool Raycast(Vector3 direction, out RaycastHit hit)
        {
            if (Physics.Raycast(_rayOrigin.position, direction, out RaycastHit thit, _settings.HitRange, _rayCollidableLayerMask))
            {
                hit = thit;
                return true;
            }
            else
            {
                hit = default;
                return false;
            }
        }

        private IEnumerator PrepareFireCoroutine()
        {
            _lookDirection = _entityController.GetLookDirection();

            if (_lookDirection == Vector3.zero)
            {
                _isPreparingToFire = false;
                yield break;
            }

            _isPreparingToFire = true;

            float elapsedTime = 0f;

            while (elapsedTime <= _settings.FirePreparingTime)
            {
                elapsedTime += Time.deltaTime;
                _lookDirection = _entityController.GetLookDirection();
                DisplayRayPreview();
                yield return null;
            }

            Vector3 fireDirection = _lookDirection;

            elapsedTime = 0f;

            if (_settings.DelayBetweenPreparingAndFire > 0)
            {
                _entityController.SetCanMove(false);

                while (elapsedTime <= _settings.DelayBetweenPreparingAndFire)
                {
                    elapsedTime += Time.deltaTime;
                    DisplayRayPreview();
                    yield return null;
                }

                _entityController.SetCanMove(true);
            }

            _lineRenderer.positionCount = 0;
            _isPreparingToFire = false;
            Fire(fireDirection);
        }

        private IEnumerator Recharge()
        {
            _canUse = false;

            yield return new WaitForSeconds(_settings.cooldown);

            _canUse = true;
        }

        public override void StartListening()
        {
            base.StartListening();

            _eventController?.AddListener(EntityEventType.OnPrimaryActionPerformed, TryFire);
        }

        public override void StopListening()
        {
            base.StopListening();

            _eventController?.RemoveListener(EntityEventType.OnPrimaryActionPerformed, TryFire);
        }
    } 
}
