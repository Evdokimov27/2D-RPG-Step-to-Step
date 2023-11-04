using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Transform player;
    public Transform enemy;
    public float smoothSpeed = 2.0f;
    public Vector3 originalOffset;

    private Vector3 offset;
    private bool isFollowing = false;

    private void Start()
    {
        offset = new Vector3(0, 0, -10);
        originalOffset = offset;
    }

    private void LateUpdate()
    {
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            if (target == null)
            {
                target = GameObject.FindGameObjectWithTag("Player").transform;
                player = target;
            }
            if (target.GetComponent<Player>().nearestEnemy != null) enemy = target.GetComponent<Player>().nearestEnemy.transform;

            if (isFollowing)
            {
                Vector3 targetPosition = (player.position + enemy.position) / 2 + offset;
                Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
                transform.position = smoothedPosition;
            }
            else
            {
                Vector3 desiredPosition = target.position + offset;
                Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
                transform.position = smoothedPosition;
            }
        }
    }

    public void StartFollowing()
    {
        offset = originalOffset;
        isFollowing = true;
    }

    public void StopFollowing()
    {
        offset = originalOffset;
        isFollowing = false;
    }
}
