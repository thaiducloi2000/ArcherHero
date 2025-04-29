using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

[Serializable]
public struct EnemyData : IBusdata<Enemy>
{
    /// <summary>
    /// Nav Mesh Setting
    /// </summary>
    public float Speed;

    public int AngularSpeed;
    public float Acceleration;

    /// <summary>
    /// Stat Setting
    /// </summary>

    #region Base Stat

    //public StatData stat;

    #endregion

    #region Advanced Stat

    //public AdvancedStatData advancedStat;

    #endregion

    #region Action Callback
    public Action<Enemy> OnEnemyAtTargetCallback;
    #endregion

    #region World Data

    //public Position GridPosition;
    #endregion
}

public struct OnCurrentEnemyTargetDeathParam : IBusdata<OnCurrentEnemyTargetDeathParam>
{
    public Enemy Enemy;
    public Action<Vector3> OnSetTargetPositionCallback;
}


public enum EnemyType
{
    NE = 0,
    EE,
    BE,
}

[RequireComponent(typeof(NavMeshAgent))]
public abstract class Enemy : Agent, ITarget, IBusdata<Enemy>
{
    public EnemyData Data { get; protected set; }
    private IObjectPool<Enemy> _pool;
    //private EffectController m_EffectController;
    protected EnemyTargetSelector selector;
    public ITarget CurrentTarget { get; protected set; }
    //public StateController State { get; protected set; }
    [field: SerializeField] public EnemyAnimatorController AnimatorController { get; protected set; }
    [field: SerializeField] public LayerMask TowerLayer { get; protected set; }
    [field: SerializeField] public EnemyType Type { get; protected set; }
    public event Action OnDeadCallback;
    public IObjectPool<Enemy> Pool
    {
        get => _pool;
        set
        {
            if (_pool == null)
            {
                _pool = value;
            }
        }
    }

    public bool IsAlive { get; protected set; }

    public Vector3 Position => transform.position;
    //public EffectController EffectController => m_EffectController;
    //protected Stats m_Stat;
    //public virtual Stats TargetStats => m_Stat;

    public virtual void Setup(EnemyData data)
    {
        Data = data;
        //m_Stat = new Stats(data.stat, data.advancedStat);
        AnimatorController.Register(AnimationType.Attack, Attack);
        OnDeadCallback = null;
    }

    public virtual void TakeDamage(float damaged)
    {
        //GameplayManager.GameEvent.PostEvent((int)GameplayEventID.OnEnemyDamaged,
        //    new EnemyDamagedData(damaged));
    }

    protected virtual void OnDeath()
    {
        //EnemyDeathReward reward = new EnemyDeathReward(transform.position);
        //GameplayManager.GameEvent.PostEvent((int)GameplayEventID.OnEnemyDeath, reward);
        Stop = true;
        IsAlive = false;
        AutoRelease();
        AnimatorController.Alive(IsAlive);
    }

    public abstract void FindTarget();

    public virtual void StartAttack()
    {
        AnimatorController.Attack(true);
    }

    public virtual void EndAttack()
    {
        AnimatorController.Attack(false);
    }

    protected virtual void Attack()
    {
        //CurrentTarget.TakeDamage(Data.stat.Attack);
    }

    private void AutoRelease()
    {
        AnimatorController.Remove(AnimationType.Attack, Attack);
        CurrentTarget = null;
        Pool.Release(this);
        OnDeadCallback?.Invoke();
    }
}