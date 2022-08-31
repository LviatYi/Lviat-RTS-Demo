using System.Collections.Generic;

namespace LtBehaviorTree {
    public class Selector : Composite {
        public Selector(List<Node> children, bool isRandom = false) : base(children) {
        }

        public Selector(List<Node> children, Node parent, bool isRandom = false) : base(children, parent) {
        }

        public override NodeState Execute() {
            NodeState ret = NodeState.Success;

            if (_isRandom) {
                ShuffleChildren();
            }

            foreach (Node child in Children) {
                switch (child.Execute()) {
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