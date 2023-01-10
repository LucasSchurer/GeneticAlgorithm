using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Events
{
    public class GameEventController : EventController<GameEventType, GameEventContext>
    {
        protected override void AddEventControllerToContext(ref GameEventContext ctx)
        {
            ctx.eventController = this;
        }
    } 
}
