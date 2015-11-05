using UnityEngine;
using System.Collections;

public class OrbitTarget : MonoBehaviour
{
    [Tooltip("Toggles orbiting")] public bool Active = true;
    [Tooltip("Toggles staying or moving toward the target when orbiting is turned off")] public bool HoldOnInactive = false;
    [Range(0f, 100f)] public float Distance = 10f;
    [Range(-90f, 90f)] public float Tilt = 1f;
    public float Rotation = 0f;
    [Range(0.1f, 20f)] public float MoveDamping = 1f;
    [Range(0.1f, 20f)] public float LookDamping = 1f;
    [Range(-360f, 360f)] public float AutoOrbitSpeed = 0f;

    [SerializeField] private Transform _target;

    private void LateUpdate()
    {
        if (_target == null)
        {
            return;
        }

        if (!Active && HoldOnInactive)
        {
            return;
        }

        Vector3 targetPos;

        if (Active)
        {
            Rotation += AutoOrbitSpeed*Time.deltaTime;

            var tiltRads = Tilt*Mathf.Deg2Rad;
            var tiltL = Mathf.Cos(tiltRads);
            var tiltH = Mathf.Sin(tiltRads);
            var rotRads = Rotation*Mathf.Deg2Rad;
            var rotX = Mathf.Sin(rotRads);
            var rotZ = Mathf.Cos(rotRads);
            targetPos = _target.position + new Vector3(tiltL*rotX*-1f, tiltH, tiltL*rotZ*-1f)*Distance;

        }
        else
        {
            targetPos = _target.position;
        }

        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime*MoveDamping);

        var toTarget = _target.position - transform.position;
        var lookRotation = (toTarget != Vector3.zero) ? Quaternion.LookRotation(toTarget) : Quaternion.identity;
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime*LookDamping);
    }
}
