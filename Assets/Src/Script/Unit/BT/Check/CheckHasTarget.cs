using LtBehaviorTree;

public class CheckHasTarget : Check {
    public override NodeState Tick() {
        if (GetPara(Global.BtParaMountStr) is Unit unit && unit.Target != null) {
            return NodeState.Success;
        }

        return NodeState.Failure;
    }
}