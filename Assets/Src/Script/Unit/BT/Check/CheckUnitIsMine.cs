using LtBehaviorTree;

public class CheckUnitIsMine : Node {
    public override NodeState Tick() {
        if (GetPara(Global.BtParaMountStr) is Unit unit) {
            if (unit.OwnerIndex == GameController.Instance.Own.Index) {
                return NodeState.Success;
            }
        }

        return NodeState.Failure;
    }
}