using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataHandler : Singleton<DataHandler> {
    public List<UnitData> UnitDatas;
    public List<CharacterData> CharacterDatas;
    public List<BuildingData> BuildingDatas;

    private DataHandler() {
    }

    void Awake() {
        UnitDatas = Resources.LoadAll<UnitData>("ScriptableObject").ToList();
        CharacterDatas = UnitDatas.OfType<CharacterData>().ToList();
        BuildingDatas = UnitDatas.OfType<BuildingData>().ToList();
    }
}