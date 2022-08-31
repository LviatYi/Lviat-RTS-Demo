using LtBehaviorTree;

public class TaskAttack : Node {
    private Unit _unit;

    public TaskAttack(Unit unit) {
        _unit = unit;
    }

    public override NodeState Execute() {
        _unit.Target.UnderAttack(_unit.Damage);

        return NodeState.Success;
    }
}