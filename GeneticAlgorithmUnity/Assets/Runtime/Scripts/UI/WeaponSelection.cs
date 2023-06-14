using Game.Weapons;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class WeaponSelection : MonoBehaviour
    {
        [SerializeField]
        private TypeCard[] _weapons;

        [System.Serializable]
        public class TypeCard
        {
            public WeaponType type;
            public Button button;
        }

        public Menu menu;

        private int _choosingNumber = 1;

        private WeaponType[] _selectedWeapons = new WeaponType[3] { WeaponType.Pistol, WeaponType.None, WeaponType.None};

        [SerializeField]
        private TextMeshProUGUI _text;

        private void Awake()
        {
            foreach (TypeCard card in _weapons)
            {
                card.button.onClick.AddListener(() => Select(card.type));
            }
        }

        public void Enable()
        {
            _text.text = "Selecione sua segunda arma";
        }

        public void Select(WeaponType type)
        {
            if (_choosingNumber == 1)
            {
                _selectedWeapons[_choosingNumber] = type;

                for (int i = 0; i < _weapons.Length; i++)
                {
                    if (_weapons[i].type == type)
                    {
                        _weapons[i].button.enabled = false;
                    }
                }

                _text.text = "Selecione sua terceira arma";
                _choosingNumber++;

            } else if (_choosingNumber == 2)
            {
                _selectedWeapons[_choosingNumber] = type;

                Managers.GameSettings.Instance.SetSelectedWeapons(_selectedWeapons);

                menu.GoToMainMap();
            }
        }
    } 
}
