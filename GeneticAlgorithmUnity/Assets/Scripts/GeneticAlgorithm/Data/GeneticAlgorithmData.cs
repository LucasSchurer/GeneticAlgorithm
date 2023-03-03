using System;
using System.Runtime.Serialization;
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
    [DataContract(Name = "GeneticAlgorithm")]
    public class GeneticAlgorithmData
    {
        private string xmlFileName = "";
        private int version = 1;

        [DataMember]
        public GenerationData[] generations;

        public GeneticAlgorithmData() { }        

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
