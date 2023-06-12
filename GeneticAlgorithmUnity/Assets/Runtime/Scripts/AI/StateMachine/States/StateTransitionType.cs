using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.AI.States
{
    public enum StateTransitionType
    {
        Default,
        // One Opportunity transitions will try to transition only once, making the transition blocked for the current state after one try
        OneOpportunity,
    } 
}
