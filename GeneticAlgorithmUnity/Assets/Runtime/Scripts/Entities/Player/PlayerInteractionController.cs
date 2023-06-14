using Game.Events;
using Game.InteractableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entities.Player
{
    public class PlayerInteractionController : MonoBehaviour, IEventListener
    {
        [SerializeField]
        private float _maxInteractionDistance;
        [SerializeField]
        private float _interactionInterval = 0.1f;
        [SerializeField]
        private Transform _interactionUI;

        private EntityEventController _eventController;

        private bool _hasInteractableObjectInView = false;
        private IInteractable _interactableObjectInView;        
        
        private void Awake()
        {
            _eventController = GetComponent<EntityEventController>();
        }

        private IEnumerator CheckForInteractionCoroutine()
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

            if (Physics.Raycast(ray, out RaycastHit hit, _maxInteractionDistance, 1 << Constants.InteractableLayer))
            {
                IInteractable interactableObject = hit.transform.GetComponent<IInteractable>();

                if (interactableObject != null)
                {
                    _interactionUI.gameObject.SetActive(true);
                    _hasInteractableObjectInView = true;
                    _interactableObjectInView = interactableObject;
                }
                else
                {
                    _interactionUI.gameObject.SetActive(false);
                    _hasInteractableObjectInView = false;
                }
            }
            else
            {
                _interactionUI.gameObject.SetActive(false);
                _hasInteractableObjectInView = false;
            }

            yield return new WaitForSeconds(_interactionInterval);

            StartCoroutine(CheckForInteractionCoroutine());
        }

        private void OnInteractActionPerformed(ref EntityEventContext ctx)
        {
            if (_hasInteractableObjectInView && _interactableObjectInView != null)
            {
                _interactableObjectInView.Interact();
            }
        }

        public void StartListening()
        {
            if (_eventController)
            {
                _eventController.AddListener(EntityEventType.OnInteractActionPerformed, OnInteractActionPerformed);
            }
        }

        public void StopListening()
        {
            if (_eventController)
            {
                _eventController.RemoveListener(EntityEventType.OnInteractActionPerformed, OnInteractActionPerformed);
            }
        }

        private void OnEnable()
        {
            StartCoroutine(CheckForInteractionCoroutine());
            StartListening();
        }

        private void OnDisable()
        {
            StopAllCoroutines();
            StopListening();
        }
    } 
}
