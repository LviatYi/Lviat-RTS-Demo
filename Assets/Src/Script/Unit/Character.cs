using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 
/// </summary>
/// <author>LviatYi</author>
/// <time>2022/8/17</time>
public class Character : Unit {
    [SerializeField] public NavMeshAgent Agent;
    private CharacterBt<Character> _behaviorTree;

    [Header("Character Ability")] public float MoveSpeed;

    public Vector3 Destination {
        set => Agent.destination = value;
        get => Agent.destination;
    }

    void Awake() {
        Init();
    }

    // Update is called once per frame
    void Update() {
        _behaviorTree.Execute();
    }

    protected override void Init() {
        base.Init();
        if (Data is CharacterData data) {
            MoveSpeed = data.MoveSpeed;
        }

        Agent = GetComponent<NavMeshAgent>();
        _behaviorTree = new CharacterBt<Character>(this);
        _behaviorTree.BuildTree();
    }
}