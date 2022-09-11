using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;


public class GameController : Singleton<GameController> {
    public enum DiplomaticRelation {
        Peace,
        War,
        Allied,
    }

    [SerializeField] public Camera MainCamera;

    public Player Own;
    public List<Player> Players;
    public Dictionary<int, HashSet<int>> Team;
    public DiplomaticRelation[][] DiplomaticRelations = new DiplomaticRelation[8][];

    private List<GameObject> _allSelectableGameObjects;

    private Collectivity _selectedUnits;
    [CanBeNull] public Building BuildingHeld;
    public GameResource OwnResource;

    void Awake() {
        _selectedUnits = new() { };
        Team = new Dictionary<int, HashSet<int>>();
        OwnResource = new(GameResource.ResourceInitType.Full);
        for (int i = 0; i < DiplomaticRelations.Length; i++) {
            DiplomaticRelations[i] = new DiplomaticRelation[8];
            for (int j = 0; j < DiplomaticRelations[i].Length; j++) {
                DiplomaticRelations[i][j] = DiplomaticRelation.Peace;
            }
        }

        DevInjectAwake();
    }

    void OnEnable() {
        EventManager.Instance.AddListener(Global.UiSelectUnitEventStr, OnUnitSelected);
        EventManager.Instance.AddListener(Global.BuildPrepareEventStr, OnBuildPrepared);
        EventManager.Instance.AddListener(Global.BuildFinishEventStr, OnBuildFinished);
        EventManager.Instance.AddListener(Global.UnitDestroyEventStr, OnUnitDestroyed);
    }

    void Start() {
        _allSelectableGameObjects = GameObject.FindGameObjectsWithTag("Selectable").ToList();
    }

    void Update() {
        _selectedUnits.Invoke();
    }

    private void OnDisable() {
        EventManager.Instance.RemoveListener(Global.UiSelectUnitEventStr, OnUnitSelected);
        EventManager.Instance.RemoveListener(Global.BuildPrepareEventStr, OnBuildPrepared);
        EventManager.Instance.RemoveListener(Global.BuildFinishEventStr, OnBuildFinished);
        EventManager.Instance.RemoveListener(Global.UnitDestroyEventStr, OnUnitDestroyed);
    }


    public void AddPlayer(Player player) {
        if (player != null) {
            Players.Add(player);
            Team.TryAdd(player.Team, new HashSet<int>());
            Team[player.Team].Add(player);
        }
    }

    public void AddPlayers(List<Player> players) {
        foreach (var player in players) {
            AddPlayer(player);
        }
    }

    [CanBeNull]
    public Player GetPlayer(int id) {
        return Players.FirstOrDefault(player => player.Index == id);
    }

    public void WarDeclare(int p1, int p2) {
        if (p1 == p2) {
            return;
        }

        DiplomaticRelations[Mathf.Min(p1, p2)][Mathf.Max(p1, p2)] = DiplomaticRelation.War;
    }

    public void AllyDeclare(int p1, int p2) {
        if (p1 == p2) {
            return;
        }

        DiplomaticRelations[Mathf.Min(p1, p2)][Mathf.Max(p1, p2)] = DiplomaticRelation.Allied;
    }

    public void PeaceDeclare(int p1, int p2) {
        if (p1 == p2) {
            return;
        }

        DiplomaticRelations[Mathf.Min(p1, p2)][Mathf.Max(p1, p2)] = DiplomaticRelation.Peace;
    }

    public bool IsEnemy(int p1, int p2) {
        if (p1 == p2) {
            return false;
        }

        return DiplomaticRelations[Mathf.Min(p1, p2)][Mathf.Max(p1, p2)] == DiplomaticRelation.War;
    }

    public bool IsEnemy(int p) {
        return IsEnemy(p, Own.Index);
    }

    public void OnUnitSelected(object argsObj) {
        if (argsObj is SelectEventArgs args) {
            if (args.IsSingleSelect) {
                foreach (var selectedUnit in _selectedUnits) {
                    selectedUnit.IsSelected = false;
                }

                _selectedUnits.Clear();

                Ray ray = GameController.Instance.MainCamera.ScreenPointToRay(args.Mouse0StartPos);
                if (Physics.Raycast(ray, out var raycastHit, 1000f,
                        Global.UnitLayerMaskInt)) {
                    Unit selectedUnit = raycastHit.collider.gameObject.GetComponent<Unit>();
                    selectedUnit.IsSelected = true;
                    _selectedUnits.Add(selectedUnit);
                }
            }
            else {
                _selectedUnits.Clear();
                Bounds selectedBounds = Util.GetViewportBounds(MainCamera, args.Mouse0StartPos, args.MouseCurrentPos);

                foreach (var unit in _allSelectableGameObjects) {
                    var comp = unit.GetComponent<Unit>();

                    if (comp != null) {
                        comp.IsSelected =
                            selectedBounds.Contains(MainCamera.WorldToViewportPoint(unit.transform.position));
                        if (comp.IsSelected) {
                            _selectedUnits.Add(comp);
                        }
                    }
                }
            }

            _selectedUnits.IsSelected = _selectedUnits.Count > 0;
        }
    }

    public void OnBuildPrepared(object argsObj) {
        if (argsObj is BuildPrepareEventArgs args) {
            BuildingHeld = Instantiate(args.Data.Prefab).GetComponent<Building>();
            if (BuildingHeld != null) BuildingHeld.PrepareBuild();
        }
    }

    public void OnBuildFinished(object argsObj) {
        if (BuildingHeld == null) {
            Debug.Log("BuildingHeld is null");
            return;
        }

        Building originBuildingHeld = BuildingHeld;

        if (argsObj is BuildFinishEventArgs args) {
            if (args.IsConfirm) {
                if (OwnResource.PayFor(BuildingHeld)) {
                    BuildingHeld.OwnerIndex = Instance.Own.Index;
                    BuildingHeld.ConfirmBuild();
                    BuildingHeld = null;
                }
                else {
                    EventManager.Instance.OnEvent(Global.BuildFinishEventStr, new BuildFinishEventArgs {
                        IsDone = true,
                        IsConfirm = false
                    });
                }
            }
            else {
                Destroy(BuildingHeld.gameObject);
            }

            if (!args.IsDone) {
                BuildingHeld = Instantiate(originBuildingHeld.Data.Prefab).GetComponent<Building>();
                if (BuildingHeld != null) BuildingHeld.PrepareBuild();
            }
        }
    }

    public void OnUnitDestroyed(object argsObj) {
        if (argsObj is UnitDestroyEventArgs args) {
            _allSelectableGameObjects.Remove(args.DestroyUnit.gameObject);
            if (args.DestroyUnit.IsSelected) {
                _selectedUnits.Remove(args.DestroyUnit);
            }
        }
    }

    public void DevInjectAwake() {
        Own = new(1, "Player 1", Global.AvailableColors[Global.ColorBlueStr], 1);
        Player enemy = new(2, "Player 2", Global.AvailableColors[Global.ColorRedStr], 2);

        AddPlayers(new List<Player> {
            Own, enemy
        });

        WarDeclare(Own, enemy);
    }
}