using UnityEngine;

[CreateAssetMenu(fileName = "Unit", menuName = "Scriptable Objects/Unit", order = 1)]
public class UnitData : ScriptableObject {
    [Header("Intro")] public string UnitName;
    public string Description;
    public GameObject Prefab;
    public GameResource Cost;

    [Header("Ability")] public int MaxHitPoint;
    public float AttackRange;
    public int Damage;
    public float AttackRate;
    public float RangeOfVision;
}