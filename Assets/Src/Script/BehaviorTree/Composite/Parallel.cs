using System.Collections.Generic;

namespace LtBehaviorTree {
    public class Parallel : Composite {
        public Parallel(List<Node> children, bool isRandom = false, Node parent = null) : base(children, isRandom,
            parent) {
        }

        public override NodeState Tick() {
            NodeState ret = NodeState.Failure;

            if (_isRandom) {
                ShuffleChildren();
            }

            foreach (Node child in Children) {
                switch (child.Tick()) {
                    case NodeState.Failure:
                        return NodeState.Failure;
                    case NodeState.Running:
                        ret |= NodeState.Running;
                        continue;
                    case NodeState.Success:
                        ret |= NodeState.Success;
                        continue;
                    default:
                        return NodeState.Failure;
                }
            }

            return (ret & NodeState.Running) == NodeState.Running ? NodeState.Running : NodeState.Success;
        }
    }
}