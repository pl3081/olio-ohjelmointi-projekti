using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls camera movement
public class cameraControls : MonoBehaviour
{
    public float cameraSpeed = 1;
    public float sensitivity = 2;

    private Vector3 startPosition;
    private Vector3 eulerAngle;
    private Transform targetOfView;

    // Start is called before the first frame update
    void Start()
    {
        targetOfView = transform.parent;
        startPosition = targetOfView.position;
        eulerAngle = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");

        if (Input.GetKey("up"))
        {
            targetOfView.position += targetOfView.forward * cameraSpeed * Time.deltaTime;
        }
        if (Input.GetKey("down"))
        {
            targetOfView.position += -targetOfView.forward * cameraSpeed * Time.deltaTime;
        }
        Vector3 right = Quaternion.Euler(0, 90, 0) * targetOfView.forward;
        if (Input.GetKey("right"))
        {
            targetOfView.position += right * cameraSpeed * Time.deltaTime;
        }
        if (Input.GetKey("left"))
        {
            targetOfView.position += -right * cameraSpeed * Time.deltaTime;
        }

        if (Input.GetMouseButton(1))
        {
            eulerAngle += Vector3.up * sensitivity * mouseX;
            targetOfView.eulerAngles = eulerAngle;
        }
        if (Input.GetKeyDown("space"))
        {
            targetOfView.position = startPosition;
            eulerAngle = Vector3.zero;
            targetOfView.eulerAngles = eulerAngle;
        }
    }
}
