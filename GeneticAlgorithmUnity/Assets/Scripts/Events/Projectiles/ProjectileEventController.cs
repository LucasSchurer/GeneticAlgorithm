using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles all the events of a specific entity
/// Including method register/unregister and 
/// event invokes
/// </summary>
namespace Game.Events
{
    public class ProjectileEventController : EventController<ProjectileEventType, ProjectileEventContext>
    {
    }
}
