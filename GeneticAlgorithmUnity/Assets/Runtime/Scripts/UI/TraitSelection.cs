using Game.Events;
using Game.Managers;
using Game.Traits;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class TraitSelection : MonoBehaviour, IEventListener
    {
        [SerializeField]
        private TraitCard _traitCardPrefab;
        [SerializeField]
        private List<TraitCard> _traitCards;
        [SerializeField]
        private Image _background;

        private GameEventController _gameEventController;

        private void Awake()
        {
            _gameEventController = GameManager.Instance.GetEventController();
        }

        private void OpenTraitSelection(List<Trait<EntityEventType, EntityEventContext>> traits)
        {
            _background.enabled = true;
            GameManager.Instance.PauseGame(false);

            for (int i = 0; i < traits.Count; i++)
            {
                if (i >= _traitCards.Count)
                {
                    TraitCard traitCard = Instantiate(_traitCardPrefab, transform);
                    _traitCards.Add(traitCard);
                }

                _traitCards[i].gameObject.SetActive(true);
                _traitCards[i].SetTrait(traits[i]);
            }
        }

        public void SelectTrait(Trait<EntityEventType, EntityEventContext> trait)
        {
            CloseTraitSelection();
        }

        private void CloseTraitSelection()
        {
            _background.enabled = false;
            GameManager.Instance.ResumeGame(false);

            foreach (TraitCard traitCard in _traitCards)
            {
                traitCard.gameObject.SetActive(false);
            }
        }

        private void OnGivePlayerTraits(ref GameEventContext ctx)
        {
            int amount = GameSettings.Instance.TraitSelectionAmount;
            List<Trait<EntityEventType, EntityEventContext>> traits = new List<Trait<EntityEventType, EntityEventContext>>();

            List<TraitIdentifier> identifiers = TraitManager.Instance.GetRandomUniqueTraits(amount, TraitManager.TraitHolder.Player);

            foreach (TraitIdentifier identifier in identifiers)
            {
                Trait<EntityEventType, EntityEventContext> trait = TraitManager.Instance.GetEntityTrait(identifier, TraitManager.TraitHolder.Player);

                if (trait != null)
                {
                    traits.Add(trait);
                }
            }

            if (traits.Count > 0)
            {
                OpenTraitSelection(traits);
            }
        }

        public void StartListening()
        {
            if (_gameEventController)
            {
                _gameEventController.AddListener(GameEventType.OnGivePlayerTraits, OnGivePlayerTraits);
            }
        }

        public void StopListening()
        {
            if (_gameEventController)
            {
                _gameEventController.RemoveListener(GameEventType.OnGivePlayerTraits, OnGivePlayerTraits);
            }
        }

        private void OnEnable()
        {
            StartListening();
        }

        private void OnDisable()
        {
            StopListening();
        }
    } 
}
