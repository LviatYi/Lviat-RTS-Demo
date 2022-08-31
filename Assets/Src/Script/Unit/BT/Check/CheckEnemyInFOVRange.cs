using System.Collections.Generic;
using System.Linq;
using LtBehaviorTree;
using UnityEngine;

public class CheckEnemyInFOVRange : Node {
    readonly Unit _unit;

    Vector3 _pos;

    public CheckEnemyInFOVRange(Unit unit) {
        _unit = unit;
    }

    public override NodeState Execute() {
        _pos = _unit.transform.position;
        List<Collider> enemiesColliderInRange =
            Physics.OverlapSphere(_pos, _unit.RangeOfVision, Global.UnitLayerMaskInt)
                .Where(c => {
                    Unit unit = c.GetComponent<Unit>();
                    if (unit == null) return false;
                    return GameController.Instance.IsEnemy(_unit.Owner, unit.Owner);
                }).ToList();
        if (enemiesColliderInRange.Any()) {
            _unit.Target = enemiesColliderInRange
                .OrderBy(x => (x.transform.position - _pos).sqrMagnitude)
                .First()
                .GetComponent<Unit>();

            State = NodeState.Success;
        }
        else {
            State = NodeState.Failure;
        }

        return State;
    }
}