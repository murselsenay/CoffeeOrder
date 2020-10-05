using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupPosition : MonoBehaviour
{
    public static CupPosition instance;

    [Header("ThumbsUp&Down Prefabs")]
    public GameObject thumbsUp, thumbsDown;
    internal string cupSize = null;

    internal int step = 1;

    GameObject correctCoffeeCup = null;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "SmallCup")
            cupSize = other.gameObject.tag;

        if (other.gameObject.tag == "MediumCup")
            cupSize = other.gameObject.tag;

        if (other.gameObject.tag == "LargeCup")
            cupSize = other.gameObject.tag;

        LevelManager.instance.CheckCupSize();
        if (step == 1)
        {
            if (!LevelManager.instance.CheckCupSize())
            {
                Destroy(other.gameObject, 1);
                Instantiate(thumbsDown, new Vector3(-4, 3f, 0f), Quaternion.identity);
                LevelManager.instance.UpdateScore(-5);
            }
            else
            {
                step += 1;
                correctCoffeeCup = other.gameObject;
                Instantiate(thumbsUp, new Vector3(-4, 3f, 0f), Quaternion.identity);
                Invoke("GotoNextStep", 1f);
                CupStacks.instance.DestroyCollider();
                LevelManager.instance.SecondStep();
                LevelManager.instance.UpdateScore(5);
            }
        }


    }
    void GotoNextStep()
    {
        correctCoffeeCup.GetComponent<Cup>().GotoNextStep();
        CameraMovements.instance.GotoNextStep(CameraMovements.instance.transform.position, CameraMovements.instance.transform.position + new Vector3(4f,0f,-2f));
    }

}
