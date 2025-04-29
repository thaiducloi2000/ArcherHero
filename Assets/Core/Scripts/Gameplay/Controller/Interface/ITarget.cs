using System;
using UnityEngine;
[Serializable]
public enum AnimationType
{
    Idle = 0,
    Attack,
    Death,
    Damaged,
}
public delegate void AnimationCallback();
public interface ITarget
{
    public Vector3 Position { get; }
    public void TakeDamage(float damaged);
}
