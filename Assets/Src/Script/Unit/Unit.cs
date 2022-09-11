using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Unit : MonoBehaviour, ISelectable {
    private bool _isSelected;

    [Header("Intro")] public string UnitName;
    public string Description;
    public GameResource Cost;
    [SerializeField] public UnitData Data;
    [SerializeField] private int _ownerIndex;

    public int OwnerIndex {
        set {
            _ownerIndex = value;
            Player p = GameController.Instance.GetPlayer(_ownerIndex);

            IdColor = p?.IdColor ?? Global.AvailableColors[Global.ColorWhiteStr];
        }
        get => _ownerIndex;
    }

    [Header("Unit Ability")] public int MaxHitPoint;
    public int CurrentHitPoint;
    public float AttackRange;
    public int Damage;
    public float AttackRate;
    public float AttackInterval => 1 / AttackRate;
    public float RangeOfVision;

    protected GameObject SelectedMarker;
    protected GameObject IdColorMesh;
    private Color _idColor;

    public Color IdColor {
        set {
            _idColor = value;
            SetMaterialColor();
        }
        get => _idColor;
    }

    private LtBehaviorTree.Tree<Unit> _behaviorTree;

    public Unit Target;

    protected void Awake() {
        SelectedMarker = GetComponentsInChildren<Transform>(true)
            .ToList().First(t => t.gameObject.name == "SelectedMarker").gameObject;
        IdColorMesh = GetComponentsInChildren<Transform>(true)
            .ToList().First(t => t.gameObject.name == "IDColorMesh").gameObject;

        UnitName = Data.UnitName;
        Description = Data.Description;
        Cost = Data.Cost;
        MaxHitPoint = Data.MaxHitPoint;
        CurrentHitPoint = Data.MaxHitPoint;
        AttackRange = Data.AttackRange;
        Damage = Data.Damage;
        AttackRate = Data.AttackRate;
        RangeOfVision = Data.RangeOfVision;
    }

    protected void Start() {
        OwnerIndex = _ownerIndex;
    }

    public bool IsSelected {
        get => _isSelected;
        set {
            _isSelected = value;
            SelectedMarker.SetActive(value);
        }
    }

    private void SetMaterialColor() {
        SelectedMarker.GetComponent<SpriteRenderer>().color = IdColor;
        IdColorMesh.GetComponent<MeshRenderer>().material.color = IdColor;
    }

    // 面向对象方法论的难题：为什么是被攻击者受到攻击而是攻击者发出攻击？
    // TODO_LviatYi: 将以 ECS 解决
    public virtual void UnderAttack(int damage) {
        CurrentHitPoint -= damage;
        Debug.Log($"{this.name} under attack,suffer 5 point damage");
        if (CurrentHitPoint <= 0) {
            Die();
        }
    }

    public virtual void Die() {
        EventManager.Instance.OnEvent(Global.UnitDestroyEventStr, new UnitDestroyEventArgs {
            DestroyUnit = this,
        });

        Debug.Log($"{this.name} got killed");
        Destroy(this.gameObject);
    }
}