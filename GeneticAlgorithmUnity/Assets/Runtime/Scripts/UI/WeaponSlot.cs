using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class WeaponSlot : MonoBehaviour
    {
        [SerializeField]
        private Slider _cooldownOverlaySlider;
        [SerializeField]
        private Image _iconImage;
        [SerializeField]
        private TextMeshProUGUI _ammunitionText;

        public Slider CooldownOverlaySlider => _cooldownOverlaySlider;

        public void ChangeIconColor(Color color)
        {
            _iconImage.color = color;
        }
        
        public void ChangeAmmunition(int newValue)
        {
            if (newValue == -1)
            {
                _ammunitionText.text = "-";
            } else
            {
                _ammunitionText.text = newValue.ToString();
            }
        }
    } 
}
