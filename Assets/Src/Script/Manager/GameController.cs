using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class GameController : Singleton<GameController> {
    public enum DiplomaticRelation {
        Peace,
        War,
        Allied,
    }

    [SerializeField] private Camera _mainCamera;

    public Player Own;
    public Dictionary<int, HashSet<int>> Team;
    public DiplomaticRelation[][] DiplomaticRelations = new DiplomaticRelation[8][];

    private List<GameObject> _allSelectableGameObjects;

    private List<Unit> _selectedUnits;
    public Building BuildingHeld;
    public GameResource OwnResource;

    void Awake() {
        _selectedUnits = new();
        OwnResource = new(GameResource.ResourceInitType.Full);
        for (int i = 0; i < DiplomaticRelations.Length; i++) {
            DiplomaticRelations[i] = new DiplomaticRelation[8];
            for (int j = 0; j < DiplomaticRelations[i].Length; j++) {
                DiplomaticRelations[i][j] = DiplomaticRelation.Peace;
            }
        }
    }

    void Start() {
        _allSelectableGameObjects = GameObject.FindGameObjectsWithTag("Selectable").ToList();
        EventManager.Instance.AddListener(Global.UiSelectUnitEventStr, OnUnitSelected);
        EventManager.Instance.AddListener(Global.TargetEventStr, OnTargetSelected);
        EventManager.Instance.AddListener(Global.BuildPrepareEventStr, OnBuildPrepared);
        EventManager.Instance.AddListener(Global.BuildFinishEventStr, OnBuildFinished);
        EventManager.Instance.AddListener(Global.UnitDestroyEventStr, OnUnitDestroyed);
    }

    public void AddPlayer(Player player) {
        if (player != null) {
            Team.TryAdd(player.Team, new HashSet<int>());
            Team[player.Team].Add(player);
        }
    }

    public void AddPlayers(List<Player> players) {
        foreach (var player in players) {
            AddPlayer(player);
        }
    }

    public void WarDeclare(Player p1, Player p2) {
        DiplomaticRelations[p1][p2] = DiplomaticRelation.War;
    }

    public void AllyDeclare(Player p1, Player p2) {
        DiplomaticRelations[p1][p2] = DiplomaticRelation.Allied;
    }

    public void PeaceDeclare(Player p1, Player p2) {
        DiplomaticRelations[p1][p2] = DiplomaticRelation.Peace;
    }

    public bool IsEnemy(Player p1, Player p2) {
        return DiplomaticRelations[p1][p2] == DiplomaticRelation.War;
    }

    public void OnUnitSelected(object argsObj) {
        if (argsObj is SelectEventArgs args) {
            Bounds selectedBounds = Util.GetViewportBounds(_mainCamera, args.Mouse0StartPos, args.MouseCurrentPos);
            _selectedUnits.Clear();

            foreach (var unit in _allSelectableGameObjects) {
                var comp = unit.GetComponent<Unit>();

                if (comp != null) {
                    comp.IsSelected =
                        selectedBounds.Contains(_mainCamera.WorldToViewportPoint(unit.transform.position));
                    if (comp.IsSelected) {
                        _selectedUnits.Add(comp);
                    }
                }
            }
        }
    }

    public void OnTargetSelected(object argsObj) {
        if (argsObj is TargetEventArgs args) {
            Ray ray = _mainCamera.ScreenPointToRay(args.Mouse1StartPos);

            if (Physics.Raycast(ray, out var raycastHit, 1000f, Global.TerrainLayerMaskInt)) {
                foreach (var unit in _selectedUnits) {
                    var comp = unit.GetComponent<Character>();
                    if (comp != null) {
                        comp.TargetPosition = raycastHit.point;
                    }
                }
            }
        }
    }

    public void OnBuildPrepared(object argsObj) {
        if (argsObj is BuildPrepareEventArgs args) {
            BuildingHeld = Instantiate(args.Data.Prefab).GetComponent<Building>();
        }
    }

    public void OnBuildFinished(object argsObj) {
        if (argsObj is BuildFinishEventArgs args) {
            if (args.IsConfirm) {
                if (OwnResource.PayFor(BuildingHeld)) {
                    BuildingHeld.ConfirmBuild();
                    if (OwnResource.Afford(BuildingHeld.Cost)) {
                        BuildingHeld = Instantiate(BuildingHeld.Data.Prefab).GetComponent<Building>();
                    }
                    else {
                        EventManager.Instance.OnEvent(Global.BuildFinishEventStr, new BuildFinishEventArgs {
                            IsDone = true,
                            IsConfirm = false
                        });
                    }
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

            if (args.IsDone) {
                BuildingHeld = null;
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
}