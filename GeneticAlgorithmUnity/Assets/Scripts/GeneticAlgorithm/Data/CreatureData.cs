using Game.Traits;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace Game.GA
{
    [DataContract(Name = "Creature")]
    public class CreatureData
    {
        public int id;
        public int generation;
        public bool isDead = false;
                
        private TraitIdentifier[] _traits;
        private Fitness _fitness;
        public int[] parents;
        private CreatureData[] _parents;
        public List<int> children;
        private List<CreatureData> _children;
        public float[] fitnessPropertiesValues;
        public BaseEnemyChromosome chromosome;
        public TraitIdentifier[] Traits { get => _traits; set => _traits = value; }
        public Fitness Fitness { get => _fitness; set => _fitness = value; }
        public CreatureData[] Parents { get => _parents; set => _parents = value; }
        public List<CreatureData> Children => _children;
        // Only used to XML
        public List<int> ChildrenID => _children.Select(c => c.id).ToList();
        // Only used to XML
        public List<int> ParentsID => _parents != null ? _parents.Select(p => p.id).ToList() : new List<int>();

        public CreatureData()
        {
            _children = new List<CreatureData>();
            _fitness = new Fitness();
        }
    }
}
