using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyrupFlow : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartFlowing(Material mat)
    {
        gameObject.GetComponent<Renderer>().material = mat;
        StartCoroutine(Flowing());
    }

    public IEnumerator Flowing()
    {
        while (gameObject.transform.localScale.z < 0.6f)
        {
            Vector3 scale = transform.localScale;
            scale.z += 0.5f;
            transform.localScale = scale;
            yield return new WaitForSeconds(0.08f);
        }
        StartCoroutine(StopFlowing());
    }

    public IEnumerator StopFlowing()
    {
        while (gameObject.transform.localScale.z > 0.05f)
        {
            Vector3 scale = transform.localScale;
            scale.z -= 0.07f;
            transform.localScale = scale;
            yield return new WaitForSeconds(0.08f);
        }

    }
}
