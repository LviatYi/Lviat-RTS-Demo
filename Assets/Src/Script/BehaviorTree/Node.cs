using System;
using System.Collections.Generic;

namespace LtBehaviorTree {
    [Flags]
    public enum NodeState {
        Failure = 1 << 0,
        Success = 1 << 1,
        Running = 1 << 2,
    }

    public abstract class Node {
        protected NodeState State { get; set; }

        private Node _parent;

        public Node(Node parent = null) {
            _parent = parent;
        }

        public abstract NodeState Execute();
    }
}