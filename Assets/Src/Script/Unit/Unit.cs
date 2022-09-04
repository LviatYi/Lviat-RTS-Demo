using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Unit : MonoBehaviour, ISelectable {
    private bool _isSelected;

    [Header("Intro")] public string UnitName;
    public string Description;
    public GameResource Cost;
    [SerializeField] public UnitData Data;
    public int OwnerIndex;

    [Header("Unit Ability")] public int MaxHitPoint;
    public int CurrentHitPoint;
    public float AttackRange;
    public int Damage;
    public float AttackRate;
    public float RangeOfVision;

    [SerializeField] protected GameObject SelectedMarker;
    public Color Color;

    private LtBehaviorTree.Tree<Unit> _behaviorTree;

    public Unit Target;

    private void Awake() {
        Init();
    }

    protected virtual void Init() {
        UnitName = Data.UnitName;
        Description = Data.Description;
        Cost = Data.Cost;
        MaxHitPoint = Data.MaxHitPoint;
        AttackRange = Data.AttackRange;
        Damage = Data.Damage;
        AttackRate = Data.AttackRate;
        RangeOfVision = Data.RangeOfVision;

        List<Transform> ts = GetComponentsInChildren<Transform>().ToList();
        SelectedMarker = GetComponentsInChildren<Transform>(true)
            .ToList().First(t => t.gameObject.name == "SelectedMarker").gameObject;
        SetMaterialColor(Color.blue);
    }


    public bool IsSelected {
        get => _isSelected;
        set {
            _isSelected = value;
            SelectedMarker.SetActive(value);
        }
    }

    private void SetMaterialColor() {
        SelectedMarker.GetComponent<SpriteRenderer>().color = Color;
    }

    private void SetMaterialColor(Color color) {
        Color = color;
        SetMaterialColor();
    }

    public virtual void UnderAttack(int damage) {
        CurrentHitPoint -= damage;
        if (CurrentHitPoint <= 0) {
            Die();
        }
    }

    public virtual void Die() {
        EventManager.Instance.OnEvent(Global.UnitDestroyEventStr, new UnitDestroyEventArgs {
            DestroyUnit = this,
        });

        Destroy(this.gameObject);
    }
}