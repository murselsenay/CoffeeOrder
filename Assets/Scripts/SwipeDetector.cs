using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeDetector : MonoBehaviour
{
    public static SwipeDetector instance;
    private Vector2 fingerDown;
    private Vector2 fingerUp;
    public bool detectSwipeOnlyAfterRelease = false;
    public Text text;
    public float SWIPE_THRESHOLD = 20f;
    internal bool canSwipe = false;
    internal bool isDone = false;

    // Update is called once per frame
    void Update()
    {
        instance = this;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnSwipeDown();
        }

        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {

                fingerUp = touch.position;
                fingerDown = touch.position;
            }

            //Detects Swipe while finger is still moving
            if (touch.phase == TouchPhase.Moved)
            {

                if (!detectSwipeOnlyAfterRelease)
                {
                    fingerDown = touch.position;

                }
            }

            //Detects swipe after finger is released
            if (touch.phase == TouchPhase.Ended)
            {

                fingerDown = touch.position;
                checkSwipe();
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnSwipeDown();
        }

    }

    void checkSwipe()
    {
        //Check if Vertical swipe
        if (verticalMove() > SWIPE_THRESHOLD && verticalMove() > horizontalValMove())
        {
            //Debug.Log("Vertical");
            if (fingerDown.y - fingerUp.y > 0)//up swipe
            {
                OnSwipeUp();
            }
            else if (fingerDown.y - fingerUp.y < 0)//Down swipe
            {
                OnSwipeDown();
            }
            fingerUp = fingerDown;
        }
        else if (horizontalValMove() > SWIPE_THRESHOLD && horizontalValMove() > verticalMove())
        {
            if (fingerDown.x - fingerUp.x > 0)//Right swipe
            {
                OnSwipeRight();
            }
            else if (fingerDown.x - fingerUp.x < 0)//Left swipe
            {
                OnSwipeLeft();
            }
            fingerUp = fingerDown;
        }

        else
        {
            //Debug.Log("No Swipe!");
        }
    }

    float verticalMove()
    {
        return Mathf.Abs(fingerDown.y - fingerUp.y);
    }

    float horizontalValMove()
    {
        return Mathf.Abs(fingerDown.x - fingerUp.x);
    }

    void OnSwipeUp()
    {

    }

    void OnSwipeDown()
    {
        if (canSwipe && !isDone)
        {
            LevelManager.instance.selectedSyrup.transform.GetChild(3).GetComponent<SyrupFlow>().StartFlowing(LevelManager.instance.selectedSyrup.GetComponent<Renderer>().material);
            StartCoroutine(PushButton());
            canSwipe = false;
            LevelManager.instance.syrupPushCount += 1;
            if (LevelManager.instance.CheckSyrupPushCount(LevelManager.instance.syrupPushCount))
            {
                LevelManager.instance.SyrupPushIsDone();
                isDone = true;
            }
        }

    }

    void OnSwipeLeft()
    {

    }

    void OnSwipeRight()
    {

    }

    public IEnumerator PushButton()
    {

        while (LevelManager.instance.selectedSyrup.transform.GetChild(1).GetChild(0).transform.position.y > 1.22f)
        {
            LevelManager.instance.selectedSyrup.transform.GetChild(1).GetChild(0).transform.position += new Vector3(0f, -0.01f, 0f);
            Debug.Log(LevelManager.instance.selectedSyrup.transform.GetChild(1).GetChild(0).transform.position);
            yield return new WaitForSeconds(0.03f);
        }

        while (LevelManager.instance.selectedSyrup.transform.GetChild(1).GetChild(0).transform.position.y < 1.4f)
        {
            LevelManager.instance.selectedSyrup.transform.GetChild(1).GetChild(0).transform.position += new Vector3(0f, +0.01f, 0f);
            Debug.Log(LevelManager.instance.selectedSyrup.transform.GetChild(1).GetChild(0).transform.position);
            yield return new WaitForSeconds(0.03f);
        }
        canSwipe = true;


    }
}
