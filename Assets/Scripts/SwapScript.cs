using UnityEngine;
using System.Collections;

public class SwapScript : MonoBehaviour {

    public int ButtonNumber;
    public Transform innerRing;
    public Transform outerRing;
    public CheckWin won;
    public delegate void SwapAction();
    public static event SwapAction OnSwap;
    private float resetTime = 0;

#if UNITY_EDITOR
    void OnMouseDown()
    {
        Swap();
    }
#endif

#if (UNITY_IOS || UNITY_ANDROID || UNITY_WSA)
    void Update()
    {
        if (Input.touchCount == 1)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position), Vector2.zero);
                if (hit.collider == GetComponent<BoxCollider2D>())
                {
                    if (resetTime > Time.time)
                    {
                        resetTime = Time.time + 0.1f;
                        Swap();
                    }
                }
            }
        }
    }
#endif

    void Swap()
    {
        if (!won.Won)
        {
            int i = mod(ButtonNumber - innerRing.gameObject.GetComponent<Movement>().rotation, innerRing.childCount);
            int j = mod(ButtonNumber - outerRing.gameObject.GetComponent<Movement>().rotation, outerRing.childCount);

            int colnum = innerRing.GetChild(i).gameObject.GetComponent<SegmentColor>().color;

            innerRing.GetChild(i).gameObject.GetComponent<SegmentColor>().SetColor(outerRing.GetChild(j).gameObject.GetComponent<SegmentColor>().color);
            outerRing.GetChild(j).gameObject.GetComponent<SegmentColor>().SetColor(colnum);

            if (OnSwap != null)
                OnSwap();
        }
    }

    int mod(int a, int n)
    {
        int result = a % n;
        if ((a < 0 && n > 0) || (a > 0 && n < 0))
            result += n;
        return result;
    }
}
