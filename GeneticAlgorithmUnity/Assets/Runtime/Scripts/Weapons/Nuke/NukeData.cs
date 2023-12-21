using UnityEngine;

namespace Game.Weapons
{
    [CreateAssetMenu(menuName = Constants.WeaponDataMenuName + "/NukeData")]
    public class NukeData : WeaponData
    {
        [Header("Nuke Settings")]
        [SerializeField]
        private float _maxDistance;
        [SerializeField]
        private float _radius;
        [SerializeField]
        private NukeBeacon _beacon;
        [SerializeField]
        private float _npcDetectionRange;
        [SerializeField]
        private Color _baseColor;
        [SerializeField]
        private Material _lineRendererMaterial;
        [SerializeField]
        private float _explosionTime;

        public float MaxDistance => _maxDistance;
        public float Radius => _radius;
        public NukeBeacon Beacon => _beacon;
        public float NPCDetectionRange => _npcDetectionRange;
        public Color BaseColor => _baseColor;
        public Material LineRendererMaterial => _lineRendererMaterial;
        public float ExplosionTime => _explosionTime;
    }
}

