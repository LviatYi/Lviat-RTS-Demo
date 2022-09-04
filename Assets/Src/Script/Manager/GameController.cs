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

    private Collectivity _selectedUnits;
    public Building BuildingHeld;
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
    }

    void Start() {
        _allSelectableGameObjects = GameObject.FindGameObjectsWithTag("Selectable").ToList();
        EventManager.Instance.AddListener(Global.UiSelectUnitEventStr, OnUnitSelected);
        EventManager.Instance.AddListener(Global.BuildPrepareEventStr, OnBuildPrepared);
        EventManager.Instance.AddListener(Global.BuildFinishEventStr, OnBuildFinished);
        EventManager.Instance.AddListener(Global.UnitDestroyEventStr, OnUnitDestroyed);

        DevInject();
    }

    void Update() {
        _selectedUnits.Invoke();
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

    public void WarDeclare(int p1, int p2) {
        DiplomaticRelations[p1][p2] = DiplomaticRelation.War;
    }

    public void AllyDeclare(int p1, int p2) {
        DiplomaticRelations[p1][p2] = DiplomaticRelation.Allied;
    }

    public void PeaceDeclare(int p1, int p2) {
        DiplomaticRelations[p1][p2] = DiplomaticRelation.Peace;
    }

    public bool IsEnemy(int p1, int p2) {
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

            _selectedUnits.IsSelected = _selectedUnits.Count > 0;
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

    public void DevInject() {
        Own = new(1, "Player 1", Global.AvailableColors[Global.ColorBlueStr], 1);
        Player enemy = new(2, "Player 2", Global.AvailableColors[Global.ColorRedStr], 2);

        AddPlayers(new List<Player> {
            Own, enemy
        });

        WarDeclare(Own, enemy);
    }
}