using UnityEngine;

public class CameraManager : Singleton<CameraManager> {
    enum CameraDirection {
        Up,
        Right,
        Down,
        Left,
    }

    private CameraManager() {
    }

    private float _maxTranslationSpeed;
    private float _translationAcceleration;
    private float _maxZoomRatio;
    private float _minZoomRatio;
    private float _zoomSpeed;

    public float Altitude = 40f;

    private Camera _mainCamera;

    private Vector3 _forwardVec;
    private Vector3 _rightVec;
    private bool _isMouseMoveCamera = false;
    private float _mouseExitTime;
    private float _currentTranslationSpeed = 0f;

    private void Awake() {
        _mainCamera = GetComponent<Camera>();
        _maxTranslationSpeed = Global.MaxTranslationSpeed;
        _translationAcceleration = Global.TranslationAcceleration;
        _maxZoomRatio = Global.MaxZoomRatio;
        _minZoomRatio = Global.MinZoomRatio;
        _zoomSpeed = Global.ZoomSpeed;


        _forwardVec = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
        _rightVec = transform.right;
    }

    void Update() {
        if (_currentTranslationSpeed < _maxTranslationSpeed) {
            _currentTranslationSpeed = _mouseExitTime * _translationAcceleration;
        }

        if (Mathf.Abs(Input.mouseScrollDelta.y) > 0f)
            Zoom(Input.mouseScrollDelta.y);

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            _TranslateCamera(CameraDirection.Up);
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            _TranslateCamera(CameraDirection.Right);
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            _TranslateCamera(CameraDirection.Down);
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            _TranslateCamera(CameraDirection.Left);

        if (_isMouseMoveCamera) {
            _mouseExitTime += Time.deltaTime;
            Vector3 mouseCurrentPos = InputManager.Instance.MouseCurrentPos;
            Vector3 mouseDir = (mouseCurrentPos - new Vector3(Screen.width / 2f, Screen.height / 2f, 0)).normalized;
            float radian = Mathf.Acos(Vector3.Dot(mouseDir, Vector3.up)) *
                           ((Vector3.Cross(mouseDir, Vector3.up).z > 0)
                               ? 1
                               : -1);
            _TranslateCamera(radian, _currentTranslationSpeed);
        }
    }

    private void _TranslateCamera(CameraDirection dir) {
        switch (dir) {
            case CameraDirection.Up:
                _TranslateCamera(Global.Radian0, _maxTranslationSpeed);
                break;
            case CameraDirection.Right:
                _TranslateCamera(Global.Radian90, _maxTranslationSpeed);
                break;
            case CameraDirection.Down:
                _TranslateCamera(Global.Radian180, _maxTranslationSpeed);
                break;
            case CameraDirection.Left:
                _TranslateCamera(Global.Radian270, _maxTranslationSpeed);
                break;
            default:
                break;
        }
    }

    private void _TranslateCamera(float radian, float translationSpeed) {
        Vector3 targetVector3 = (_forwardVec * Mathf.Cos(radian) +
                                 _rightVec * Mathf.Sin(radian)) * Time.deltaTime *
                                translationSpeed;

        transform.Translate(targetVector3, Space.World);

        var ray = new Ray(transform.position, Vector3.up * -1000f);
        if (Physics.Raycast(
                ray,
                out var hit,
                1000f,
                Global.TerrainLayerMaskInt
            ))
            transform.position = hit.point + Vector3.up * Altitude;
    }

    public void Zoom(float zoomRatio) {
        float currentSize = _mainCamera.orthographicSize;
        _mainCamera.orthographicSize =
            Mathf.Clamp(currentSize - zoomRatio * Time.deltaTime * _zoomSpeed, _minZoomRatio, _maxZoomRatio);
    }

    public void OnMouseEnterBorder() {
        _isMouseMoveCamera = true;
        _mouseExitTime = 0;
        _currentTranslationSpeed = 0;
    }

    public void OnMouseExitBorder() {
        _isMouseMoveCamera = false;
    }
}