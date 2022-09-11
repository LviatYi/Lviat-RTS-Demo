using LtBehaviorTree;
using UnityEngine;
using UnityEngine.AI;

public class CheckMoving : Check {
    public override NodeState Tick() {
        if (GetPara(Global.BtParaMountStr) is Character character) {
            if (Vector3.Distance(character.transform.position, character.Agent.destination) > 0.5f) {
                return NodeState.Running;
            }
        }

        return NodeState.Failure;
    }
}