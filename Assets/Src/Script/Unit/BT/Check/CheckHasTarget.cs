using LtBehaviorTree;

public class CheckHasTarget : Node {
    public override NodeState Tick() {
        if (GetPara(Global.BtParaMountStr) is Unit { Target: { } }) {
            return NodeState.Success;
        }

        return NodeState.Failure;
    }
}