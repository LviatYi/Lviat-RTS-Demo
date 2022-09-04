﻿using LtBehaviorTree;

public class TaskAttack : Node {
    public override NodeState Tick() {
        if (GetPara(Global.BtParaMountStr) is Unit unit) {
            unit.Target.UnderAttack(unit.Damage);
        }

        return NodeState.Success;
    }
}