using System.Collections.Generic;

namespace LtBehaviorTree {
    public abstract class Decorator : Node {
        protected Node Child;

        public void AddNode(Node node) {
            Child = node;
        }

        public void RemoveNode(Node node) {
            Child = null;
        }

        protected Decorator(Node child, Node parent = null) : base(parent) {
            Child = child;
        }

        public override List<Node> GetChildren() {
            return new List<Node> { Child };
        }
    }
}