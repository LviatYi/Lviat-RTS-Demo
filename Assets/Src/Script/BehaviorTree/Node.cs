using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace LtBehaviorTree {
    [Flags]
    public enum NodeState {
        Failure = 1 << 0,
        Success = 1 << 1,
        Running = 1 << 2,
    }

    public abstract class Node {
        [CanBeNull] private Node _parent;
        [CanBeNull] private Dictionary<string, object> _parameters;

        [CanBeNull]
        public Node Parent {
            set {
                _parent = value;
                _parameters = _parent?._parameters;
            }
            get => _parent;
        }

        protected Node(Node parent = null) {
            Parent = parent;
        }

        public abstract NodeState Tick();

        [CanBeNull]
        public object GetPara(string paraName) {
            if (_parameters is not null && _parameters.TryGetValue(paraName, out var para)) {
                return para;
            }

            return null;
        }

        public void SetPara(string paraName, object para) {
            _parameters ??= new();
            _parameters[paraName] = para;
        }

        public abstract List<Node> GetChildren();

        public void PreOrderSetChildrenParent() {
            foreach (Node child in GetChildren()) {
                child.Parent = this;
                child.PreOrderSetChildrenParent();
            }
        }
    }
}