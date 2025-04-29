using System;
using System.Collections.Generic;
public class NM_Enemy : Enemy
{
    bool isAtDestination = false;

    private void Awake()
    {
        //Dictionary<Type, IState> tmp = new Dictionary<Type, IState>();
        //tmp.Add(typeof(EnemyStateSpawn), new EnemyStateSpawn(this));
        //tmp.Add(typeof(EnemyStateDeath), new EnemyStateDeath(this));
        //tmp.Add(typeof(EnemyStateUseAbility), new EnemyStateUseAbility(this));
        //tmp.Add(typeof(EnemyStateDamaged), new EnemyStateDamaged(this));
        //tmp.Add(typeof(EnemyStateAttack), new EnemyStateAttack(this));
        //tmp.Add(typeof(EnemyStateMoveToTarget), new EnemyStateMoveToTarget(this));
        //State = new StateController(tmp);
        base.selector = new EnemyTargetSelector(this);
    }
    private void OnEnable()
    {
        isAtDestination = false;
        //State.TransitionTo(typeof(EnemyStateMoveToTarget));
    }

    private void Update()
    {
        //State.Update();
        FindTarget();
        if (isAtDestination) return;
        if (PathProccess < .98f) return;
        {
            isAtDestination = true;
            Stop = true;
        }
    }

    public override void FindTarget()
    {
        //if (CurrentTarget != null) return;
        //CurrentTarget = selector.FindTarget() as Chess;
        //if (CurrentTarget == null) return;
    }
    public override void Setup(EnemyData data)
    {
        base.Setup(data);
        Set(data);
        isAtDestination = false;
        IsAlive = true;
    }
    public override void TakeDamage(float damaged)
    {
        base.TakeDamage(damaged);
        OnDeath();
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        IsAlive = false;
        //GameplayManager.GameEvent.PostEvent((int)GameplayEventID.EnemyRelease, this);
    }

    protected override void Attack()
    {
        //base.Attack();
        //if(!CurrentTarget.IsAlive || !CurrentTarget.gameObject.activeInHierarchy)
        //{
        //    State.TransitionTo(typeof(EnemyStateMoveToTarget));
        //    CurrentTarget = null;
        //}
    }
}
