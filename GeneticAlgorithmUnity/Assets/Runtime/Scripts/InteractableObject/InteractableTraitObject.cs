using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.InteractableObjects
{
    public class InteractableTraitObject : MonoBehaviour, IInteractable
    {
        public void Interact()
        {
            Debug.Log("Teste");
        }
    }
}
