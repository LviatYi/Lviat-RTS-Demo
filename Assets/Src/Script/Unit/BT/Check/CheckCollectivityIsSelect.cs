using System;
using LtBehaviorTree;

public class CheckCollectivityIsSelect : Check {
    public override NodeState Tick() {
        if (GetPara(Global.BtParaMountStr) is Collectivity collectivity) {
            switch (collectivity.IsSelected) {
                case true:
                    return NodeState.Success;
                case false:
                    return NodeState.Failure;
            }
        }

        return NodeState.Failure;
    }
}