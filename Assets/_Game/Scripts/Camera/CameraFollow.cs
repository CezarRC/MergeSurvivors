using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    Group group;
    private void Start()
    {
        group = GameObject.FindObjectOfType<Group>();
    }
    void FixedUpdate()
    {
        Follow();
    }

    void Follow()
    {
        Vector3 newPosition = Vector3.Lerp(transform.position, group.transform.position, group.GetGroupSpeed() * Time.deltaTime);
        newPosition.z = newPosition.y-100;
        GetComponent<Camera>().orthographicSize = Mathf.Lerp(GetComponent<Camera>().orthographicSize, Mathf.Min(11 + 1 * group.GetGroupSpeed(), 25), 2f * Time.deltaTime);
        transform.position = newPosition;
    }
}
