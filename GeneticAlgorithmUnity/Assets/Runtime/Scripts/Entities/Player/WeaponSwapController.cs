using Game.Events;
using Game.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entities.Player
{
    public class WeaponSwapController : MonoBehaviour, IEventListener
    {
        [SerializeField]
        private Dictionary<WeaponType, IComponent> _weapons;
        private WeaponType[] _weaponsArray;

        private WeaponType _currentWeaponType = WeaponType.None;
        private IComponent _currentWeaponComponent;

        [SerializeField]
        private float _swapCooldown;
        private bool _canSwapWeapon = true;

        private EntityEventController _eventController;

        public float SwapCooldown => _swapCooldown;

        private void Awake()
        {
            _eventController = GetComponent<EntityEventController>();    
        }

        private void Start()
        {
            SetWeapons();
            ChangeWeapon(0);
        }

        private void SetWeapons()
        {
            _weapons = new Dictionary<WeaponType, IComponent>();
            _weaponsArray = new WeaponType[3];

            WeaponType[] selectedWeapons = Managers.GameSettings.Instance.SelectedWeapons;

            for (int i = 0; i < selectedWeapons.Length && i < 3; i++)
            {
                IComponent component = WeaponManager.Instance.AddWeaponComponent(gameObject, selectedWeapons[i], WeaponManager.WeaponHolder.Team1);

                if (component != null)
                {
                    component.Disable();

                    _weapons.TryAdd(selectedWeapons[i], component);
                    _weaponsArray[i] = selectedWeapons[i];
                } else
                {
                    _weaponsArray[i] = WeaponType.None;
                }
            }
        }

        private IEnumerator SwapCooldownCoroutine()
        {
            _canSwapWeapon = false;

            yield return new WaitForSeconds(_swapCooldown);

            _canSwapWeapon = true;
        }

        private void ChangeWeapon(int index)
        {
            if (!_canSwapWeapon)
            {
                return;
            }

            WeaponType desiredWeapon = _weaponsArray[index];

            if (desiredWeapon == _currentWeaponType || desiredWeapon == WeaponType.None)
            {
                return;
            }

            if (_weapons.TryGetValue(desiredWeapon, out IComponent desiredWeaponComponent))
            {
                if (desiredWeaponComponent != null)
                {
                    if (_currentWeaponComponent != null)
                    {
                        _currentWeaponComponent.Disable();
                    }

                    _currentWeaponType = desiredWeapon;
                    _currentWeaponComponent = desiredWeaponComponent;
                    desiredWeaponComponent.Enable();

                    _eventController.TriggerEvent(EntityEventType.OnWeaponSwap, new EntityEventContext() { Weapon = new EntityEventContext.WeaponPacket() { CurrentWeapon = _currentWeaponType, SwapCooldown = _swapCooldown } });
                    StartCoroutine(SwapCooldownCoroutine());
                }
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

        private void OnSwap1Performed(ref EntityEventContext ctx)
        {
            ChangeWeapon(0);
        }

        private void OnSwap2Performed(ref EntityEventContext ctx)
        {
            ChangeWeapon(1);
        }

        private void OnSwap3Performed(ref EntityEventContext ctx)
        {
            ChangeWeapon(2);
        }

        public void StartListening()
        {
            if (_eventController)
            {
                _eventController.AddListener(EntityEventType.OnSwap1Performed, OnSwap1Performed);
                _eventController.AddListener(EntityEventType.OnSwap2Performed, OnSwap2Performed);
                _eventController.AddListener(EntityEventType.OnSwap3Performed, OnSwap3Performed);
            }
        }

        public void StopListening()
        {
            if (_eventController)
            {
                _eventController.RemoveListener(EntityEventType.OnSwap1Performed, OnSwap1Performed);
                _eventController.RemoveListener(EntityEventType.OnSwap2Performed, OnSwap2Performed);
                _eventController.RemoveListener(EntityEventType.OnSwap3Performed, OnSwap3Performed);
            }
        }
    } 
}
