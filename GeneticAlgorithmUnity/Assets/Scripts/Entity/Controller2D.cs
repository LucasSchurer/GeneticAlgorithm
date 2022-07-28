using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class Controller2D : MonoBehaviour
{
    [SerializeField]
    protected LayerMask collisionMask;

    public abstract void Move(Vector2 velocity);
}
