using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entities.Shared
{
    public class EntitySocketController : MonoBehaviour
    {
        [Tooltip("Duplicated socket type will be ignored. Will only be used if assigned before Awake()")]
        [SerializeField]
        private List<Socket> _socketList;
        private Dictionary<EntitySocketType, Transform> _sockets;

        private void Awake()
        {
            _sockets = new Dictionary<EntitySocketType, Transform>();

            if (_socketList != null)
            {
                foreach (Socket socket in _socketList)
                {
                    _sockets.TryAdd(socket.Type, socket.Transform);
                }
            }
        }

        public Transform GetSocket(EntitySocketType type)
        {
            if (_sockets.TryGetValue(type, out Transform socketTransform))
            {
                return socketTransform;
            } else
            {
                Debug.LogError("Socket Type not defined in creature");
                return null;
            }
        }


        [System.Serializable]
        private struct Socket
        {
            [SerializeField]
            private EntitySocketType _type;
            [SerializeField]
            private Transform _transform;

            public EntitySocketType Type => _type;
            public Transform Transform => _transform;
        }
    } 
}
