using System.Collections.Generic;

namespace LtBehaviorTree {
    public class Selector : Composite {
        public Selector(List<Node> children, bool isRandom = false, Node parent = null) : base(children, isRandom,
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
                        ret = NodeState.Failure;
                        continue;
                    case NodeState.Running:
                        return NodeState.Running;
                    case NodeState.Success:
                        return NodeState.Success;
                    default:
                        return NodeState.Failure;
                }
            }

            return ret;
        }
    }
}