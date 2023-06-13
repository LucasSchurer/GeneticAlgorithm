using System.Collections;
using System.Collections.Generic;
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

        public Slider CooldownOverlaySlider => _cooldownOverlaySlider;

        public void ChangeIconColor(Color color)
        {
            _iconImage.color = color;
        }
    } 
}
