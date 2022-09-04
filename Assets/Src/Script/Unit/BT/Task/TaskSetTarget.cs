using LtBehaviorTree;

public class TaskSetTarget : Node {
    public override NodeState Tick() {
        if (GetPara(Global.BtParaMountStr) is Unit unit) {
            if (GetPara(Global.BtParaTargetStr) is Unit target) {
                unit.Target = target;
                return NodeState.Success;
            }
        }

        return NodeState.Failure;
    }
}