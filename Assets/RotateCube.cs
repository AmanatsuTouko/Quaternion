using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCube : MonoBehaviour
{
    public GameObject unityObj;
    public GameObject originalObj;

    public GameObject targetObj;

    public Quaternion unityQuaternion;
    public FThingSoftware.Quaternion originalQuaternion;

    [Header("Angle and Axis")]
    public float angle = 0;
    public Vector3 axis = Vector3.zero;

    [Header("FromDirection and ToDirection")]
    public Vector3 fromDirection;
    public Vector3 toDirection;

    void Start()
    {
        // unityQuaternion = Quaternion.identity;
        // unityQuaternion = Quaternion.AngleAxis(0, transform.up);

        // originalQuaternion = FThingSoftware.Quaternion.AngleAxis(30, transform.up);
        // originalObj.transform.rotation = originalQuaternion.ToUnity();
        // Debug.Log(originalQuaternion.eulerAngle);
    }

    void Update()
    {
        // Test AngleAxis()
        // unityQuaternion = Quaternion.AngleAxis(angle, axis);
        // unityObj.transform.rotation = unityQuaternion;
        // originalQuaternion = FThingSoftware.Quaternion.AngleAxis(angle, axis);
        // originalObj.transform.rotation = originalQuaternion.ToUnity();

        // Test SetFromToRotation()
        // unityQuaternion.SetFromToRotation(fromDirection, toDirection);
        // originalQuaternion.SetFromToRotation(fromDirection, toDirection);

        // Test eulerAngle
        // unityQuaternion = Quaternion.AngleAxis(angle, axis);
        // unityObj.transform.rotation = unityQuaternion;
        // Debug.Log(unityQuaternion.eulerAngles);

        // originalQuaternion = FThingSoftware.Quaternion.AngleAxis(angle, axis);
        // originalObj.transform.rotation = originalQuaternion.ToUnity();
        // Debug.Log(originalQuaternion.eulerAngle);

        // unityQuaternion.eulerAngles = axis;
        // unityObj.transform.rotation = unityQuaternion;

        // originalQuaternion.eulerAngle = axis;
        // originalObj.transform.rotation = originalQuaternion.ToUnity();

        // Test LookRotation
        Vector3 look = targetObj.transform.position - this.transform.position;
        unityQuaternion = Quaternion.LookRotation(look);
        unityObj.transform.rotation = unityQuaternion;
    }
}
