using System.Collections.Generic;

namespace LtBehaviorTree {
    public abstract class Check : Node {
        public override List<Node> GetChildren() {
            return new() {
                Capacity = 0
            };
        }
    }
}