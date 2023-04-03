using UnityEngine;

namespace Game.AI
{
    [System.Serializable]
    public class AvoidanceData
    {
        [SerializeField]
        private float _detectionRange = 1.5f;
        [SerializeField]
        private int _boxCastAmount = 9;
        [SerializeField]
        private float _detectionAngle = 180f;
        [SerializeField]
        private Vector3 _boxCastHalfExtents = Vector3.one * 0.45f;
        [SerializeField]
        private LayerMask _avoidanceLayerMask;

        public float DetectionRange => _detectionRange;
        public int BoxCastAmount => _boxCastAmount;
        public float DetectionAngle => _detectionAngle;
        public Vector3 BoxCastHalfExtents => _boxCastHalfExtents;
        public LayerMask AvoidanceLayerMask => _avoidanceLayerMask;

        public Vector3 GetUpdatedDirection(Transform agentTransform, Vector3 direction)
        {
            Vector3 updatedDirection = direction;

            RaycastHit[] hits = DebugBoxCastAll(agentTransform, direction);

            if (IsHittingOther(hits, agentTransform))
            {
                float angleStep = _detectionAngle / _boxCastAmount;

                for (int i = 1; i < _boxCastAmount + 1; i++)
                {
                    float minAngle = -(i * angleStep);
                    Vector3 minDirection = (Quaternion.Euler(0, minAngle, 0) * direction).normalized;

                    RaycastHit[] minHits = DebugBoxCastAll(agentTransform, minDirection);
                    if (IsHittingSelfOrNone(minHits, agentTransform))
                    {
                        updatedDirection = minDirection;
                        break;
                    }

                    float maxAngle = (i * angleStep);
                    Vector3 maxDirection = (Quaternion.Euler(0, maxAngle, 0) * direction).normalized;

                    RaycastHit[] maxHits = DebugBoxCastAll(agentTransform, maxDirection);
                    if (IsHittingSelfOrNone(maxHits, agentTransform))
                    {
                        updatedDirection = maxDirection;
                        break;
                    }
                }
            }

            return updatedDirection;
        }

        public RaycastHit[] BoxCastAll(Vector3 position, Quaternion orientation, Vector3 direction)
        {
            RaycastHit[] hits = Physics.BoxCastAll(position, _boxCastHalfExtents, direction, orientation, _detectionRange, _avoidanceLayerMask);

            return hits;
        }
        
        public RaycastHit[] DebugBoxCastAll(Transform origin, Vector3 direction)
        {
            RaycastHit[] hits = Physics.BoxCastAll(origin.position, _boxCastHalfExtents, direction, origin.rotation, _detectionRange, _avoidanceLayerMask);

            ExtDebug.DrawBoxCastBox(origin.position, _boxCastHalfExtents, origin.rotation, direction, _detectionRange, IsHittingOther(hits, origin) ? Color.red : Color.green);            

            return hits;
        }

        public bool IsHittingOther(RaycastHit[] hits, Transform origin)
        {
            if (hits.Length > 1 || (hits.Length == 1 && hits[0].transform != origin))
            {
                return true;
            } else
            {
                return false;
            }
        }

        public bool IsHittingSelfOrNone(RaycastHit[] hits, Transform origin)
        {
            if (hits.Length == 0 || (hits.Length == 1 && hits[0].transform == origin))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    } 
}
