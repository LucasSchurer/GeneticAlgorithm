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
        private GameObject _valuesContainer;        
        [SerializeField]
        private TextMeshProUGUI _fitnessValue;
        [SerializeField]
        private TextMeshProUGUI _damageDealtValue;
        [SerializeField]
        private TextMeshProUGUI _damageTakenValue;
        [SerializeField]
        private TextMeshProUGUI _traitsValue;
        [SerializeField]
        private TextMeshProUGUI _hitsDealtValue;
        [SerializeField]
        private TextMeshProUGUI _hitsTakenValue;

        private PopulationGraph _graph;

        private CreatureVertex _selectedVertex;

        private CreatureVertex[] _selectedVertexParents;

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
                    foreach (CreatureVertex vertex in _graph.GetGeneration(i).Values)
                    {
                        CreatureButton creatureButton = creaturesRow.AddCreatureButton(vertex.data.id);

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
            CreatureVertex vertex = _graph.GetVertex(creatureButton.Generation, creatureButton.Id);

            if (_selectedVertex != null)
            {
                _creaturesRowManager._creaturesRows[_selectedVertex.data.generation]._creatures[_selectedVertex.data.id].GetComponent<Image>().color = Color.white;
            }

            if (_selectedVertexParents != null)
            {
                foreach (CreatureVertex parent in _selectedVertexParents)
                {
                    _creaturesRowManager._creaturesRows[parent.data.generation]._creatures[parent.data.id].GetComponent<Image>().color = Color.white;
                }
            }

            _selectedVertex = vertex;

            _creaturesRowManager._creaturesRows[_selectedVertex.data.generation]._creatures[_selectedVertex.data.id].GetComponent<Image>().color = Color.red;

            if (vertex.data.parents != null)
            {
                _selectedVertexParents = new CreatureVertex[vertex.data.parents.Length];

                for (int i = 0; i < vertex.data.parents.Length; i++)
                {
                    _selectedVertexParents[i] = _graph.GetVertex(vertex.data.generation - 1, vertex.data.parents[i]);
                }

                foreach (CreatureVertex parent in _selectedVertexParents)
                {
                    _creaturesRowManager._creaturesRows[parent.data.generation]._creatures[parent.data.id].GetComponent<Image>().color = Color.blue;
                }
            } else
            {
                _selectedVertexParents = null;
            }

            UpdateDataText(vertex, StatisticsType.DamageDealt, _damageDealtValue);
            UpdateDataText(vertex, StatisticsType.DamageTaken, _damageTakenValue);
            UpdateDataText(vertex, StatisticsType.HitsDealt, _hitsDealtValue);
            UpdateDataText(vertex, StatisticsType.HitsTaken, _hitsTakenValue);

            _fitnessValue.text = vertex.data.fitness.ToString("0.000");
            _traitsValue.text = "";

            foreach (TraitIdentifier identifier in vertex.data.traits)
            {
                _traitsValue.text += identifier.ToString() + " ";
            }
        }

        private void UpdateDataText(CreatureVertex vertex, StatisticsType dataType, TextMeshProUGUI valueField)
        {
            if (vertex.data.statistics.TryGetValue(dataType, out float value))
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
