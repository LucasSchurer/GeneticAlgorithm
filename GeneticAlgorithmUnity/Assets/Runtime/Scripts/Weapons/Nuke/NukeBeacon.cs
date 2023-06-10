using Game.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Weapons
{
    public class NukeBeacon : MonoBehaviour
    {
        private NukeData _data;
        private Nuke _nuke;
        private LayerMask _damageLayer;

        private Material _glowingEyeMaterial;

        [SerializeField]
        private ParticleSystem _chargingParticles;
        [SerializeField]
        private Renderer _indicatorRenderer;
        private Transform _indicator;
        [SerializeField]
        private Renderer _boundsRenderer;
        private Transform _bounds;
        private GameObject _owner;

        [SerializeField]
        private ParticleSystem _explosionParticles;

        public void Initialize(GameObject owner, Nuke nuke, NukeData data, LayerMask damageLayer)
        {
            _data = data;
            _nuke = nuke;
            _damageLayer = damageLayer;
            _owner = owner;

            GetComponent<LineRenderer>().material = _data.LineRendererMaterial;

            _glowingEyeMaterial = GetComponent<MeshRenderer>().materials[9];
            _glowingEyeMaterial.SetColor("_EmissionColor", _data.BaseColor * 2f);

            SetParticleSystemStartColor(_chargingParticles);

            SetRendererColor(_indicatorRenderer);
            SetRendererColor(_boundsRenderer);

            _indicator = _indicatorRenderer.transform;
            _bounds = _boundsRenderer.transform;

            float size = _data.Radius;

            _indicator.localScale = new Vector3(size, _indicator.localScale.y, size);
            _bounds.localScale = new Vector3(size, _bounds.localScale.y, size);

            ParticleSystem.MainModule explosionMain = _explosionParticles.main;
            explosionMain.startSize = _data.Radius * 2f;

            StartCoroutine(ExplosionCountdownCoroutine());
        }

        private void SetParticleSystemStartColor(ParticleSystem particles)
        {
            ParticleSystem.MainModule main = particles.main;
            main.startColor = _data.BaseColor;
        }

        private void SetRendererColor(Renderer renderer)
        {
            renderer.material.SetColor("_EmissionColor", _data.BaseColor * 2f);
            renderer.material.SetColor("_Color", _data.BaseColor * 2f);
        }

        private IEnumerator ExplosionCountdownCoroutine()
        {
            float elapsedTime = 0f;
            Vector3 indicatorScale = _indicator.localScale;
            float scale = _indicator.localScale.x;
            float initialScale = scale;

            while (elapsedTime < _data.ExplosionTime)
            {
                scale = Mathf.Lerp(initialScale, 0f, elapsedTime / _data.ExplosionTime);

                indicatorScale.x = scale;
                indicatorScale.z = scale;

                _indicator.localScale = indicatorScale;

                elapsedTime += Time.deltaTime;

                yield return null;
            }

            Explode();
        }

        private void Explode()
        {
            _explosionParticles.Play();

            GetComponent<MeshRenderer>().enabled = false;
            _bounds.gameObject.SetActive(false);
            _indicator.gameObject.SetActive(false);

            foreach (Collider hit in Physics.OverlapSphere(transform.position, _data.Radius, _damageLayer))
            {
                EntityEventController other = hit.transform.GetComponent<EntityEventController>();

                if (other != null)
                {
                    EntityEventContext.DamagePacket damagePacket = new EntityEventContext.DamagePacket()
                    {
                        Damage = _data.Damage,
                        ImpactPoint = hit.transform.position,
                        HitDirection = (hit.transform.position - transform.position).normalized
                    };

                    other.TriggerEvent(EntityEventType.OnHitTaken, new EntityEventContext() { Other = transform.gameObject, Damage = damagePacket });

                    if (_owner)
                    {
                        _owner.GetComponent<EntityEventController>().TriggerEvent(EntityEventType.OnHitDealt, new EntityEventContext() { Other = other.gameObject, Damage = damagePacket });
                    }
                }
            }

            Destroy(gameObject, _explosionParticles.main.duration);
        }
    } 
}
