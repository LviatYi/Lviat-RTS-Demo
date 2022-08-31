using System.Collections.Generic;
using UnityEngine;

namespace LtBehaviorTree {
    public class Delayer : Decorator {
        private float _delay;
        private float _time;

        public Delayer(Node child, float delay = 0.0f) : base(child) {
            _delay = delay;
        }

        public Delayer(Node child, Node parent, float delay = 0.0f) : base(child, parent) {
            _delay = delay;
        }

        public override NodeState Execute() {
            NodeState ret = NodeState.Failure;
            if (Child == null) {
                return ret;
            }

            if (_time > 0.0f) {
                _time -= Time.deltaTime;
                ret = NodeState.Running;
            }
            else {
                ret = Child.Execute();
                if (ret == NodeState.Success) {
                    _time = _delay;
                }
            }

            return ret;
        }
    }
}