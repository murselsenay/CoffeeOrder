using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovements : MonoBehaviour
{
    public static CameraMovements instance;

    internal Vector3 startPosition;
    internal Vector3 targetPosition;
    bool canGotoNextStep = false;


    float t;
    float timeToReachTarget = 1f;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        startPosition = transform.position;
        targetPosition = new Vector3(0f, 5f, -5.8f);
    }

    // Update is called once per frame
    void Update()
    {
        if (canGotoNextStep)
        {
            if (transform.position!=targetPosition)
            {
                t += Time.deltaTime / timeToReachTarget;
                transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            }
            else
            {
                canGotoNextStep = false;
            }
            
        }
    }
    public void GotoNextStep(Vector3 _startPosition, Vector3 _targetPosition)
    {
        t = 0;
        startPosition = _startPosition;
        targetPosition = _targetPosition;
        canGotoNextStep = true;
    }
}
