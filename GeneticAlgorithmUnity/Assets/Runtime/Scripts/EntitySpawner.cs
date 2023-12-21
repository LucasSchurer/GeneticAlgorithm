using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntitySpawner : MonoBehaviour
{
    public abstract Transform[] GetEntities();
}
