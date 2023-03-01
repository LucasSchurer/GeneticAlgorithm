using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using UnityEngine;

namespace Game.GA
{
    public class PopulationGraph
    {
        private string xmlFileName = "";

        private List<Dictionary<int, CreatureData>> _creatures;

        public int CurrentGeneration => _creatures.Count;

        public PopulationGraph()
        {
            _creatures = new List<Dictionary<int, CreatureData>>();
        }

        public void CreateAndAddVertex(CreatureController creature)
        {
            AddVertex(creature.data);
        }

        public void AddVertex(CreatureData data)
        {
            if (data.generation >= _creatures.Count)
            {
                _creatures.Add(new Dictionary<int, CreatureData>());
            }

            _creatures[data.generation].TryAdd(data.id, data);
        }

        public Dictionary<int, CreatureData> GetGeneration(int generation)
        {
            if (generation < _creatures.Count)
            {
                return _creatures[generation];
            } else
            {
                return null;
            }
        }

        public CreatureData GetVertex(int generation, int id)
        {
            return _creatures[generation][id];
        }

        public void ToXML()
        {
            if (xmlFileName == "")
            {
                xmlFileName = $"{DateTime.Now.ToString("yyyyMMdd-HHmmss")}.xml";
            }

            CreatureData[] vertices = _creatures[0].Values.ToArray();

            foreach (CreatureData data in vertices)
            {
                XMLHelper.SerializeData($"{Application.persistentDataPath}/{xmlFileName}", data);
            }
        }
    } 
}
