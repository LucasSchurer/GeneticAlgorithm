using Game.Events;
using Game.Traits;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class TraitCardDetailed : MonoBehaviour
    {
        [SerializeField]
        private Trait<EntityEventType, EntityEventContext> _trait;
        [SerializeField]
        private TextMeshProUGUI _name;
        [SerializeField]
        private TextMeshProUGUI _description;
        [SerializeField]
        private TextMeshProUGUI _stacks;

        public void SetTrait(int stacks, Trait<EntityEventType, EntityEventContext> trait)
        {
            if (trait != null)
            {
                _trait = trait;

                _name.text = trait.Name;
                _description.text = trait.Description;
                _stacks.text = stacks.ToString();
            }
        }
    } 
}
