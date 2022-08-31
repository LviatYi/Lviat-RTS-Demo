using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameResource {
    private static readonly int MaxValue = 999999;
    private static readonly object ResourceLock = new();

    public enum ResourceType {
        Food,
        Wood,
        Gold,
    }

    public enum ResourceInitType {
        Zero,
        Standard,
        Rich,
        Full,
    }

    [SerializeField] private int _food;
    [SerializeField] private int _wood;
    [SerializeField] private int _gold;

    public GameResource(ResourceInitType initType = ResourceInitType.Standard) {
        switch (initType) {
            case ResourceInitType.Zero:
                SetGameResource(0, 0, 0);
                break;
            case ResourceInitType.Standard:
                SetGameResource(300, 150, 50);
                break;
            case ResourceInitType.Rich:
                SetGameResource(2000, 1500, 1000);
                break;
            case ResourceInitType.Full:
                SetGameResource(MaxValue, MaxValue, MaxValue);
                break;
            default:
                SetGameResource(0, 0, 0);
                break;
        }
    }

    public GameResource(int food, int wood, int gold) {
        SetGameResource(food, wood, gold);
    }

    public int Food {
        get => _food;
        set => _food = value < MaxValue ? value : MaxValue;
    }

    public int Wood {
        get => _wood;
        set => _wood = value < MaxValue ? value : MaxValue;
    }

    public int Gold {
        get => _gold;
        set => _gold = value < MaxValue ? value : MaxValue;
    }

    public static bool operator >(GameResource lhs, GameResource rhs) {
        return lhs._food >= rhs.Food && lhs._wood >= rhs.Wood && lhs._gold >= rhs.Gold;
    }

    public static bool operator <(GameResource lhs, GameResource rhs) {
        return lhs._food < rhs.Food || lhs._wood < rhs.Wood || lhs._gold < rhs.Gold;
    }

    public static GameResource operator +(GameResource lhs, GameResource rhs) {
        return new GameResource(lhs.Food + rhs.Food, lhs.Wood + rhs.Wood, lhs.Gold + rhs.Gold);
    }

    public static GameResource operator -(GameResource lhs, GameResource rhs) {
        return new GameResource(lhs.Food - rhs.Food, lhs.Wood - rhs.Wood, lhs.Gold - rhs.Gold);
    }

    private void SetGameResource(int food, int wood, int gold) {
        lock (ResourceLock) {
            _food = food;
            _wood = wood;
            _gold = gold;
        }
    }

    public bool Afford(GameResource rhs) {
        return this > rhs;
    }

    public bool PayFor(Unit unit) {
        if (!Afford(unit.Cost)) {
            return false;
        }

        lock (ResourceLock) {
            Food -= unit.Cost.Food;
            Wood -= unit.Cost.Wood;
            Gold -= unit.Cost.Gold;
        }

        return true;
    }
}