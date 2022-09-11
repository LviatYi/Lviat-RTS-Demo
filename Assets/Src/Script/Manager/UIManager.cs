using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class UIManager : Singleton<UIManager> {
    private UIManager() {
    }

    [SerializeField] private Camera _mainCamera;
    [SerializeField] private GameObject _selectRect;
    [SerializeField] private GameObject _functionalPanel;

    [SerializeField] private Object _buildButtonPrefab;

    public bool IsBuildingHeld = false;

    void Awake() {
        _selectRect = GameObject.Find("SelectRect");
        _functionalPanel = GameObject.Find("FunctionalPanel");
    }

    void OnEnable() {
        EventManager.Instance.AddListener(Global.UiSelectUnitEventStr, OnUnitSelected);
        EventManager.Instance.AddListener(Global.BuildFinishEventStr, OnBuildFinished);
    }

    void OnDisable() {
        EventManager.Instance.RemoveListener(Global.UiSelectUnitEventStr, OnUnitSelected);
        EventManager.Instance.RemoveListener(Global.BuildFinishEventStr, OnBuildFinished);
    }

    void Start() {
        foreach (BuildingData data in DataHandler.Instance.BuildingDatas) {
            var buttonObj = Instantiate(_buildButtonPrefab, _functionalPanel.transform);
            var buttonCmpt = buttonObj.GetComponent<Button>();

            buttonCmpt.name = $"{data.name} build button";
            BuildingData currentData = data;
            buttonCmpt.onClick.AddListener(() => OnBuildButtonClicked(currentData));
            buttonObj.GetComponentInChildren<TextMeshProUGUI>().text = data.name;
        }
    }

    private void Update() {
        if (IsBuildingHeld && GameController.Instance.BuildingHeld != null) {
            Ray ray = _mainCamera.ScreenPointToRay(InputManager.Instance.MouseCurrentPos);

            if (Physics.Raycast(ray, out var raycastHit, 1000f, Global.TerrainLayerMaskInt)) {
                GameController.Instance.BuildingHeld.SetPosition(raycastHit.point);
                GameController.Instance.BuildingHeld.UpdatePlaceStateAndMaterials();
            }
        }
    }


    private void OnUnitSelected(object argsObj) {
        if (argsObj is SelectEventArgs { IsSingleSelect: false } args) {
            SetSelectRect(args.Mouse0StartPos, args.MouseCurrentPos);
        }
    }

    public void SetSelectRect(Vector2 pos1, Vector2 pos2) {
        Vector2 max = Vector2.Max(pos1, pos2);
        Vector2 min = Vector2.Min(pos1, pos2);
        Vector2 size = max - min;

        _selectRect.GetComponent<RectTransform>().anchoredPosition = min;
        _selectRect.GetComponent<RectTransform>().sizeDelta = size;
    }

    public void ClearSelectRect() {
        SetSelectRect(new Vector2(0, 0), new Vector2(-100, -100));
    }

    public void OnBuildButtonClicked(BuildingData data) {
        EventManager.Instance.OnEvent(Global.BuildPrepareEventStr, new BuildPrepareEventArgs {
            Data = data
        });
        IsBuildingHeld = true;
    }

    public void OnBuildFinished(object argsObj) {
        if (argsObj is BuildFinishEventArgs args) {
            IsBuildingHeld = !args.IsDone;
        }
    }
}