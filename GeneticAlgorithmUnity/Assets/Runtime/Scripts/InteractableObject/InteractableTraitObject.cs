using Game.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.InteractableObjects
{
    public class InteractableTraitObject : MonoBehaviour, IInteractable
    {
        public void Interact()
        {
            GameManager.Instance.GetEventController().TriggerEvent(Events.GameEventType.OnGivePlayerTraits, new Events.GameEventContext());
        }
    }
}
