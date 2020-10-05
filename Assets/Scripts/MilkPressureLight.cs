using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkPressureLight : MonoBehaviour
{
    Material startMaterial;
    public Material defaultMaterial;
    // Start is called before the first frame update
    void Start()
    {
        startMaterial = gameObject.GetComponent<Renderer>().material;
        gameObject.GetComponent<Renderer>().material = defaultMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SwitchOn()
    {
        gameObject.GetComponent<Renderer>().material = startMaterial;
    }
    public void SwitchOff()
    {
        gameObject.GetComponent<Renderer>().material = defaultMaterial;
    }
}
