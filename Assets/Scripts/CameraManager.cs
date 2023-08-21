using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Object Reference")]
    [SerializeField] private PlayerController character = null;
    [SerializeField] private float distanceFollow = 10f;
    [SerializeField] private float leftOffsetfollow = 3f;

    [Header("Configuration")]
    [SerializeField] private float smoothTime = 0.01f;

    private Transform followTarget => character.transform;

    private Vector3 target;
    private Vector3 refVelocity = Vector3.zero;



    private void LateUpdate()
    {
        target = new Vector3(followTarget.position.x + leftOffsetfollow, transform.position.y, followTarget.transform.position.z - distanceFollow);
        transform.position = Vector3.SmoothDamp(transform.position, target, ref refVelocity, smoothTime);
    }
}
