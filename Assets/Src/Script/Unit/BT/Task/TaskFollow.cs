using LtBehaviorTree;

public class TaskFollow : Node {
    public override NodeState Tick() {
        if (GetPara(Global.BtParaMountStr) is Character character) {
            character.Destination = character.Target.transform.position;
        }

        return NodeState.Success;
    }
}