using UnityEngine;
using System.Collections;

public class OrbitTarget : MonoBehaviour
{
    public float TargetDistance = 10f;
    public float TargetHeight = 1f;
    public float TargetRotation = 0f;
    public float DistanceDamping = 1f;
    public float HeightDamping = 1f;
    public float RotationDamping = 1f;
    public float LookDamping = 1f;
    public float AutoOrbitSpeed = 0f;

    [SerializeField] private Transform _target;

    private void LateUpdate()
    {
        if (_target == null)
        {
            return;
        }

        TargetRotation += AutoOrbitSpeed*Time.deltaTime;

        var toMe = transform.position - _target.position;
        var currentDistance = toMe.magnitude;
        var currentHeight = toMe.y;
        var currentRotation = Mathf.Atan2(toMe.z, toMe.x) * Mathf.Rad2Deg;

        var newDistance = Mathf.Lerp(currentDistance, TargetDistance, Time.deltaTime*DistanceDamping);
        var newHeight = Mathf.Lerp(currentHeight, TargetHeight, Time.deltaTime*HeightDamping);
        var newRotation = Mathf.LerpAngle(currentRotation, TargetRotation, Time.deltaTime*RotationDamping);

        var newRotationRads = newRotation*Mathf.Deg2Rad;
        var newRotationDir = new Vector3(Mathf.Cos(newRotationRads), 0f, Mathf.Sin(newRotationRads)) * newDistance;
        newRotationDir.y = newHeight;
        transform.position = _target.position + newRotationDir.normalized*newDistance;

        var lookRotation = Quaternion.LookRotation(_target.position - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime*LookDamping);
    }
}
