using JetBrains.Annotations;

namespace LtBehaviorTree {
    public abstract class Tree<T> {
        protected Node Root;
        protected readonly T Mount;

        public Node Parent { get; }

        public Tree([NotNull] T mount, Node parent = null) {
            Parent = parent;
            Mount = mount;
        }

        public void BuildTree() {
            OnBuildTree();
            SetPara(Global.BtParaMountStr, Mount);
            Root.PreOrderSetChildrenParent();
        }

        protected abstract void OnBuildTree();

        public virtual NodeState Execute() {
            return Root.Tick();
        }

        public void SetPara(string paraName, object para) {
            Root?.SetPara(paraName, para);
        }

        public object GetPara(string paraName) {
            return Root?.GetPara(paraName);
        }
    }
}