using LtBehaviorTree;

public class TaskFollow : Node {
    private Unit _unit;

    public TaskFollow(Unit unit) {
        _unit = unit;
    }

    public override NodeState Execute() {
        _unit.TargetPosition = _unit.Target.transform.position;

        return NodeState.Success;
    }
}