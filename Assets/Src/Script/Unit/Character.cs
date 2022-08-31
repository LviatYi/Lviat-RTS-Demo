using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 
/// </summary>
/// <author>LviatYi</author>
/// <time>2022/8/17</time>
public class Character : Unit {
    [SerializeField] private NavMeshAgent agent;


    public bool HasDest => Vector3.Distance(transform.position, TargetPosition) > 0.5f;

    [Header("Character Ability")] public float MoveSpeed;

    void Awake() {
        Init();
    }

    // Update is called once per frame
    void Update() {
    }

    protected override void Init() {
        base.Init();
        if (Data is CharacterData data) {
            MoveSpeed = data.MoveSpeed;
        }

        BehaviorTree = new CharacterBt();
        BehaviorTree.BuildTree();
    }


    public bool MoveTo(Vector3 destination) {
        agent.destination = destination;
        if (
            agent.pathStatus == NavMeshPathStatus.PathInvalid) {
            return false;
        }

        return true;
    }
}