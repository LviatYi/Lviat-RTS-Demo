using System;
using UnityEngine;

public class Player {
    private int _index;

    public string Name;
    public string Uid;
    public int Team;
    public Color Color;
    public bool IsAlive;

    public int Index {
        get => _index;
        set {
            if (value is < 1 or > 8) {
                throw new IndexOutOfRangeException();
            }
        }
    }

    public Player() {
        Uid = System.Guid.NewGuid().ToString("N");
    }

    public static implicit operator int(Player player) {
        return player.Index;
    }
}