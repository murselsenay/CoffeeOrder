using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cup : MonoBehaviour
{
    public static Cup instance;
    [Header("Materials")]
    public Material opaqueOrangeMat;
    public Material transparentOrangeMat;
    [Header("Cup Size")]
    public bool isSmallCup;
    public bool isMediumCup;
    public bool isLargeCup;

    float t;
    float timeToReachTarget = 0.5f;

    internal GameObject target;
    Vector3 startPosition;
    internal Vector3 startPositionForSyrup;
    GameObject coffee;
    Renderer cupRenderer;
    bool canCheckCoffeeOverFlow = true;
    internal bool canMove = true;
    internal bool canDrag = false;
    internal bool canReleaseCup = false;

    private Vector3 screenPoint;
    private Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        startPosition = transform.position;
        coffee = gameObject.transform.GetChild(2).gameObject;
        cupRenderer = gameObject.GetComponent<Renderer>();
        OpaqueRenderMode();
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime / timeToReachTarget;
        CheckCoffeeOverFlow();
        if (canMove)
        {
            transform.position = Vector3.Lerp(startPosition, target.transform.position, t);
        }


        //if (canFlipCup)
        //transform.eulerAngles += new Vector3(-2f, 0f, 0f);

    }

    void OnMouseDown()
    {

        if (canDrag)
        {
            screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
            offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        }
        canReleaseCup = false;

    }

    void OnMouseDrag()
    {
        if (canDrag)
        {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
            transform.position = new Vector3(Mathf.Clamp(curPosition.x, 3.30f, 4.65f), Mathf.Clamp(curPosition.y, 0.6f, 0.6f), Mathf.Clamp(curPosition.z, -2f, -1f));
        }

        canReleaseCup = false;
    }
    private void OnMouseUp()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "TopCollider")
        {
            startPosition = transform.position;
            target = GameObject.FindGameObjectWithTag("CupPosition");
            t = 0;
            timeToReachTarget = 1f;
        }
        if (other.gameObject.tag == "CupPosition")
        {
            transform.eulerAngles = new Vector3(-90, 0, 0);
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "CoffeeFlow")
        {
            Vector3 scale = coffee.transform.localScale;
            if (LevelManager.instance.activeFlowCount == 1)
                scale.z += 0.0025f;
            if (LevelManager.instance.activeFlowCount == 2)
                scale.z += 0.001f;

            if (coffee.transform.localScale.z >= 0.9f)
            {
                scale.y = 1f;
                scale.x = 1f;
            }
            if (coffee.transform.localScale.z < 0.85f)
            {
                coffee.transform.localScale = scale;
            }
        }

        if (other.gameObject.tag == "CupPosition" && other.gameObject.transform.position.x == gameObject.transform.position.x)
        {
            startPositionForSyrup = transform.position;
            canMove = false;
            LevelManager.instance.canCheckWrongSyrup = true;
        }
        if (other.gameObject.tag == "CupDetector")
        {
            LevelManager.instance.selectedSyrup = other.gameObject.transform.parent.transform.parent.gameObject;
            LevelManager.instance.CheckSyrupInput(other.gameObject.transform.parent.transform.parent.gameObject.name);
            if (canReleaseCup)
            {
                other.gameObject.tag = "Untagged";
                transform.position = new Vector3(other.transform.position.x, transform.position.y, other.transform.position.z);
                SwipeDetector.instance.canSwipe=true;
                canDrag = false;
                canReleaseCup = false;
            }
            else
            {
                transform.position = startPositionForSyrup;
                LevelManager.instance.canCheckWrongSyrup = false;
            }

        }
    }

    public void GotoNextStep()
    {
        canMove = true;
        t = 0;
        startPosition = transform.position;
        GameObject.FindGameObjectWithTag("LevelManager").transform.position = new Vector3(0f, 0.1f, 0f);
        TrasparentRenderMode();
    }
    public void TrasparentRenderMode()
    {
        cupRenderer.material = transparentOrangeMat;
    }

    public void OpaqueRenderMode()
    {
        cupRenderer.material = opaqueOrangeMat;
    }
    public void CheckCoffeeOverFlow()
    {
        if (canCheckCoffeeOverFlow)
        {
            if (gameObject.transform.GetChild(2).localScale.z >= 0.85f)
            {
                LevelManager.instance.ThumbsUp();
                LevelManager.instance.StopFillMilk();
                LevelManager.instance.ThirdStepAction();
                ButtonClick.instance.canClick = false;
                canCheckCoffeeOverFlow = false;
            }
        }

    }
    public void ResetPosition()
    {
        t = 0;
        startPosition = transform.position;
        canMove = true;
    }
    
}
