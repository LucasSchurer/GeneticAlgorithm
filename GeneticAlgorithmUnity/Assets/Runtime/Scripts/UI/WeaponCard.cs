using Game.Weapons;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class WeaponCard : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _name;
        [SerializeField]
        private TextMeshProUGUI _description;

        public void SetWeapon(WeaponData data)
        {
            _name.text = data.Name;
            _description.text = data.Description;
        }

        public void Select()
        {

        }
    } 
}
