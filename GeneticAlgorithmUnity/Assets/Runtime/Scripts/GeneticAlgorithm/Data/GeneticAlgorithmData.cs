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

        [DataMember(Name = "Team")]
        public int team;

        [DataMember(Name = "FitnessProperties")]
        public FitnessProperty[] properties;

        [DataMember(Name = "TraitSelectionAmount")]
        public int traitSelectionAmount;

        [DataMember(Name = "TraitSelectionDumbness")]
        public float traitSelectionDumbness;

        [DataMember(Name = "Generations")]
        public GenerationData[] generations;

        [DataMember(Name = "TraitChangePositiveThreshold")]
        public float traitChangePositiveThreshold;
        [DataMember(Name = "TraitChangeNegativeThreshold")]
        public float traitChangeNegativeThreshold;
        [DataMember(Name = "TraitChangeAmount")]
        public float traitChangeAmount;

        [DataMember(Name = "MutationRate")]
        public float mutationRate;

        [DataMember(Name = "MutateIndividually")]
        public bool mutateIndividually;

        [DataMember(Name = "ParentsAmount")]
        public int parentsAmount;

        [DataMember(Name = "SelectionMethod")]
        public GeneticAlgorithmController.SelectionMethod selectionMethod;

        [DataMember(Name = "TournamentSize")]
        public int tournamentSize;

        [DataMember(Name = "Elitism")]
        public int elitism;

        [DataMember(Name = "GenerationsNeededToAddTrait")]
        public int generationsNeededToAddTrait;

        [DataMember(Name = "CloneEverything")]
        public bool cloneEverything;

        [DataMember(Name = "AddTraitsToCloneEverything")]
        public bool addTraitsToCloneEverything;

        [DataMember(Name = "MaxTraitAmount")]
        public int maxTraitAmount;

        public void ToXML()
        {
            if (xmlFileName == "")
            {
                xmlFileName = $"{DateTime.Now.ToString("yyyyMMdd-HHmmss")} - {team}.xml";
            }

            XMLHelper.SerializeData($"{Application.persistentDataPath}/{xmlFileName}", this, false);
        }
    } 
}
