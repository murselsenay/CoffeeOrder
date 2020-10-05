using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flow : MonoBehaviour
{

    Vector3 startScale;


    float shots = 0;
    float targetShots;

  

    // Start is called before the first frame update
    void Start()
    {


        startScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartFlowing()
    {
        targetShots = LevelManager.instance.targetShot;
        if (gameObject.activeSelf)
        {
            LevelManager.instance.activeFlowCount += 1;
            StartCoroutine(Flowing());
        }


    }

    public IEnumerator Flowing()
    {
        while (shots <= targetShots)
        {
            Vector3 scale = transform.localScale;
            scale.z += 0.1f;
            transform.localScale = scale;
            shots += 0.08f;
            yield return new WaitForSeconds(0.1f);
        }
        
        StartCoroutine(StopFlowing());

    }
    public IEnumerator StopFlowing()
    {
        float rnd = Random.Range(0.01f, 0.05f);
        while (shots >= 0)
        {
            Vector3 scale = transform.localScale;
            scale.z -= 0.1f;
            transform.localScale = scale;
            shots -= 0.08f;
            yield return new WaitForSeconds(rnd);
        }
        LevelManager.instance.ThirdStep();
    }




    public void StopMilkFlowing()
    {
        if (gameObject.activeSelf)
        {
            StartCoroutine(StopMilkFlowCoroutine());
        }
       
    }
    public IEnumerator StopMilkFlowCoroutine()
    {
        while (transform.localScale.z > startScale.z)
        {
            Vector3 scale = transform.localScale;
            scale.z -= 0.2f;
            transform.localScale = scale;
            shots -= 0.05f;
            yield return new WaitForSeconds(0.002f);
        }
    }

}
