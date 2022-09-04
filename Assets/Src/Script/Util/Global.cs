using System.Collections.Generic;
using UnityEngine;

public static class Global {
    public static readonly string UiSelectUnitEventStr = "UiSelectUnitEvent";
    public static readonly string TargetEventStr = "TargetEvent";
    public static readonly string BuildPrepareEventStr = "BuildPrepareEvent";
    public static readonly string BuildFinishEventStr = "BuildFinishEvent";
    public static readonly string UnitDestroyEventStr = "UnitDestroyEvent";

    public static readonly string ColorWhiteStr = "White";
    public static readonly string ColorBlueStr = "Blue";
    public static readonly string ColorRedStr = "Red";
    public static readonly string ColorGreenStr = "Green";
    public static readonly string ColorYellowStr = "Yellow";
    public static readonly string ColorOrangeStr = "Orange";
    public static readonly string ColorPurpleStr = "Purple";
    public static readonly string ColorCyanStr = "Cyan";
    public static readonly string ColorGrayStr = "Gray";

    public static readonly Dictionary<string, Color> AvailableColors = new() {
        { ColorWhiteStr, Color.white },
        { ColorBlueStr, new Color(0, 116, 217) },
        { ColorRedStr, new Color(242, 15, 56) },
        { ColorGreenStr, new Color(43, 140, 68) },
        { ColorYellowStr, new Color(242, 206, 22) },
        { ColorOrangeStr, new Color(242, 116, 5) },
        { ColorPurpleStr, new Color(58, 58, 56) },
        { ColorCyanStr, new Color(41, 217, 217) },
        { ColorGrayStr, new Color(58, 58, 56) },
    };

    public static readonly string BtParaMountStr = "Mount";
    public static readonly string BtParaTargetStr = "Target";


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