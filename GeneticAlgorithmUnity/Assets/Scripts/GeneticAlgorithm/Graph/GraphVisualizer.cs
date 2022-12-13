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
                        CreatureButton creatureButton = creaturesRow.AddCreatureButton(vertex.Id);

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
                _creaturesRowManager._creaturesRows[_selectedVertex.Generation]._creatures[_selectedVertex.Id].GetComponent<Image>().color = Color.white;
            }

            if (_selectedVertexParents != null)
            {
                foreach (CreatureVertex parent in _selectedVertexParents)
                {
                    _creaturesRowManager._creaturesRows[parent.Generation]._creatures[parent.Id].GetComponent<Image>().color = Color.white;
                }
            }

            _selectedVertex = vertex;

            _creaturesRowManager._creaturesRows[_selectedVertex.Generation]._creatures[_selectedVertex.Id].GetComponent<Image>().color = Color.red;

            if (vertex.parents != null)
            {
                _selectedVertexParents = new CreatureVertex[vertex.parents.Length];

                for (int i = 0; i < vertex.parents.Length; i++)
                {
                    _selectedVertexParents[i] = _graph.GetVertex(vertex.Generation - 1, vertex.parents[i]);
                }

                foreach (CreatureVertex parent in _selectedVertexParents)
                {
                    _creaturesRowManager._creaturesRows[parent.Generation]._creatures[parent.Id].GetComponent<Image>().color = Color.blue;
                }
            } else
            {
                _selectedVertexParents = null;
            }

            if (vertex.statistics.baseStatistics.TryGetValue(Entities.StatisticsType.DamageDealt, out float damageDealtValue))
            {
                _damageDealtValue.text = damageDealtValue.ToString("0.000");
            } else
            {
                _damageDealtValue.text = "0";
            }

            if (vertex.statistics.baseStatistics.TryGetValue(Entities.StatisticsType.DamageDealt, out float damageTakenValue))
            {
                _damageTakenValue.text = damageTakenValue.ToString("0.000");
            }
            else
            {
                _damageTakenValue.text = "0";
            }

            if (vertex.statistics.baseStatistics.TryGetValue(Entities.StatisticsType.HitsTaken, out float hitsTakenValue))
            {
                _hitsTakenValue.text = hitsTakenValue.ToString("0.000");
            }
            else
            {
                _hitsTakenValue.text = "0";
            }

            if (vertex.statistics.baseStatistics.TryGetValue(Entities.StatisticsType.HitsDealt, out float hitsDealtValue))
            {
                _hitsDealtValue.text = hitsDealtValue.ToString("0.000");
            }
            else
            {
                _hitsDealtValue.text = "0";
            }

            _fitnessValue.text = vertex.statistics.fitness.ToString("0.000");
            _traitsValue.text = "";

            foreach (TraitIdentifier identifier in vertex.statistics.traits)
            {
                _traitsValue.text += identifier.ToString() + " ";
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
