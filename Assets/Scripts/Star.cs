using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
    Vector3 startPosition;
    Vector3 targetPosition;

    float t;
    float timeToReachTarget = 1f;

    bool canGo = false;
    // Start is called before the first frame update
    void Start()
    {

        targetPosition = GameObject.FindGameObjectWithTag("ScoreTarget").transform.position;
        Invoke("CanGoTrue", 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (canGo)
        {
            transform.Rotate(Vector3.forward * 10f);
            t += Time.deltaTime / timeToReachTarget;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
        }
    }

    void CanGoTrue()
    {
        startPosition = transform.position;
        canGo = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "ScoreTarget")
        {
            Destroy(gameObject);
        }
    }
}
