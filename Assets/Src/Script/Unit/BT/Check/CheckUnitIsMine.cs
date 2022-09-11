using LtBehaviorTree;

public class CheckUnitIsMine : Check {
    public override NodeState Tick() {
        if (GetPara(Global.BtParaMountStr) is Unit unit) {
            if (unit.OwnerIndex == GameController.Instance.Own.Index) {
                return NodeState.Success;
            }
        }

        return NodeState.Failure;
    }
}