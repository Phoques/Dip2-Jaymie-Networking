using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 20f;
    public Transform player;
    private float _xRotation = 0f;

    private void Start()
    {
        LockCursor();
    }

    private void Update()
    {
        MouseMove();
    }
    private void MouseMove()
    {
        //This takes in the X & Y input of the mouse.
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // the X rotation needs to be minus the mouseY input, otherwise the controls are inverted.
        //This is our look up and down.
        _xRotation -= mouseY;
        //This stops the player from looking higher than 85deg, and -85deg on the up and down axis.
        _xRotation = Mathf.Clamp(_xRotation, -85f, 85f);

        //Local rotation takes the local position of the object the script is on (in this case the camera)
        //This rotates AROUND the xRotation variable, which just pivots the camera.
        transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);

        //This takes the Transform of the player, and makes it Rotate around an axis.
        //Which in this case is the Y axis (Vector3.up) based on the mouseX input.
        player.Rotate(Vector3.up * mouseX);
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
