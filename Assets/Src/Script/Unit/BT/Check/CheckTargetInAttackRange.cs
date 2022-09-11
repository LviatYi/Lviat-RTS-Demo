using LtBehaviorTree;
using UnityEngine;

public class CheckTargetInAttackRange : Check {
    public override NodeState Tick() {
        if (GetPara(Global.BtParaMountStr) is Unit unit) {
            if (Vector3.Distance(unit.Target.transform.position, unit.transform.position) < unit.AttackRange) {
                return NodeState.Success;
            }
        }

        return NodeState.Failure;
    }
}