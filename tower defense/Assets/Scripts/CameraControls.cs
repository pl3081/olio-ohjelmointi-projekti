using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls camera movement
public class CameraControls : MonoBehaviour
{
    [SerializeField] float cameraSpeed = 1;
    [SerializeField] float sensitivity = 2;

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
        // by buttons
        if (Input.GetKey("w"))
        {
            targetOfView.position += targetOfView.forward * cameraSpeed * Time.deltaTime;
        }
        if (Input.GetKey("s"))
        {
            targetOfView.position += -targetOfView.forward * cameraSpeed * Time.deltaTime;
        }
        Vector3 right = Quaternion.Euler(0, 90, 0) * targetOfView.forward;
        if (Input.GetKey("d"))
        {
            targetOfView.position += right * cameraSpeed * Time.deltaTime;
        }
        if (Input.GetKey("a"))
        {
            targetOfView.position += -right * cameraSpeed * Time.deltaTime;
        }
        // rmb rotating
        if (Input.GetMouseButton(1))
        {
            eulerAngle += Vector3.up * sensitivity * mouseX;
            targetOfView.eulerAngles = eulerAngle;
        }
        // reset
        if (Input.GetKeyDown("space"))
        {
            targetOfView.position = startPosition;
            eulerAngle = Vector3.zero;
            targetOfView.eulerAngles = eulerAngle;
        }
        // hover
        Vector3 mousePos = Input.mousePosition;
        Vector3 screenCenter = new Vector3(Screen.width / 2, 0, Screen.height / 2);

        if(mousePos.x < 5 || mousePos.x > Screen.width - 5 || mousePos.y < 5 || mousePos.y > Screen.height - 5)
        {
            Vector3 convertedMousePos = new Vector3(mousePos.x, 0, mousePos.y);
            targetOfView.position += Quaternion.Euler(eulerAngle) * Vector3.Normalize(convertedMousePos - screenCenter) * cameraSpeed * Time.deltaTime;
        }
    }
}
