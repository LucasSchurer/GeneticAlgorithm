namespace Game.Events
{
    public class EntityEventContext : EventContext
    {
        private EntityEventController _eventController;
        private float _healthModifier;

        public EntityEventController EventController { get => _eventController; set => _eventController = value; }
        public float HealthModifier { get => _healthModifier; set => _healthModifier = value; }
    } 
}