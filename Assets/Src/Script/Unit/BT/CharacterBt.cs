using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using LtBehaviorTree;

public class CharacterBt<T> : Tree<Character> where T : Character {
    public CharacterBt([NotNull] T mount, Node parent = null) : base(mount, parent) {
    }

    protected override void OnBuildTree() {
        Root = new Selector(
            new() {
                new Sequence(new() {
                        new CheckHasTarget(),
                        new Selector(
                            new() {
                                new Sequence(new() {
                                    new CheckTargetInAttackRange(),
                                    new Timer(
                                        new TaskAttack(), Mount.AttackRate)
                                }),
                                new Selector(new() {
                                    new Sequence(new() {
                                        new CheckAnyTargetInFOVRange(),
                                        new TaskFollow()
                                    }),
                                    new TaskRemoveTarget()
                                })
                            }),
                        new CheckMoving(),
                        new Sequence(
                            new() {
                                new CheckAnyTargetInFOVRange(),
                                new TaskSetTarget(),
                            })
                    }
                )
            });
    }
}