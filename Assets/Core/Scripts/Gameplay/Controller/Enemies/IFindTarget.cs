using UnityEngine;

public interface ITargetSelector
{
    public ITarget FindTarget();
}

public abstract class TargetSelector : ITargetSelector
{
    public abstract ITarget FindTarget();
}

public class EnemyTargetSelector : TargetSelector
{
    protected Enemy source;
    public EnemyTargetSelector(Enemy enemy)
    {
        source = enemy;
    }
    public override ITarget FindTarget()
    {
        //var objs = Physics.OverlapSphere(source.transform.position, source.TargetStats.BaseStat.AttackRange, source.TowerLayer, QueryTriggerInteraction.Ignore);
        //if (objs == null || objs.Length <= 0) return null;
        //ITarget target;
        //Chess chess;
        //foreach (var obj in objs)
        //{
        //    target = obj.GetComponent<ITarget>();
        //    if (target == null) continue;
        //    chess =  target as Chess;
        //    if(chess == null) continue;
            
        //    if(source.Data.GridPosition.Y != chess.CurrentPosition.Y) continue;
        //    return target;
        //}
        return null;
    }
}
