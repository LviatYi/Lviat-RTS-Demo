namespace LtBehaviorTree {
    public abstract class Tree {
        protected Node _root;

        public Node Parent { get; }

        public Tree(Node parent = null) {
            Parent = parent;
        }

        public abstract void BuildTree();

        public virtual NodeState Execute() {
            return _root.Execute();
        }
    }
}