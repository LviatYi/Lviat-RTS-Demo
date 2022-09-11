using System.Collections.Generic;
using System.Linq;
using LtBehaviorTree;
using UnityEngine;

public class CheckAnyTargetInFOVRange : Check {
    public override NodeState Tick() {
        if (GetPara(Global.BtParaMountStr) is Unit unit) {
            Vector3 unitPosition = unit.transform.position;
            List<Collider> enemiesColliderInRange =
                Physics.OverlapSphere(unitPosition, unit.RangeOfVision, Global.UnitLayerMaskInt)
                    .Where(c => {
                        Unit u = c.GetComponent<Unit>();
                        if (u == null) return false;
                        return GameController.Instance.IsEnemy(u.OwnerIndex, unit.OwnerIndex);
                    }).ToList();
            if (enemiesColliderInRange.Any()) {
                Unit target = enemiesColliderInRange
                    .OrderBy(x => (x.transform.position - unitPosition).sqrMagnitude)
                    .First()
                    .GetComponent<Unit>();

                SetPara(Global.BtParaTargetStr, target);
                return NodeState.Success;
            }
        }

        return NodeState.Failure;
    }
}