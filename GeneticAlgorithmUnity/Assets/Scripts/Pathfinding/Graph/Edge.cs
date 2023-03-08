namespace Game.Pathfinding
{
    public class Edge
    {
        private Vertex _source;
        private Vertex _target;

        public Vertex Source => _source;
        public Vertex Target => _target;

        public Edge(Vertex source, Vertex target)
        {
            _source = source;
            _target = target;
        }
    }
}
