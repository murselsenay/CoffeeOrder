using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClick : MonoBehaviour
{
    public static ButtonClick instance;
    Vector3 startPosition;
    internal bool canClick = false;
    public Material milkMat;
    public GameObject milk;
    internal float milkLevel = 0.1f;
    bool canDecreasePressure = true;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void FixedUpdate()
    {
        if (canDecreasePressure)
            LevelManager.instance.DecreaseMilkPressure();
    }
    private void OnMouseDown()
    {
        
        if (canClick)
        {
            
            foreach (GameObject coffeeFlow in LevelManager.instance.coffeeFlows)
            {
                if (coffeeFlow.GetComponent<Renderer>().material != milkMat)
                {
                    coffeeFlow.GetComponent<Renderer>().material = milkMat;
                }
                
            }
            gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, 0.7f);
        }
       
       
      
    }
    private void OnMouseDrag()
    {

        if (canClick)
        {
            if (gameObject.tag == "Milk")
            {
                foreach (GameObject coffeeFlow in LevelManager.instance.coffeeFlows)
                {
                    Vector3 scale = coffeeFlow.transform.localScale;
                    scale.z += milkLevel;
                    coffeeFlow.transform.localScale = scale;
                }
                canDecreasePressure = false;
                LevelManager.instance.IncreaseMilkPressure();
                milk.transform.position += new Vector3(0, -0.001f, 0);
            }
        }
        
    }
    private void OnMouseUp()
    {
        if (canClick)
        {
            gameObject.transform.position = startPosition;
            if (gameObject.tag == "Milk")
            {
                canDecreasePressure = true;
                LevelManager.instance.StopFillMilk();
            }
        }
       
    }
}
