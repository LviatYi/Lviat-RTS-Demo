using System;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
    private static T _instance;
    private static readonly object Lock = new();

    public static T Instance {
        get {
            if (_instance == null) {
                GameObject ownerGameObject = FindObjectOfType(typeof(T)).GameObject();

                if (ownerGameObject == null) {
                    lock (Lock) {
                        if (_instance == null) {
                            GameObject obj = new();
                            obj.name = typeof(T).ToString() + " singleton";
                            _instance = obj.AddComponent<T>();
                        }
                    }
                }
                else {
                    _instance = ownerGameObject.GetComponent<T>();
                }

                DontDestroyOnLoad(ownerGameObject);
            }

            return _instance;
        }
    }
}


public static class Util {
    public static Bounds GetViewportBounds(Camera camera, Vector3 screenPosition1, Vector3 screenPosition2) {
        var v1 = camera.ScreenToViewportPoint(screenPosition1);
        var v2 = camera.ScreenToViewportPoint(screenPosition2);
        var min = Vector3.Min(v1, v2);
        var max = Vector3.Max(v1, v2);
        min.z = camera.nearClipPlane;
        max.z = camera.farClipPlane;

        var bounds = new Bounds();
        bounds.SetMinMax(min, max);
        return bounds;
    }
}