using System.Collections.Generic;
using System.Linq;
using LtBehaviorTree;
using UnityEngine;

public class TaskFindTargetInFOVRange : Task {
    public override NodeState Tick() {
        if (GetPara(Global.BtParaMountStr) is Unit unit) {
            Vector3 unitPosition = unit.transform.position;
            List<Collider> enemiesColliderInRange =
                Physics.OverlapSphere(unitPosition, unit.RangeOfVision, Global.UnitLayerMaskInt)
                    .Where(c => {
                        Unit unit = c.GetComponent<Unit>();
                        if (unit == null) return false;
                        return GameController.Instance.IsEnemy(unit.OwnerIndex, unit.OwnerIndex);
                    }).ToList();
            if (enemiesColliderInRange.Any()) {
                unit.Target = enemiesColliderInRange
                    .OrderBy(x => (x.transform.position - unitPosition).sqrMagnitude)
                    .First()
                    .GetComponent<Unit>();

                return NodeState.Success;
            }
        }

        return NodeState.Failure;
    }
}