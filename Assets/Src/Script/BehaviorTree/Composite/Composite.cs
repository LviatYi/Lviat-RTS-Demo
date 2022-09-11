using System.Collections.Generic;
using System.Linq;

namespace LtBehaviorTree {
    public abstract class Composite : Node {
        protected List<Node> Children;
        protected bool _isRandom;

        public void AddNode(Node node) {
            Children.Add(node);
        }

        public void RemoveNode(Node node) {
            Children.Remove(node);
        }

        protected Composite(List<Node> children, bool isRandom = false, Node parent = null) : base(parent) {
            Children = children;
            _isRandom = isRandom;
            foreach (Node child in children) {
                child.Parent = this;
            }
        }

        protected void ShuffleChildren() {
            System.Random r = new System.Random();
            Children = Children.OrderBy(_ => r.Next()).ToList();
        }

        public override List<Node> GetChildren() {
            return Children;
        }
    }
}