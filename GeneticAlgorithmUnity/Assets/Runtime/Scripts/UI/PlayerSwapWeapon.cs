using Game.Entities.Player;
using Game.Events;
using Game.Weapons;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class PlayerSwapWeapon : MonoBehaviour, IEventListener
    {
        [SerializeField]
        private EntityEventController _eventController;
        [SerializeField]
        private WeaponSlot[] _weaponSlots;
        private Dictionary<WeaponType, WeaponSlot> _weapons;
        private WeaponSlot _selectedWeapon;

        private Coroutine _swapCooldownCoroutine;

        [SerializeField]
        private Color _notSelectedColor;
        [SerializeField]
        private Color _selectedColor;

        private void Start()
        {
            WeaponType[] _selectedWeapons = Managers.GameSettings.Instance.SelectedWeapons;
            _weapons = new Dictionary<WeaponType, WeaponSlot>();

            for (int i = 0; i < _selectedWeapons.Length && i < _weaponSlots.Length; i++)
            {
                if (_selectedWeapons[i] != WeaponType.None)
                {
                    _weapons.TryAdd(_selectedWeapons[i], _weaponSlots[i]);
                    _weaponSlots[i].ChangeIconColor(_notSelectedColor);

                    _weaponSlots[i].ChangeAmmunition(WeaponManager.Instance.GetWeaponData(_selectedWeapons[i], WeaponManager.WeaponHolder.Team1).Ammunition);
                }
            }

            if (_weaponSlots.Length > _weapons.Count)
            {
                int difference = _weaponSlots.Length - _weapons.Count;

                for (int i = _weaponSlots.Length - 1; i > _weaponSlots.Length - 1 - difference; i--)
                {
                    _weaponSlots[i].gameObject.SetActive(false);
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

        private void OnWeaponSwap(ref EntityEventContext ctx)
        {
            if (ctx.Weapon != null && ctx.Weapon.CurrentWeapon != WeaponType.None)
            {
                ChangeWeapon(ctx.Weapon.CurrentWeapon, ctx.Weapon.SwapCooldown);
            }
        }

        private void ChangeWeapon(WeaponType type, float cooldown)
        {
            if (_weapons.TryGetValue(type, out WeaponSlot slot))
            {
                if (_selectedWeapon != null)
                {
                    _selectedWeapon.ChangeIconColor(_notSelectedColor);
                }

                _selectedWeapon = slot;
                _selectedWeapon.ChangeIconColor(_selectedColor);

                if (_swapCooldownCoroutine != null)
                {
                    StopCoroutine(_swapCooldownCoroutine);
                }

                _swapCooldownCoroutine = StartCoroutine(SwapCooldownCoroutine(cooldown));
            }
        }

        private IEnumerator SwapCooldownCoroutine(float cooldown)
        {
            Slider[] sliders = new Slider[_weaponSlots.Length];

            for (int i = 0; i < sliders.Length; i++)
            {
                sliders[i] = _weaponSlots[i].CooldownOverlaySlider;
                sliders[i].maxValue = cooldown;
            }

            float elapsedTime = cooldown;

            while (elapsedTime > 0f)
            {
                for (int i = 0; i < sliders.Length; i++)
                {
                    sliders[i].value = elapsedTime;
                }

                elapsedTime -= Time.deltaTime;

                yield return null;
            }

            for (int i = 0; i < sliders.Length; i++)
            {
                sliders[i].value = 0f;
            }

            _swapCooldownCoroutine = null;
        }

        public void StartListening()
        {
            if (_eventController)
            {
                _eventController.AddListener(EntityEventType.OnWeaponSwap, OnWeaponSwap);
                _eventController.AddListener(EntityEventType.OnWeaponAttack, OnWeaponAttack);
            }
        }

        private void OnWeaponAttack(ref EntityEventContext ctx)
        {
            if (ctx.Weapon != null && ctx.Weapon.CurrentWeapon != WeaponType.None)
            {
                if (ctx.Weapon.CurrentWeapon != WeaponType.Pistol)
                {
                    UpdateAmmunition(ctx.Weapon.CurrentAmmunition);
                } 

                if (ctx.Weapon.Cooldown > 0)
                {
                    ShowWeaponCooldown();
                }
            }
        }

        private void UpdateAmmunition(int newValue)
        {
            _selectedWeapon.ChangeAmmunition(newValue);
        }

        private void ShowWeaponCooldown()
        {

        }

        public void StopListening()
        {
            if (_eventController)
            {
                _eventController.RemoveListener(EntityEventType.OnWeaponSwap, OnWeaponSwap);
                _eventController.RemoveListener(EntityEventType.OnWeaponAttack, OnWeaponAttack);
            }
        }
    } 
}
