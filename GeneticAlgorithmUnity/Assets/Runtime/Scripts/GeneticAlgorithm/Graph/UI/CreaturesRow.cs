using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.GA.UI
{
    public class CreaturesRow : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private CreatureButton _creatureButtonPrefab;

        [SerializeField]
        public Dictionary<int, CreatureButton> _creatures;
        private int _generation;

        private void Awake()
        {
            _creatures = new Dictionary<int, CreatureButton>();
        }

        public void SetGeneration(int generation)
        {
            _generation = generation;
        }

        public CreatureButton AddCreatureButton(int id)
        {
            if (!_creatures.ContainsKey(id))
            {
                CreatureButton creatureButton = Instantiate(_creatureButtonPrefab, transform);
                creatureButton.SetIdAndGeneration(id, _generation);
                _creatures.Add(id, creatureButton);
                return creatureButton;
            } else
            {
                return null;
            }
        }
    } 
}
