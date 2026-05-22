using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float smoothTime = .1f;
    [SerializeField] Vector3 startingPosition;
    
    float minimumYPosition;
    Vector3 targetPosition;
    Vector3 velocity = Vector3.zero;
    bool isFollowing = true;
    
    void Start()
    {
        startingPosition = transform.position;
        minimumYPosition = transform.position.y;
        targetPosition = transform.position;
    }

    void LateUpdate()
    {
        if (!isFollowing)
        {
            return;
        }

        targetPosition.y = player.position.y;
        ClampVerticalPosition();
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

    void ClampVerticalPosition()
    {
        targetPosition.y = Mathf.Max(targetPosition.y, minimumYPosition);
    }

    public void CheckStateAndEnableFollowing()
    {
        if (GameManager.Instance.CurrentState == GameManager.State.GameOver)
        {
            isFollowing = false;
            transform.position = startingPosition;
            velocity = Vector3.zero;
        }
        else if (GameManager.Instance.CurrentState == GameManager.State.Playing)
        {
            isFollowing = true;
        }
    }
}
