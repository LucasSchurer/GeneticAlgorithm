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
    [DataContract(Name = "GeneticAlgorithm", Namespace = "")]
    public class GeneticAlgorithmData
    {
        private string xmlFileName = "";
        [DataMember(Name = "Version")]
        private readonly int version = 1;

        [DataMember(Name = "Generations")]
        public GenerationData[] generations;    

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
