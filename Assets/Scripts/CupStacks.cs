using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupStacks : MonoBehaviour
{
    public static CupStacks instance;

    [Header ("Cup Stack Size")]
    public bool isSmallCupStacks;
    public bool isMediumCupStacks;
    public bool isLargeCupStacks;
    [Space]
    public GameObject cupPrefab;
    GameObject[] cupStacks;
    Quaternion q;

    
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        q = Quaternion.Euler(90, 0, 0);
        cupStacks = GameObject.FindGameObjectsWithTag("CupStacks");
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnMouseDown()
    {
        DisableCollider();
        GameObject cup = Instantiate(cupPrefab, transform.GetChild(0).position, q);
        cup.GetComponent<Cup>().target = transform.GetChild(2).gameObject;
    }
    public void DisableCollider()
    {
        foreach (var cupStack in cupStacks)
        {
            cupStack.GetComponent<CapsuleCollider>().enabled = false;
        }
        Invoke("EnableCollider", 2f);
        
    }
    public void EnableCollider()
    {
        foreach (var cupStack in cupStacks)
        {
            if (cupStack.GetComponent<CapsuleCollider>()!=null)
            {
                cupStack.GetComponent<CapsuleCollider>().enabled = true;
            }
            
        }
    }
    public void DestroyCollider()
    {
        foreach (var cupStack in cupStacks)
        {
            Destroy(cupStack.GetComponent<CapsuleCollider>());
            
        }
    }
}
