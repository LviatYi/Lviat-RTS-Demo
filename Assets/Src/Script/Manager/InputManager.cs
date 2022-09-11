using UnityEngine;

public class InputManager : Singleton<InputManager> {
    private Vector3 _mouse0StartPos;
    private Vector3 _mouse1StartPos;
    private Vector3 _mouse1EndPos;
    private float _mouse0HoldDuration;

    public Vector3 MouseCurrentPos => Input.mousePosition;

    private bool _prepareBuild;

    private InputManager() {
    }

    void OnEnable() {
        EventManager.Instance.AddListener(Global.BuildPrepareEventStr, OnBuildPrepared);
        EventManager.Instance.AddListener(Global.BuildFinishEventStr, OnBuildFinished);
    }

    void OnDisable() {
        EventManager.Instance.RemoveListener(Global.BuildPrepareEventStr, OnBuildPrepared);
        EventManager.Instance.RemoveListener(Global.BuildFinishEventStr, OnBuildFinished);
    }

    // Update is called once per frame
    void Update() {
        if (_prepareBuild) {
            if (Input.GetMouseButtonDown(0) &&
                GameController.Instance.BuildingHeld?.PlaceState == Building.BuildingPlaceState.Valid) {
                BuildFinishEventArgs args = new BuildFinishEventArgs {
                    IsConfirm = true,
                    IsDone = !Input.GetKey(KeyCode.LeftShift),
                };

                EventManager.Instance.OnEvent(Global.BuildFinishEventStr, args);
            }

            if (Input.GetKeyDown(KeyCode.Escape)) {
                EventManager.Instance.OnEvent(Global.BuildFinishEventStr, new BuildFinishEventArgs() {
                    IsConfirm = false,
                    IsDone = true
                });
            }
        }

        if (Input.GetMouseButtonDown(0)) {
            _mouse0StartPos = Input.mousePosition;
        }

        if (Input.GetMouseButton(0)) {
            _mouse0HoldDuration += Time.deltaTime;
            if (_mouse0HoldDuration > 0.1) {
                EventManager.Instance.OnEvent(Global.UiSelectUnitEventStr, new SelectEventArgs() {
                    IsSingleSelect = false,
                    Mouse0StartPos = _mouse0StartPos,
                    MouseCurrentPos = MouseCurrentPos
                });
            }
        }

        if (Input.GetMouseButtonUp(0)) {
            UIManager.Instance.ClearSelectRect();
            Debug.Log(_mouse0HoldDuration);
            if (_mouse0HoldDuration <= 0.1) {
                EventManager.Instance.OnEvent(Global.UiSelectUnitEventStr, new SelectEventArgs() {
                    IsSingleSelect = true,
                    Mouse0StartPos = _mouse0StartPos,
                    MouseCurrentPos = MouseCurrentPos,
                });
            }

            _mouse0HoldDuration = 0f;
        }
    }

    public void OnBuildPrepared(object argsObj) {
        _prepareBuild = true;
    }

    public void OnBuildFinished(object argsObj) {
        if (argsObj is BuildFinishEventArgs args) {
            _prepareBuild = !args.IsDone;
        }
    }
}