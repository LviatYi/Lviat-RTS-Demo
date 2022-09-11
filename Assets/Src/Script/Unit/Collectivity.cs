using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
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
    private bool _behaviorAble;

    public int OwnerIndex;
    public bool IsSelected;
    [CanBeNull] private CollectivityBt<Collectivity> _behaviorTree;

    public bool BehaviorAble {
        get => _behaviorAble;
        set {
            if (!_behaviorAble && value) {
                _behaviorTree = new CollectivityBt<Collectivity>(this);
                _behaviorTree.BuildTree();
                _behaviorAble = true;
            }
            else if (_behaviorAble && !value) {
                _behaviorTree = null;
                _behaviorAble = false;
            }
        }
    }

    public Collectivity(CollectivityTypeState collectivityTypeState = CollectivityTypeState.Mixed,
        CollectivityOwnerState collectivityOwnerState = CollectivityOwnerState.Mixed,
        bool behaviorAble = false) {
        CollectivityType = collectivityTypeState;
        CollectivityOwner = collectivityOwnerState;
        BehaviorAble = behaviorAble;
    }

    public Collectivity(IEnumerable<Unit> units) : this() {
        this.AddRange(units);
    }

    public void SetData(IEnumerable<Unit> units) {
        this.Clear();
        this.AddRange(units);
    }

    public void Invoke() {
        BehaviorAble = true;

        _behaviorTree!.Execute();
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

    public Collectivity ToMineList() {
        int ownIndex = GameController.Instance.Own.Index;
        Collectivity ret = new Collectivity();

        ret = new Collectivity(this.Where(value => value.OwnerIndex == ownIndex));

        return ret;
    }

    public Collectivity FilterMine() {
        Collectivity temp = new Collectivity(this);
        this.Clear();
        this.AddRange(temp.ToMineList());
        return this;
    }
}