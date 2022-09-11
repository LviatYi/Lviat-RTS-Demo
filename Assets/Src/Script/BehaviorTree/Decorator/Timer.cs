using System;
using System.Collections.Generic;
using UnityEngine;

namespace LtBehaviorTree {
    public class Timer : Decorator {
        public float Delay;
        private float _time;

        public Timer(Node child, float delay = 0.0f) : base(child) {
            Delay = delay;
        }

        public Timer(Node child, Node parent, float delay = 0.0f) : base(child, parent) {
            Delay = delay;
        }

        public override NodeState Tick() {
            NodeState ret = NodeState.Failure;
            if (Child == null) {
                return ret;
            }

            if (_time > 0.0f) {
                _time -= Time.deltaTime;
                ret = NodeState.Running;
            }
            else {
                ret = Child.Tick();
                if (ret == NodeState.Success) {
                    _time = Delay;
                }
            }

            return ret;
        }
    }
}