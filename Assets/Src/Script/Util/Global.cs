using UnityEngine;

public static class Global {
    public static readonly string UiSelectUnitEventStr = "UiSelectUnitEvent";
    public static readonly string TargetEventStr = "TargetEvent";
    public static readonly string BuildPrepareEventStr = "BuildPrepareEvent";
    public static readonly string BuildFinishEventStr = "BuildFinishEvent";
    public static readonly string UnitDestroyEventStr = "UnitDestroyEvent";


    public static readonly int TerrainLayerInt = 8;
    public static readonly int UnitLayerInt = 10;
    public static readonly int TerrainLayerMaskInt = 1 << TerrainLayerInt;
    public static readonly int UnitLayerMaskInt = 1 << UnitLayerInt;

    public static readonly float Radian0 = 0f * Mathf.PI;
    public static readonly float Radian90 = 0.5f * Mathf.PI;
    public static readonly float Radian180 = 1f * Mathf.PI;
    public static readonly float Radian270 = 1.5f * Mathf.PI;

    public static readonly float MaxTranslationSpeed = 40f;
    public static readonly float TranslationAcceleration = 40f;
    public static readonly float MaxZoomRatio = 20f;
    public static readonly float MinZoomRatio = 5f;
    public static readonly float ZoomSpeed = 50f;
}