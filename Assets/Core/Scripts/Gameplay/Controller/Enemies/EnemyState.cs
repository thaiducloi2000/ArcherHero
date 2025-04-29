using UnityEngine;

public abstract class EnemyState : IState
{
    protected Enemy Source;
    public EnemyState(Enemy sourceObject, object param = null)
    {
        Source = sourceObject;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void OnCheckCondition() { }
}

public class EnemyStateSpawn : EnemyState
{
    public EnemyStateSpawn(Enemy sourceObject, object param = null) : base(sourceObject, param)
    {
    }
}

public class EnemyStateMove : EnemyState
{
    public EnemyStateMove(Enemy sourceObject, object param = null) : base(sourceObject, param)
    {
    }

    public override void OnCheckCondition()
    {
        //if (Source.CurrentTarget == null) return;
        //if (Vector3.Distance(Source.Position, Source.CurrentTarget.Position) > Source.TargetStats.BaseStat.AttackRange) return;
        //Source.State.TransitionTo(typeof(EnemyStateAttack));
    }
}

public class EnemyStateMoveToTarget : EnemyState
{
    public EnemyStateMoveToTarget(Enemy sourceObject, object param = null) : base(sourceObject, param)
    {
    }

    public override void Enter()
    {
        OnCurrentEnemyTargetDeathParam param = new OnCurrentEnemyTargetDeathParam
        {
            Enemy = Source,
            OnSetTargetPositionCallback = (position) =>
            {
                Source.SetDestination(position);
            }
        };
        
        //Source.AnimatorController.Move(true);
        //if (Source.CurrentTarget != null && (!Source.CurrentTarget.IsAlive || !Source.CurrentTarget.gameObject.activeInHierarchy))
        //{
        //    GameplayManager.GameEvent.PostEvent((int)GameplayEventID.OnCurrentEnemyTargetDeath, param);
        //}
    }

    public override void Exit()
    {
        Source.AnimatorController.Move(false);
    }

    public override void OnCheckCondition()
    {
        if(ExitState()) return;
        if (Source.CurrentTarget == null) return;
        if (TranslateToAttack()) return;
    }

    private bool TranslateToAttack()
    {
        /*if (!Source.CurrentTarget.IsAlive) */return false;
        //Source.State.TransitionTo(typeof(EnemyStateAttack));
        return true;
    }

    private bool ExitState()
    {
        if (Source.PathProccess < .98f) return false;
        //Source.State.TransitionTo(typeof(EnemyStateDeath));
        return true;
    }
}

public class EnemyStateAttack : EnemyState
{
    public EnemyStateAttack(Enemy sourceObject, object param = null) : base(sourceObject, param)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Source.Stop = true;
        Source.StartAttack();
    }

    public override void Exit()
    {
        base.Exit();
        Source.EndAttack();
    }

    public override void OnCheckCondition()
    {
        /*if(Source.CurrentTarget.IsAlive)*/ return;
        //Source.State.TransitionTo(typeof(EnemyStateMoveToTarget));
    }
}

public class EnemyStateDamaged : EnemyState
{
    public EnemyStateDamaged(Enemy sourceObject, object param = null) : base(sourceObject, param)
    {
    }
}

public class EnemyStateDeath : EnemyState
{
    public EnemyStateDeath(Enemy sourceObject, object param = null) : base(sourceObject, param)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Source.AnimatorController.Alive(false);
        Source.Stop = true;
    }
}

public class EnemyStateUseAbility : EnemyState
{
    public EnemyStateUseAbility(Enemy sourceObject, object param = null) : base(sourceObject, param)
    {
    }
}
