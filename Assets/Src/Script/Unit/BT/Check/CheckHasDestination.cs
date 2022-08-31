using LtBehaviorTree;
using UnityEngine;

public class CheckHasDestination : Node {
    readonly Character _character;

    Vector3 _pos;

    public CheckHasDestination(Character character) {
        _character = character;
    }

    public override NodeState Execute() {
        if (_character.HasDest) {
            return NodeState.Success;
        }

        return NodeState.Failure;
    }
}