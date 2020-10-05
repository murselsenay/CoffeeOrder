using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    public static ButtonController instance;



    Color startColor;
    GameObject[] buttons;
    internal bool shotButtons = false;
    internal bool milkButtons = false;
    void Start()
    {
        instance = this;

    }
    public void ButtonClick(int shot)
    {
        buttons = GameObject.FindGameObjectsWithTag("TouchButton");
        startColor = Color.white;


        if (shotButtons)
        {
          

            if (LevelManager.instance.targetShot == shot)
            {
                LevelManager.instance.CheckShotInput(shot);
                CorrectSelection();
            }
            else
            {
                LevelManager.instance.CheckShotInput(shot);
                WrongSelection();
            }
        }

        if (milkButtons)
        {

            if (EventSystem.current.currentSelectedGameObject!=null)
            {
                if (EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<Text>().text == LevelManager.instance.targetMilkType)
                {
                    CorrectSelection();
                    LevelManager.instance.CheckMilkInput(LevelManager.instance.targetMilkType);
                }
                else
                {
                    LevelManager.instance.CheckMilkInput(null);
                    WrongSelection();
                }
            }
           
        }




    }

    public void ChangeTouchButtonsFunction(string buttonFunction)
    {
        if (buttonFunction == "MilkButtons")
        {
            milkButtons = true;
            shotButtons = false;
        }

        if (buttonFunction == "ShotButtons")
        {
            milkButtons = false;
            shotButtons = true;
        }

    }



    public void SetColorToDefault()
    {

        foreach (GameObject button in buttons)
        {
            button.GetComponent<Image>().color = startColor;
            button.GetComponent<Button>().enabled = true;
            button.GetComponent<Button>().interactable = true;
        }
    }
    void WrongSelection()
    {
        EventSystem.current.currentSelectedGameObject.GetComponent<Image>().color = Color.red;
        foreach (GameObject button in buttons)
        {
            button.GetComponent<Button>().interactable = false;
        }
        Invoke("SetColorToDefault", 1f);
    }

    void CorrectSelection()
    {
        EventSystem.current.currentSelectedGameObject.GetComponent<Image>().color = Color.green;
        foreach (GameObject button in buttons)
        {
            button.GetComponent<Button>().interactable = false;
        }
    }

    public void CloseMenu()
    {
        Destroy(EventSystem.current.currentSelectedGameObject.transform.parent.gameObject);
    }
    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
}
