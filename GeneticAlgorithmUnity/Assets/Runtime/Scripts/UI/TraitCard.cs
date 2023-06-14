using Game.Events;
using Game.Traits;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class TraitCard : MonoBehaviour
    {
        [SerializeField]
        private Trait<EntityEventType, EntityEventContext> _trait;
        [SerializeField]
        private TextMeshProUGUI _name;
        [SerializeField]
        private TextMeshProUGUI _description;

        public void SetTrait(Trait<EntityEventType, EntityEventContext> trait)
        {
            if (trait != null)
            {
                _trait = trait;

                _name.text = trait.Name;
                _description.text = trait.Description;                
            }
        }

        public void Select()
        {
            TraitSelection traitSelection = GetComponentInParent<TraitSelection>();

            if (traitSelection)
            {
                traitSelection.SelectTrait(_trait);
            }
        }
    } 
}
