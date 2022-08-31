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

        protected Decorator(Node child) {
            Child = child;
        }

        protected Decorator(Node child, Node parent) : base(parent) {
            Child = child;
        }
    }
}