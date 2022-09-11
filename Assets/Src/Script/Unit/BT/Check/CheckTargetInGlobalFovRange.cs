using LtBehaviorTree;
using UnityEngine;

public class CheckTargetInGlobalFovRange : Check {
    public override NodeState Tick() {
        if (GetPara(Global.BtParaMountStr) is Unit unit) {
            //if (Vector3.Distance(unit.Target.transform.position, unit.transform.position) < unit.RangeOfVision) {
            //    return NodeState.Success;
            //}
            return NodeState.Success;
        }

        return NodeState.Failure;
    }
}