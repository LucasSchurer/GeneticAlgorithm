using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Traits
{
    [CreateAssetMenu(menuName = "TraitWeights", fileName = "TraitWeights")]
    public class TraitWeights : ScriptableObject
    {
        private Dictionary<TraitIdentifier, float> _traitWeights;

        [SerializeField]
        private List<TraitWeight> _traitWeightsList;
        [SerializeField]
        private float _defaultWeight = 0.5f;
        [SerializeField]
        private float _minimumWeight = 0f;

        public Dictionary<TraitIdentifier, float> GetTraitWeightsDictionary()
        {
            if (_traitWeights == null)
            {
                _traitWeights = new Dictionary<TraitIdentifier, float>();

                foreach (TraitWeight weight in _traitWeightsList)
                {
                    _traitWeights.TryAdd(weight.Trait, weight.Weight);
                }
            }

            return _traitWeights;
        }


        private void Awake()
        {
            if (_traitWeights == null)
            {
                InitializeTraitWeights();
            }
        }

        private void InitializeTraitWeights()
        {
            _traitWeights = new Dictionary<TraitIdentifier, float>();

            foreach (TraitWeight weight in _traitWeightsList)
            {
                _traitWeights.TryAdd(weight.Trait, weight.Weight);
            }
        }

        public float GetTraitWeight(TraitIdentifier trait)
        {
            if (_traitWeights == null)
            {
                InitializeTraitWeights();
            }

            if (_traitWeights.TryGetValue(trait, out float weight))
            {
                return weight;
            } else
            {
                _traitWeights.Add(trait, _defaultWeight);
                return _defaultWeight;
            }
        }

        public void ChangeTraitWeight(TraitIdentifier trait, float newWeight)
        {
            if (_traitWeights == null)
            {
                InitializeTraitWeights();
            }

            float clampedWeight = Mathf.Clamp(newWeight, _minimumWeight, 1);

            if (_traitWeights.ContainsKey(trait))
            {
                _traitWeights[trait] = clampedWeight;
            }
            else
            {
                _traitWeights.Add(trait, clampedWeight);
            }
        }

        public void SaveWeights()
        {
            if (_traitWeights == null)
            {
                InitializeTraitWeights();
            }

            _traitWeightsList.Clear();

            foreach (KeyValuePair<TraitIdentifier, float> trait in _traitWeights)
            {
                _traitWeightsList.Add(new TraitWeight(trait.Key, trait.Value));
            }
        }

        [System.Serializable]
        private struct TraitWeight
        {
            [SerializeField]
            private TraitIdentifier _trait;
            [SerializeField]
            private float _weight;

            public TraitIdentifier Trait => _trait;
            public float Weight => _weight;

            public TraitWeight(TraitIdentifier trait, float weight)
            {
                _trait = trait;
                _weight = weight;
            }
        }
    }
}
