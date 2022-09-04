using System.Collections.Generic;
using UnityEngine;

public class Building : Unit {
    public enum BuildingPlaceState {
        Invalid,
        Valid,
        Placed,
    }

    //private Tree<Building> _behaviorTree;

    private BoxCollider _collider;
    private int _nCollisions;
    private MeshRenderer _meshRenderer;
    private List<Material> _materials;
    private BuildingPlaceState _placeState;

    void Awake() {
        Init();
    }

    public BuildingPlaceState PlaceState {
        get => _placeState;
        set {
            _placeState = value;
            SetMaterials();
        }
    }

    protected override void Init() {
        base.Init();
        _collider = GetComponent<BoxCollider>();
        _meshRenderer = GetComponentInChildren<MeshRenderer>();

        _materials = new List<Material>();
        foreach (Material material in transform.Find("Mesh").GetComponent<Renderer>().materials) {
            _materials.Add(new Material(material));
        }

        _placeState = BuildingPlaceState.Valid;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Terrain") return;
        _nCollisions++;
    }

    private void OnTriggerExit(Collider other) {
        if (other.tag == "Terrain") return;
        _nCollisions--;
    }

    public void SetPosition(Vector3 position) {
        transform.position = position;
    }

    private void SetMaterials() {
        SetMaterials(PlaceState);
    }

    private void SetMaterials(BuildingPlaceState placement) {
        List<Material> materials;
        if (placement == BuildingPlaceState.Valid) {
            Material refMaterial = Resources.Load("Material/Valid") as Material;
            materials = new List<Material>();
            for (int i = 0; i < _materials.Count; i++)
                materials.Add(refMaterial);
        }
        else if (placement == BuildingPlaceState.Invalid) {
            Material refMaterial = Resources.Load("Material/Invalid") as Material;
            materials = new List<Material>();
            for (int i = 0; i < _materials.Count; i++)
                materials.Add(refMaterial);
        }
        else if (placement == BuildingPlaceState.Placed)
            materials = _materials;
        else
            return;

        _meshRenderer.materials = materials.ToArray();
    }

    public bool CheckTerrainSuitability() {
        Vector3 p = transform.position;
        Vector3 c = _collider.center;
        Vector3 e = _collider.size / 2f;
        float bottomHeight = c.y - e.y + 1f;
        Vector3[] bottomCorners = {
            new(c.x - e.x, bottomHeight, c.z - e.z),
            new(c.x - e.x, bottomHeight, c.z + e.z),
            new(c.x + e.x, bottomHeight, c.z - e.z),
            new(c.x + e.x, bottomHeight, c.z + e.z)
        };

        int invalidCornersCount = 0;
        foreach (Vector3 corner in bottomCorners) {
            if (!Physics.Raycast(
                    p + corner,
                    Vector3.down,
                    1000f,
                    Global.TerrainLayerMaskInt
                ))
                invalidCornersCount++;
        }

        if (invalidCornersCount >= 2) {
            return false;
        }

        return true;
    }

    public bool UpdatePlaceStateAndMaterials() {
        if (PlaceState == BuildingPlaceState.Placed) {
            return false;
        }

        if (_nCollisions > 0 || !CheckTerrainSuitability()) {
            PlaceState = BuildingPlaceState.Invalid;
            return false;
        }

        PlaceState = BuildingPlaceState.Valid;
        return true;
    }

    public void ConfirmBuild() {
        PlaceState = BuildingPlaceState.Placed;
    }
}