using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanerController : MonoBehaviour
{
    public float mouseHorizontalSpeed = 1.0f;
    public float mouseVerticalSpeed = 1.0f;
    public float speed = 1.0f;
    private float angleHorizontal = 0.0f;
    private float angleVertical = 0.0f;
    //最大可视角度
    public float MaxVerticalViewAngle = 80f;

    //是否垂直方向反转
    public bool reverseVertical = true;

    private bool _isLockMouse;
    public bool LockMouse
    {
        set
        {
            if (value)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            _isLockMouse = value;
        }
        get
        {
            return _isLockMouse;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float h = mouseHorizontalSpeed * Input.GetAxis("Mouse X");
        float v = mouseVerticalSpeed * Input.GetAxis("Mouse Y");
        angleHorizontal += h;
        if (reverseVertical)
        {
            angleVertical -= v;
        }
        else
        {
            angleVertical += v;
        }
        if (angleVertical < -MaxVerticalViewAngle)
        {
            angleVertical = -MaxVerticalViewAngle;
        }
        if (angleVertical > MaxVerticalViewAngle)
        {
            angleVertical = MaxVerticalViewAngle;
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            this.LockMouse = !this.LockMouse;
        }
        transform.rotation = Quaternion.Euler(new Vector3(angleVertical, angleHorizontal, 0));

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 velocity = new Vector3();
        if (Mathf.Abs(moveHorizontal) > 0.0f || Mathf.Abs(moveVertical) > 0.0f)
        {
            //计算速度
            velocity = (transform.forward * moveVertical + transform.right * moveHorizontal) * speed;
        }
        var newPosition = transform.position + Time.deltaTime * velocity;
        transform.position = newPosition;

        //this.transform.position = follow.transform.position + new Vector3(followOffsetX, followOffsetY, followOffsetZ) - transform.forward * followDistance;

        //this.transform.Rotate()

    }
}
