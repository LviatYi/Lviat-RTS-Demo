using System.Threading;
using LtBehaviorTree;
using UnityEngine;

public class TaskTrySetTargetOrDestination : Task {
    public override NodeState Tick() {
        if (Input.GetMouseButtonDown(1) && GetPara(Global.BtParaMountStr) is Collectivity collectivity) {
            Ray ray = GameController.Instance.MainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out var raycastHit, 1000f,
                    Global.UnitLayerMaskInt | Global.TerrainLayerMaskInt)) {
                foreach (Unit unit in collectivity.ToCharacterList().FilterMine()) {
                    if (unit is Character character) {
                        character.Target = raycastHit.collider.gameObject.GetComponent<Unit>();

                        if (null == character.Target) {
                            character.Destination = raycastHit.point;
                        }
                    }
                }
            }
        }

        return NodeState.Success;
    }
}