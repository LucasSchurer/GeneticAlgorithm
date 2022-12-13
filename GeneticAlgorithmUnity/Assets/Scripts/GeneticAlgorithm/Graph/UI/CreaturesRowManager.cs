using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.GA.UI
{
    public class CreaturesRowManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private CreaturesRow _creaturesRowPrefab;

        public List<CreaturesRow> _creaturesRows;

        private void Awake()
        {
            _creaturesRows = new List<CreaturesRow>();
        }

        public CreaturesRow AddRow(int generation)
        {
            if (generation >= _creaturesRows.Count)
            {
                CreaturesRow creaturesRow = Instantiate(_creaturesRowPrefab, transform);
                creaturesRow.SetGeneration(generation);
                _creaturesRows.Add(creaturesRow);
                return creaturesRow;
            } else
            {
                return null;
            }
        }

        public void Clear()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }

            _creaturesRows.Clear();
        }
    } 
}
