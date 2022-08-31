using System.Collections.Generic;

namespace LtBehaviorTree {
    public class Parallel : Composite {
        public Parallel(List<Node> children, bool isRandom = false) : base(children) {
        }

        public Parallel(List<Node> children, Node parent, bool isRandom = false) : base(children, parent) {
        }

        public override NodeState Execute() {
            NodeState ret = NodeState.Failure;

            if (_isRandom) {
                ShuffleChildren();
            }

            foreach (Node child in Children) {
                switch (child.Execute()) {
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