using System.Collections.Generic;
using UnityEngine;

namespace LtBehaviorTree {
    public class Repeater : Decorator {
        private readonly int _count;
        private int _counter;

        public Repeater(Node child, int count = 0) : base(child) {
            _count = count;
        }

        public Repeater(Node child, Node parent, int count = 0) : base(child, parent) {
            _count = count;
        }

        public override NodeState Execute() {
            NodeState ret = NodeState.Success;

            if (Child == null) {
                return NodeState.Failure;
            }

            while (_counter < _count) {
                ret = Child.Execute() == NodeState.Failure ? NodeState.Failure : ret;
                _counter++;
            }

            _counter = 0;
            return ret;
        }
    }
}