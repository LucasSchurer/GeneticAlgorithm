using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Entities.Shared
{
    public interface IEntityController
    {
        public Vector3 GetLookDirection();
        public void SetCanMove(bool canMove);
        public bool CanMove();
    } 
}
