using System;
using UnityEngine;


[Serializable]
public class Player {
    [SerializeField] private int _index;
    [SerializeField] public string Name;
    [SerializeField] public string Uid;
    [SerializeField] public int Team;
    [SerializeField] public Color Color;
    [SerializeField] public bool IsAlive;

    public int Index {
        get => _index;
        set {
            if (value is < 1 or > 8) {
                throw new IndexOutOfRangeException();
            }
        }
    }

    public Player(int index, string name) : this(index, name, UnityEngine.Color.white) {
    }

    public Player(int index, string name, Color color, int team = 0, bool isAlive = true) : this(
        index, name, System.Guid.NewGuid().ToString("N"), color, team, isAlive) {
    }

    public Player(int index, string name, string uid, Color color, int team = 0, bool isAlive = true) {
        _index = index;
        Name = name;
        Uid = uid;
        Team = team;
        Color = color;
        IsAlive = isAlive;
    }

    public static implicit operator int(Player player) {
        return player.Index;
    }
}