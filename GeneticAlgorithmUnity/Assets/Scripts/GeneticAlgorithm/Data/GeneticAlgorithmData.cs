using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

namespace Game.GA
{
    /// <summary>
    /// Contains all the information about
    /// the genetic algorithm data, 
    /// such as generations, creatures, genes,
    /// and statistics.
    /// Used to build the XML file.
    /// </summary>
    [XmlRoot("Genetic_Algorithm")]
    public class GeneticAlgorithmData
    {
        private string xmlFileName = "";
        [XmlAttribute]
        public int version = 1;

        [XmlArray("Generations")]
        [XmlArrayItem("Generation")]
        public GenerationData[] generations;

        public GeneticAlgorithmData() { }
        public GeneticAlgorithmData(GenerationData[] generations)
        {
            this.generations = generations;
        }

        public void ToXML()
        {
            if (xmlFileName == "")
            {
                xmlFileName = $"{DateTime.Now.ToString("yyyyMMdd-HHmmss")}.xml";
            }

            XMLHelper.SerializeData($"{Application.persistentDataPath}/{xmlFileName}", this, false);
        }
    } 
}
