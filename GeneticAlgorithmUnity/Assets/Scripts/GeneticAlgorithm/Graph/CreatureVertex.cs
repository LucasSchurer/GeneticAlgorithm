using System.IO;
using System.Xml.Serialization;

namespace Game.GA
{
    public class CreatureVertex
    {
        public CreatureData data;

        public CreatureVertex(CreatureController creature)
        {
            BuildData(creature);
        }

        private void BuildData(CreatureController creature)
        {
            data = creature.data;
        }
    } 
}
