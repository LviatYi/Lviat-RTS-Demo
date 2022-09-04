using JetBrains.Annotations;
using LtBehaviorTree;

public class CollectivityBt<T> : Tree<T> where T : Collectivity {
    public CollectivityBt([NotNull] T mount, Node parent = null) : base(mount, parent) {
    }

    protected override void OnBuildTree() {
        Root = new Sequence(new() {
            new CheckCollectivityIsSelect(),
            new TaskTrySetTargetOrDestination()
        });
    }
}