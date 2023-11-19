using UnityEngine;
using UnityEngine.UI;

public class CameraControls : MonoBehaviour
{
    public float zoomSpeed = 5; // Скорость зума
    public float dragSpeed = 10;
    public Slider slider;
    public Vector2 sizeX = new Vector2(-15, -10);
    public Vector2 sizeY = new Vector2(15, 10);
    public DragAxes dragAxes = DragAxes.XY;

    public enum DragAxes
    {
        XY,
        X,
        Y
    }
    public enum CameraPosition
    {
        SaveCurrent,
        WithNextLevel
    }
    public CameraPosition cameraPosition = CameraPosition.SaveCurrent;
    public Bounds bounds;
    public Vector3 defaultPosition;
    private Vector3 dragOrigin, touchPos, moveDir;
    public float minX, maxX, minY, maxY;

    void Awake()
    {
        gameObject.tag = "MainCamera"; // Присвоить тег по умолчанию
    }

    void Start()
    {
        minX = -182;
        maxX = 182;
        minY = -169;
        maxY = 169;
        this.gameObject.transform.position = new Vector3(minX, maxY);
    }
    void LateUpdate()
    {
        DragCam();
        dragSpeed = slider.value;
        // Проверка границ после перемещения
        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);

        transform.position = new Vector3(clampedX, clampedY, -5);
    }


    // Движение камеры
    void DragCam()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
            return;
        }
        if (!Input.GetMouseButton(0)) return;

        touchPos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);

        switch (dragAxes)
        {
            case DragAxes.XY:
                moveDir = new Vector3(touchPos.x, touchPos.y);
                break;
            case DragAxes.X:
                moveDir = new Vector3(touchPos.x, 0);
                break;
            case DragAxes.Y:
                moveDir = new Vector3(0, touchPos.y);
                break;
            default:
                break;
        }
        
        moveDir *= dragSpeed * Time.deltaTime;
        transform.position -= moveDir;

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            touchPos = Input.GetTouch(0).deltaPosition;
            switch (dragAxes)
            {
                case DragAxes.XY:
                    moveDir = new Vector3(touchPos.x, touchPos.y);
                    break;
                case DragAxes.X:
                    moveDir = new Vector3(touchPos.x, 0);
                    break;
                case DragAxes.Y:
                    moveDir = new Vector3(0, touchPos.y);
                    break;
                default:
                    break;
            }

            transform.position -= moveDir * dragSpeed * Time.deltaTime;
        }
    }
}
