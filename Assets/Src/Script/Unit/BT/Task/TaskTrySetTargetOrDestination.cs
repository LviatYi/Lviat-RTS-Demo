using LtBehaviorTree;
using UnityEngine;

public class TaskTrySetTargetOrDestination : Node {
    public override NodeState Tick() {
        if (Input.GetMouseButtonDown(1) && GetPara(Global.BtParaMountStr) is Collectivity collectivity) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var raycastHit, 1000f, Global.TerrainLayerMaskInt)) {
                foreach (Unit unit in collectivity.ToCharacterList()) {
                    if (unit is Character character) {
                        character.Destination = raycastHit.point;
                    }
                }
            }
        }

        return NodeState.Success;
    }
}