using Game.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entities.Player
{
    public class WeaponSwap : MonoBehaviour
    {
        [SerializeField]
        private HomingOrbSpawner _damageHomingOrbSpawner;
        [SerializeField]
        private GrenadeLauncher _grenadeLauncher;
        [SerializeField]
        private Nuke _nuke;

        private int _currentWeapon = 1;

        [SerializeField]
        private float _swapTimer;
        private bool _canSwapWeapon = true;

        private void Awake()
        {
            _damageHomingOrbSpawner.enabled = true;
        }

        private void Update()
        {
            if (_canSwapWeapon)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    ChangeWeapon(1);
                }

                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    ChangeWeapon(2);
                }

                if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    ChangeWeapon(3);
                }
            }
        }

        private IEnumerator SwapCooldownCoroutine()
        {
            yield return new WaitForSeconds(_swapTimer);

            _canSwapWeapon = true;
        }

        private void ChangeWeapon(int index)
        {
            if (!_canSwapWeapon)
            {
                return;
            }

            _damageHomingOrbSpawner.enabled = false;
            _grenadeLauncher.enabled = false;
            _nuke.enabled = false;

            _canSwapWeapon = false;

            switch (index)
            {
                case 1:
                    _damageHomingOrbSpawner.enabled = true;
                    break;
                case 2:
                    _grenadeLauncher.enabled = true;
                    break;
                case 3:
                    _nuke.enabled = true;
                    break;
                default:
                    break;
            }

            StartCoroutine(SwapCooldownCoroutine());
        }
    } 
}
