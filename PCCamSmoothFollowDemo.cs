using UnityEngine;
using System.Collections;
using Prime31;

/// <summary>
/// For camera Effects - alpha version
/// Attach to camera object
/// Follows target object - PC
/// </summary>
public class PCCamSmoothFollowDemo : MonoBehaviour
{
    public Transform target;
    public float smoothDampTime = 0.2f;
    [HideInInspector]
    public new Transform transform;
    public Vector3 cameraOffset;
    public bool useFixedUpdate = false;

    private RoboController _playerController;
    private Vector3 _smoothDampVelocity;
    private float lastZCam;


    void Awake()
    {
        transform = gameObject.transform;
        lastZCam = transform.position.z;
        _playerController = target.GetComponent<RoboController>();
    }


    void LateUpdate()
    {
        if (!useFixedUpdate)
            updateCameraPosition();
    }


    void FixedUpdate()
    {
        if (useFixedUpdate)
            updateCameraPosition();
    }


    void updateCameraPosition()
    {
        if (_playerController == null)
        {
            transform.position = Vector3.SmoothDamp(transform.position, target.position - cameraOffset, ref _smoothDampVelocity, smoothDampTime);
            return;
        }

        if (_playerController.velocity.x > 0)
        {
            transform.position = Vector3.SmoothDamp(transform.position, target.position - cameraOffset, ref _smoothDampVelocity, smoothDampTime);
        }
        else
        {
            var leftOffset = cameraOffset;
            leftOffset.x *= -1;
            transform.position = Vector3.SmoothDamp(transform.position, target.position - leftOffset, ref _smoothDampVelocity, smoothDampTime);
        }

        Vector3 fixZ = transform.position;
        fixZ = new Vector3(transform.position.x, transform.position.y, lastZCam);
        transform.position = fixZ;

    }

}
