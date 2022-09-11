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

    public float MoveSpeed {
        get => Agent.speed;
        set => Agent.speed = value;
    }

    public Vector3 Destination {
        set => Agent.destination = value;
        get => Agent.destination;
    }

    new void Awake() {
        base.Awake();
        Agent = GetComponent<NavMeshAgent>();

        if (Data is CharacterData data) {
            MoveSpeed = data.MoveSpeed;
        }

        _behaviorTree = new CharacterBt<Character>(this);
        _behaviorTree.BuildTree();
    }

    // Update is called once per frame
    void Update() {
        _behaviorTree.Execute();
    }
}