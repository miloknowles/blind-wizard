using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Forces the camera to follow the wizard at a given height.
public class CameraFollower : MonoBehaviour
{
    public Transform objectTransformToFollow;
    private Vector3 offset = new Vector3(0, 0, -15);

    // Update is called once per frame
    void Update()
    {
        this.transform.position = objectTransformToFollow.position + offset;
    }
}