using System.Collections.Generic;

namespace LtBehaviorTree {
    public class Sequence : Composite {
        public Sequence(List<Node> children, bool isRandom = false) : base(children) {
        }

        public Sequence(List<Node> children, Node parent, bool isRandom = false) : base(children, parent) {
        }

        public override NodeState Execute() {
            NodeState ret = NodeState.Success;

            if (_isRandom) {
                ShuffleChildren();
            }

            foreach (Node child in Children) {
                switch (child.Execute()) {
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