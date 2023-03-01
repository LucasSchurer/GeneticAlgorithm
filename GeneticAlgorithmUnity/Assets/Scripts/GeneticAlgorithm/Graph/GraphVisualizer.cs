using Game.Entities;
using Game.Traits;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.GA.UI
{
    public class GraphVisualizer : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private CreaturesRowManager _creaturesRowManager;

        [Header("Info References")]
        [SerializeField]
        private GameObject _infoContainer;
        [SerializeField]
        private Game.UI.LabelValue _labelValuePrefab;
        [SerializeField]
        private Game.UI.LabelValue _fitnessValue;
        [SerializeField]
        private Game.UI.LabelValue _traitsValue;

        private PopulationGraph _graph;

        private CreatureData _selectedVertex;

        private CreatureData[] _selectedVertexParents;

        public void SetGraph(PopulationGraph graph)
        {
            _graph = graph;
        }

        public void UpdateUI()
        {
            _creaturesRowManager.Clear();

            for (int i = 0; i < _graph.CurrentGeneration; i++)
            {
                CreaturesRow creaturesRow = _creaturesRowManager.AddRow(i);
                if (creaturesRow)
                {
                    foreach (CreatureData vertex in _graph.GetGeneration(i).Values)
                    {
                        CreatureButton creatureButton = creaturesRow.AddCreatureButton(vertex.id);

                        if (creatureButton)
                        {
                            creatureButton.button.onClick.AddListener(() => ChangeSelectedVertex(creatureButton));
                        }
                    }
                }
            }
        }
        
        private void ChangeSelectedVertex(CreatureButton creatureButton)
        {
            CreatureData vertex = _graph.GetVertex(creatureButton.Generation, creatureButton.Id);

            if (_selectedVertex != null)
            {
                _creaturesRowManager._creaturesRows[_selectedVertex.generation]._creatures[_selectedVertex.id].GetComponent<Image>().color = Color.white;
            }

            if (_selectedVertexParents != null)
            {
                foreach (CreatureData parent in _selectedVertexParents)
                {
                    _creaturesRowManager._creaturesRows[parent.generation]._creatures[parent.id].GetComponent<Image>().color = Color.white;
                }
            }

            _selectedVertex = vertex;

            _creaturesRowManager._creaturesRows[_selectedVertex.generation]._creatures[_selectedVertex.id].GetComponent<Image>().color = Color.red;

            if (vertex.parents != null)
            {
                _selectedVertexParents = new CreatureData[vertex.parents.Length];

                for (int i = 0; i < vertex.parents.Length; i++)
                {
                    _selectedVertexParents[i] = _graph.GetVertex(vertex.generation - 1, vertex.parents[i]);
                }

                foreach (CreatureData parent in _selectedVertexParents)
                {
                    _creaturesRowManager._creaturesRows[parent.generation]._creatures[parent.id].GetComponent<Image>().color = Color.blue;
                }
            } else
            {
                _selectedVertexParents = null;
            }

            /*UpdateDataText(vertex, StatisticsType.DamageDealt, _damageDealtValue);
            UpdateDataText(vertex, StatisticsType.DamageTaken, _damageTakenValue);
            UpdateDataText(vertex, StatisticsType.HitsDealt, _hitsDealtValue);
            UpdateDataText(vertex, StatisticsType.HitsTaken, _hitsTakenValue);*/

            _fitnessValue.SetValue(vertex.fitness.ToString("0.000"));
            string traits = "";

            foreach (TraitIdentifier identifier in vertex.traits)
            {
                traits += identifier.ToString() + " ";
            }

            _traitsValue.SetValue(traits);
        }

        private void UpdateDataText(CreatureData vertex, StatisticsType dataType, TextMeshProUGUI valueField)
        {
            if (vertex.statistics.TryGetValue(dataType, out float value))
            {
                valueField.text = value.ToString("0.000");
            } else
            {
                valueField.text = "0";
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                UpdateUI();
            }
        }
    } 
}
