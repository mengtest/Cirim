using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

    private int snapAngle = 0;
    private bool dragging = false;
    private bool firstMove = false;
    private float prevAngle = 0;
    public float speed = 1f;
    public int rotation = 0;
    public LevelStateListener won;

    public delegate void RotateAction();
    public static event RotateAction OnRotate;

    void Awake()
    {
        snapAngle = 360 / transform.childCount;
    }

#if UNITY_EDITOR
    void OnMouseDown()
    {
        StartDrag();
    }

    void OnMouseUp()
    {
        EndDrag();
    }
#endif

    // Update is called once per frame
    void Update () {
#if (UNITY_IOS || UNITY_ANDROID || UNITY_WSA)
        if (Input.touchCount == 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position), Vector2.zero);
                if (hit.collider == GetComponent<CircleCollider2D>())
                {
                    StartDrag();
                }
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                EndDrag();
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                Rotate();
            }
        }
#endif

#if UNITY_EDITOR
        Rotate(true);
#endif
    }

    private void StartDrag()
    {
        if (!won.Won)
        {
            StopCoroutine("SnapToRotation");
            dragging = true;
            firstMove = true;
        }
    }

    private void EndDrag()
    {
        dragging = false;

        if (!won.Won)
        {
            rotation = Mathf.RoundToInt(transform.rotation.eulerAngles.z / snapAngle) % transform.childCount; //Find the rotation (according to the snap angles) of the ring
            float tempAngle = (rotation * snapAngle) % 360; //Multiply the rotation by snap angle to find the absolute angle

            StartCoroutine("SnapToRotation", Quaternion.Euler(0, 0, tempAngle)); //Snap

            //Call On Rotate event
            if (OnRotate != null)
                OnRotate();
        }
    }

    private void Rotate(bool mouse = false)
    {
        if (dragging)
        {
            float angle = Mathf.Atan2((mouse ? Input.mousePosition.y : Input.GetTouch(0).position.y) - (Screen.height / 2), (mouse ? Input.mousePosition.x : Input.GetTouch(0).position.x) - (Screen.width / 2)) * Mathf.Rad2Deg;

            if (firstMove)
            {
                prevAngle = angle;
                firstMove = false;
            }

            transform.Rotate(new Vector3(0, 0, (angle - prevAngle) * speed));

            prevAngle = angle;
        }

        
    }

    public float smoothningTime = 0.2f;

    IEnumerator SnapToRotation(Quaternion angle)
    {
        while (Quaternion.Angle(angle, transform.rotation) > 0.1f) {

            transform.rotation = Quaternion.Slerp(transform.rotation, angle, smoothningTime * Time.deltaTime);

            yield return null;
        }
    }
}
