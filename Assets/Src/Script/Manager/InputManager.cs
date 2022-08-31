using UnityEngine;

public class InputManager : Singleton<InputManager> {
    private Vector3 _mouse0StartPos;
    private Vector3 _mouse1StartPos;
    private Vector3 _mouse1EndPos;

    public Vector3 MouseCurrentPos => Input.mousePosition;

    private bool _prepareBuild = false;

    private InputManager() {
    }

    void Start() {
        EventManager.Instance.AddListener(Global.BuildPrepareEventStr, OnBuildPrepared);
        EventManager.Instance.AddListener(Global.BuildFinishEventStr, OnBuildFinished);
    }

    // Update is called once per frame
    void Update() {
        if (_prepareBuild) {
            if (Input.GetMouseButtonDown(0) &&
                GameController.Instance.BuildingHeld.PlaceState == Building.BuildingPlaceState.Valid) {
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
            EventManager.Instance.OnEvent(Global.UiSelectUnitEventStr, new SelectEventArgs() {
                Mouse0StartPos = _mouse0StartPos,
                MouseCurrentPos = MouseCurrentPos
            });
        }

        if (Input.GetMouseButtonUp(0)) {
            UIManager.Instance.ClearSelectRect();
        }

        if (Input.GetMouseButtonDown(1)) {
            EventManager.Instance.OnEvent(Global.TargetEventStr, new TargetEventArgs() {
                Mouse1StartPos = Input.mousePosition,
                Mouse1DragDestPos = Input.mousePosition
            });
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