using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Collectivity : List<Unit> {
    public enum CollectivityTypeState {
        Mixed,
        CharacterOnly
    }

    public enum CollectivityOwnerState {
        Mixed,
        Mine,
    }

    public CollectivityTypeState CollectivityType;
    public CollectivityOwnerState CollectivityOwner;

    public int OwnerIndex;
    public bool IsSelected;
    private readonly CollectivityBt<Collectivity> _behaviorTree;

    private Collectivity() {
        _behaviorTree = new CollectivityBt<Collectivity>(this);
        _behaviorTree.BuildTree();
    }

    public Collectivity(CollectivityTypeState collectivityTypeState = CollectivityTypeState.Mixed,
        CollectivityOwnerState collectivityOwnerState = CollectivityOwnerState.Mixed) : this() {
        CollectivityType = collectivityTypeState;
        CollectivityOwner = collectivityOwnerState;
    }

    public Collectivity(IEnumerable<Unit> units) : this() {
        this.AddRange(units);
    }

    public void SetData(IEnumerable<Unit> units) {
        this.Clear();
        this.AddRange(units);
    }

    public void Invoke() {
        _behaviorTree.Execute();
    }

    /// <summary>
    /// Filter Character in a new List
    /// </summary>
    /// <returns></returns>
    public Collectivity ToCharacterList() {
        Collectivity ret = new Collectivity(CollectivityTypeState.CharacterOnly);
        if (CollectivityType == CollectivityTypeState.CharacterOnly) {
            ret = new Collectivity(this);
        }
        else {
            ret = new Collectivity(this.Where(value => value is Character));
        }

        return ret;
    }
}