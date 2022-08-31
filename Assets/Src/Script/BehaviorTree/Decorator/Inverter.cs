using System.Collections.Generic;

namespace LtBehaviorTree {
    public class Inverter : Decorator {
        public Inverter(Node child) : base(child) {
        }

        public Inverter(Node child, Node parent) : base(child, parent) {
        }

        public override NodeState Execute() {
            var ret = Child?.Execute() switch {
                NodeState.Running => NodeState.Running,
                NodeState.Success => NodeState.Failure,
                _ => NodeState.Success
            };

            return ret;
        }
    }
}