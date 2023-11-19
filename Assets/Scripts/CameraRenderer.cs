using UnityEngine;

public class CameraRenderer : MonoBehaviour
{
	public Vector2 DefaultResolution = new Vector2(720, 1280);
	[Range(0f, 1f)] public float WidthOrHeight = 0;

	private Camera componentCamera;

	private float initialSize;
	private float targetAspect;

	private float initialFov;
	private float horizontalFov = 120f;

	private void Start()
	{
		Canvas[] objectsToCanvas = FindObjectsOfType<Canvas>();
		foreach (var obj in objectsToCanvas)
		{
			obj.worldCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		}
	}
}