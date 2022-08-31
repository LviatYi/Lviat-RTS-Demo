using LtBehaviorTree;
using UnityEngine;

public class CheckTargetInAttackRange : Node {
    private Unit _unit;

    public CheckTargetInAttackRange(Unit unit) {
        _unit = unit;
    }


    public override NodeState Execute() {
        if (Vector3.Distance(_unit.Target.transform.position, _unit.transform.position) < _unit.AttackRange) {
            return NodeState.Success;
        }

        return NodeState.Failure;
    }
}