using System.Collections.Generic;

namespace LtBehaviorTree {
    public class Sequence : Composite {
        public Sequence(List<Node> children, bool isRandom = false, Node parent = null) : base(children, isRandom,
            parent) {
        }

        public override NodeState Tick() {
            NodeState ret = NodeState.Success;

            if (_isRandom) {
                ShuffleChildren();
            }

            foreach (Node child in Children) {
                switch (child.Tick()) {
                    case NodeState.Failure:
                        return NodeState.Failure;
                    case NodeState.Running:
                        ret = NodeState.Running;
                        continue;
                    case NodeState.Success:
                        continue;
                    default:
                        return NodeState.Failure;
                }
            }

            return ret;
        }
    }
}