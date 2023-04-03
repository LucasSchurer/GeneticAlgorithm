using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game.UI
{
    public class LabelValue : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _label;
        [SerializeField]
        private TextMeshProUGUI _value;

        public void SetLabel(string label)
        {
            _label.text = label;
        }

        public void SetValue(string value)
        {
            _value.text = value;
        }
    } 
}
