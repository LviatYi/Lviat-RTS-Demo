using System;
using System.Threading.Tasks;
using LtBehaviorTree;

public class TaskMoveTo : Node {
    private Character _character;


    public TaskMoveTo(Character character) {
        _character = character;
    }

    public override NodeState Execute() {
        if (_character.MoveTo(_character.TargetPosition)) {
            return NodeState.Running;
        }

        return NodeState.Failure;
    }
}