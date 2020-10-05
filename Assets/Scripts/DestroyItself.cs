using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyItself : MonoBehaviour
{
    public int destroyTime;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(); 
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    void Destroy()
    {
        Destroy(gameObject, destroyTime);
    }
}
