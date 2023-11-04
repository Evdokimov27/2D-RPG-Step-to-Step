using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public BoxCollider2D boundsCollider; // Ссылка на коллайдер, который будет определять границы
    public float zoomSpeed = 5; // Скорость зума
    public float dragSpeed = 10;
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
    private float mapX, mapY;
    public float minX, maxX, minY, maxY;
    private float vertExtent, horzExtent;

    void Awake()
    {
        gameObject.tag = "MainCamera"; // Присвоить тег по умолчанию
        boundsCollider = this.gameObject.GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        Bounds cameraBounds = boundsCollider.bounds;
        minX = cameraBounds.min.x;
        maxX = cameraBounds.max.x;
        minY = cameraBounds.min.y;
        maxY = cameraBounds.max.y;
        this.gameObject.transform.position = new Vector3(-12f, 10.5f, -1.15f);
    }
    void LateUpdate()
    {
        DragCam();

        // Проверка границ после перемещения
        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);

        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }


    // Движение камеры
    void DragCam()
    {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBPLAYER
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
                moveDir = new Vector3(touchPos.x, touchPos.y, 0);
                break;
            case DragAxes.X:
                moveDir = new Vector3(touchPos.x, 0, 0);
                break;
            case DragAxes.Y:
                moveDir = new Vector3(0, touchPos.y, 0);
                break;
            default:
                break;
        }

        moveDir *= dragSpeed * Time.deltaTime;
        transform.position -= moveDir;
#endif

#if UNITY_ANDROID || UNITY_IPHONE || UNITY_WP8
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            touchPos = Input.GetTouch(0).deltaPosition;
            switch (dragAxes)
            {
                case DragAxes.XY:
                    moveDir = new Vector3(touchPos.x, touchPos.y, 0);
                    break;
                case DragAxes.X:
                    moveDir = new Vector3(touchPos.x, 0, 0);
                    break;
                case DragAxes.Y:
                    moveDir = new Vector3(0, touchPos.y, 0);
                    break;
                default:
                    break;
            }

            transform.position -= moveDir * dragSpeed * Time.deltaTime;
        }
#endif
    }

    // Отображение границ в режиме редактирования
    void OnDrawGizmos()
    {
        bounds.SetMinMax(sizeX, sizeY);
        Gizmos.DrawWireCube(bounds.center, bounds.size);
    }
}
