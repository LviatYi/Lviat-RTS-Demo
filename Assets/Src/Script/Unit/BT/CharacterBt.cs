using System;
using System.Collections.Generic;
using LtBehaviorTree;

public class CharacterBt : Tree {
    private Character _parent;

    public CharacterBt(Node parent = null) : base(parent) {
    }

    public override void BuildTree() {
        _root = new Selector(
            new List<Node>() {
                new Sequence(new List<Node> { new CheckHasDestination(_parent), new TaskMoveTo(_parent) }),
                new Selector(new List<Node>() {
                    new Sequence(new List<Node>() {
                        new CheckTargetInAttackRange(_parent), new TaskAttack(_parent)
                    }),
                    new TaskFollow(_parent)
                })
            });
    }
}