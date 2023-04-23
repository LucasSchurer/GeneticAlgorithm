using UnityEngine;

namespace Game.Events
{
    public class EntityEventContext : EventContext
    {
        private EntityEventController _eventController;

        private MovementPacket _movementPacket;

        private float _healthModifier;

        public EntityEventController EventController { get => _eventController; set => _eventController = value; }
        public MovementPacket Movement { get => _movementPacket; set => _movementPacket = value; }

        public float HealthModifier { get => _healthModifier; set => _healthModifier = value; }

        public class MovementPacket
        {
            private bool _isMoving = false;
            private Vector3 _movingDirection = Vector3.zero;
            private Vector3 _lookDirection = Vector3.zero;
            private float _movementAimPenalty = 0f;

            public bool IsMoving { get => _isMoving; set => _isMoving = value; }
            public Vector3 MovingDirection { get => _movingDirection; set => _movingDirection = value; }
            public Vector3 LookDirection { get => _lookDirection; set => _lookDirection = value; }
            public float MovementAimPenalty { get => _movementAimPenalty; set => _movementAimPenalty = value; }
        }
    }
}
