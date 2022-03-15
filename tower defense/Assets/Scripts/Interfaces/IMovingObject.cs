using UnityEngine;

public interface IMovingObject : IRotatingObject
{
    bool MoveTo(Vector3 pos);
    void StopMoving();
}
