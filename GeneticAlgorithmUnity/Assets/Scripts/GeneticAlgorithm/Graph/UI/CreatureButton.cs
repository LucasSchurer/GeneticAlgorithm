using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GA.UI
{
    public class CreatureButton : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private TextMeshProUGUI _idText;
        public Button button;

        private int _id;
        private int _generation;

        public int Id => _id;
        public int Generation => _generation;

        private void Awake()
        {
            button = GetComponent<Button>();
        }

        public void SetIdAndGeneration(int id, int generation)
        {
            _id = id;
            _generation = generation;
            UpdateText();
        }

        private void UpdateText()
        {
            _idText.text = _id.ToString();
        }
    }
}
