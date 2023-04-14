using UnityEngine;

namespace Game.Events
{
    public class EntityEventContext : EventContext
    {
        private EntityEventController _eventController;
        private float _healthModifier;
        private Vector3 _origin;
        private Vector3 _direction;

        public EntityEventController EventController { get => _eventController; set => _eventController = value; }
        public float HealthModifier { get => _healthModifier; set => _healthModifier = value; }
        public Vector3 Origin { get => _origin; set => _origin = value; }
        public Vector3 Direction { get => _direction; set => _direction = value; }
    } 
}
