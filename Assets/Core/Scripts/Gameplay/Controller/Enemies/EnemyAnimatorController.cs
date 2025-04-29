using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorController : MonoBehaviour
{
    [SerializeField] private Animator m_animator;
    public readonly int _animIDIsAttack = Animator.StringToHash("IsAttack");
    public readonly int _animIDIsAlive = Animator.StringToHash("IsAlive");
    public readonly int _animIDIsMove = Animator.StringToHash("IsMove");
    public readonly int _animIDSpeed = Animator.StringToHash("Speed");

    #region Callback from

    private Dictionary<AnimationType, AnimationCallback> m_callbacks =
        new Dictionary<AnimationType, AnimationCallback>();

    #endregion

    public bool m_HasAnimator;

    private void Awake()
    {
        Setup();
        m_HasAnimator = m_animator != null;
    }

    public void Attack(bool HasTarget)
    {
        m_animator.SetBool(_animIDIsAttack, HasTarget);
    }

    public void SetSpeed(float speed = 1)
    {
        m_animator.SetFloat(_animIDSpeed, speed);
    }

    public void Move(bool IsMove)
    {
        m_animator.SetBool(_animIDIsMove, IsMove);
    }

    public void Alive(bool IsAlive)
    {
        m_animator.SetBool(_animIDIsAlive, IsAlive);
    }

    private void Setup()
    {
        m_animator = GetComponent<Animator>();
    }

    #region Animation Call Back

    private void OnAnimationCallBack(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            AnimationType type = (AnimationType)animationEvent.intParameter;
            if (!m_callbacks.ContainsKey(type)) return;
            m_callbacks[type]?.Invoke();
        }
    }

    public void Register(AnimationType type, AnimationCallback Callback)
    {
        // check if listener exist in distionary
        if (m_callbacks.ContainsKey(type))
        {
            // add callback to our collection
            m_callbacks[type] += Callback;
            return;
        }

        m_callbacks.Add(type, null);
        m_callbacks[type] += Callback;
    }

    public void Remove(AnimationType type, AnimationCallback Callback)
    {
        if (!m_callbacks.ContainsKey(type)) return;
        m_callbacks[type] -= Callback;
    }

    #endregion

    private void Clear()
    {
        m_callbacks.Clear();
    }

    private void OnDisable()
    {
        Clear();
    }
}