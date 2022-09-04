using System;
using LtBehaviorTree;

public class TaskRemoveTarget : Node {
    public override NodeState Tick() {
        if (GetPara(Global.BtParaMountStr) is Unit unit) {
            unit.Target = null;
        }

        return NodeState.Success;
    }
}