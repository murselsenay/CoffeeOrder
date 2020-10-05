using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    //Score
    internal int score = 0;
    public Text scoreText;

    [Header("Coffee Order Text Objects")]
    public Text size;
    public Text shots;
    public Text syrup;
    public Text milk;
    public Text custom;
    public Text drink;

    [Header("Espresso Machine")]
    public Text screenText;
    GameObject[] touchButtons;
    public GameObject cameraCanvas;


    //Size
    string[] cupSize = { "SmallCup", "MediumCup", "LargeCup" };
    string selectedCupSize = null;

    //Shot

    internal int targetShot;
    internal GameObject[] coffeeFlows;
    internal int activeFlowCount = 0;


    //TouchButtons


    //Steps
    public GameObject thumbsUp, thumbsDown;
    float thumbsUpDownYPos = 0;
    int step = 1;
    public GameObject[] stepSliders;


    //Milk
    internal string[] milkTypes = { "LF", "LGT", "N" };
    internal string targetMilkType;
    public GameObject[] milkPressureLights;
    float milkPressure = 0.0f;

    //Syrup
    internal string[] syrupTypes = { "CR", "CHCL", "BRY" };
    internal string targetSyrupType;
    public GameObject selectedSyrup;
    internal bool canCheckWrongSyrup = true;


    //Menus
    [Header("Menus")]
    public GameObject failMenu;
    bool isFailed = false;

    // Start is called before the first frame update
    void Start()
    {

        instance = this;
        touchButtons = GameObject.FindGameObjectsWithTag("TouchButton");
        coffeeFlows = GameObject.FindGameObjectsWithTag("CoffeeFlow");



        SetCupSize();
        SetShotSelection();
        SetMilkSelection();
        SetSyrupSelection();


        SetEspressoMachineTouchButtonsDisabled();


        // touchButtons = GameObject.FindGameObjectsWithTag("TouchButton");
#if UNITY_EDITOR
        if (Application.platform == RuntimePlatform.WindowsEditor)
            SetEspressoMachineTouchButtonsEnabled(2, "1", "2", "3");
#endif
#if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
            SetEspressoMachineTouchButtonsEnabled(2, "2", "1", "3");
#endif



    }

    // Update is called once per frame

    //Score
    public void UpdateScore(int _score)
    {
        if (score >= 0)
        {
            score += _score;
            if (score < 0)
            {
                score = 0;
            }
        }

        scoreText.text = "Score: " + score.ToString();
    }


    //Size
    void SetCupSize()
    {
        int rnd = Random.Range(0, 3);
        selectedCupSize = cupSize[rnd];
        size.text = size.text + "\n" + cupSize[rnd].Substring(0, 1);

    }

    public bool CheckCupSize()
    {

        if (selectedCupSize == CupPosition.instance.cupSize)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    //Size



    //Steps
    public void SecondStep()//ShotStep
    {
        ButtonController.instance.ChangeTouchButtonsFunction("ShotButtons");

        for (int i = 0; i < targetShot; i++)
        {
            coffeeFlows[i].SetActive(true);
        }
        NextStep();
        UpdateLCDScreen("How many shots ?", 100);


    }
    public void SecondStepAction()//ShotStepAction
    {
        NextStep();
        FillCup();
    }
    public void ThirdStep()//MilkSelection
    {

        ButtonController.instance.ChangeTouchButtonsFunction("MilkButtons");
        ButtonController.instance.SetColorToDefault();
        UpdateLCDScreen("Select your milk: ", 100);
        ResetTouchButtons();
        Invoke("SetTouchButtonsToMilk", 0.3f);
    }
    public void ThirdStepAction()//MilkSelectionAction
    {
        milkPressureLights[0].GetComponent<MilkPressureLight>().SwitchOff();
        milkPressureLights[1].GetComponent<MilkPressureLight>().SwitchOff();
        milkPressureLights[2].GetComponent<MilkPressureLight>().SwitchOff();
        milkPressureLights[3].GetComponent<MilkPressureLight>().SwitchOff();
        milkPressureLights[4].GetComponent<MilkPressureLight>().SwitchOff();
        milkPressureLights[5].GetComponent<MilkPressureLight>().SwitchOff();
        milkPressureLights[6].GetComponent<MilkPressureLight>().SwitchOff();
        //AudioContoller.instance.source.Stop();
        NextStep();
        Invoke("ForthStep", 1f);
    }

    public void ForthStep()//SyrupSelection
    {
        Cup.instance.ResetPosition();
        Cup.instance.OpaqueRenderMode();
        Cup.instance.canDrag = true;
        thumbsUpDownYPos += 4f;
        CameraMovements.instance.GotoNextStep(CameraMovements.instance.transform.position, CameraMovements.instance.transform.position + new Vector3(4f, 0f, 1f));
        transform.position = new Vector3(4f, transform.position.y, -2f);
    }

    public void FillCup()
    {
        foreach (GameObject coffeeFlow in coffeeFlows)
        {
            coffeeFlow.GetComponent<Flow>().StartFlowing();
        }
    }

    void NextStep()
    {
        stepSliders[step - 1].GetComponent<Animator>().SetBool("canFill", true);
        cameraCanvas.transform.GetChild(step).GetComponent<Image>().color = Color.green;
        step += 1;
    }

    public void ThumbsUp()
    {
        Instantiate(thumbsUp, new Vector3(thumbsUpDownYPos, 3f, -2f), Quaternion.identity);
    }
    public void ThumbsDown()
    {
        Instantiate(thumbsDown, new Vector3(thumbsUpDownYPos, 3f, -2f), Quaternion.identity);
    }
    //Steps


    //Milk
    void SetTouchButtonsToMilk()
    {

#if UNITY_EDITOR
        if (Application.platform == RuntimePlatform.WindowsEditor)
            SetEspressoMachineTouchButtonsEnabled(3, "LF", "LGT", "N");
#endif
#if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
            SetEspressoMachineTouchButtonsEnabled(3, "LGT", "LF", "N");
#endif
        Invoke("EnableTouchButtons", 0.1f);


    }

    public void SetMilkSelection()
    {
        int rnd = Random.Range(0, milkTypes.Length);
        targetMilkType = milkTypes[rnd];
        milk.fontSize = 50;
        milk.text = milk.text + "\n" + milkTypes[rnd];
    }
    public void StopFillMilk()
    {
        foreach (GameObject coffeeFlow in coffeeFlows)
        {
            coffeeFlow.GetComponent<Flow>().StopMilkFlowing();
        }
    }
    public void CheckMilkInput(string milkInput)
    {
        if (targetMilkType == milkInput)
        {

            UpdateLCDScreen("Click Milk Button To Fill", 80);
            ButtonClick.instance.canClick = true;
            UpdateScore(5);
            ThumbsUp();

        }
        else
        {

            UpdateScore(-5);
            ThumbsDown();
        }
    }
    public void IncreaseMilkPressure()
    {
        if (milkPressure <= 30f)
        {
            if (milkPressure >= 9f)
            {
                //if (!AudioContoller.instance.source.isPlaying)
                //{
                //    AudioContoller.instance.WarningSound();
                //}
                //}

            }
            milkPressure += 0.3f;
        }
        else
        {
            if (!isFailed)
            {
                GameObject insFailMenu = Instantiate(failMenu, cameraCanvas.transform.position, Quaternion.identity);
                insFailMenu.transform.SetParent(cameraCanvas.transform);
                isFailed = true;
            }

        }

        CheckMilkPressure();
    }
    public void DecreaseMilkPressure()
    {
        AudioContoller.instance.source.Stop();
        if (milkPressure > 12f)
        {
            milkPressure = 12f;
        }
        if (milkPressure >= 0f)
        {
            milkPressure -= 0.3f;
        }
        CheckMilkPressure();
    }
    void CheckMilkPressure()
    {
        if (milkPressure <= 0f)
        {
            milkPressureLights[0].GetComponent<MilkPressureLight>().SwitchOff();
            milkPressureLights[1].GetComponent<MilkPressureLight>().SwitchOff();
            milkPressureLights[2].GetComponent<MilkPressureLight>().SwitchOff();
            milkPressureLights[3].GetComponent<MilkPressureLight>().SwitchOff();
            milkPressureLights[4].GetComponent<MilkPressureLight>().SwitchOff();
            milkPressureLights[5].GetComponent<MilkPressureLight>().SwitchOff();
            milkPressureLights[6].GetComponent<MilkPressureLight>().SwitchOff();
        }
        else if (milkPressure > 0f && milkPressure <= 1f)
        {
            milkPressureLights[0].GetComponent<MilkPressureLight>().SwitchOn();
            milkPressureLights[1].GetComponent<MilkPressureLight>().SwitchOff();
            milkPressureLights[2].GetComponent<MilkPressureLight>().SwitchOff();
            milkPressureLights[3].GetComponent<MilkPressureLight>().SwitchOff();
            milkPressureLights[4].GetComponent<MilkPressureLight>().SwitchOff();
            milkPressureLights[5].GetComponent<MilkPressureLight>().SwitchOff();
            milkPressureLights[6].GetComponent<MilkPressureLight>().SwitchOff();
        }
        else if (milkPressure > 1f && milkPressure <= 2f)
        {
            milkPressureLights[0].GetComponent<MilkPressureLight>().SwitchOn();
            milkPressureLights[1].GetComponent<MilkPressureLight>().SwitchOn();
            milkPressureLights[2].GetComponent<MilkPressureLight>().SwitchOff();
            milkPressureLights[3].GetComponent<MilkPressureLight>().SwitchOff();
            milkPressureLights[4].GetComponent<MilkPressureLight>().SwitchOff();
            milkPressureLights[5].GetComponent<MilkPressureLight>().SwitchOff();
            milkPressureLights[6].GetComponent<MilkPressureLight>().SwitchOff();
        }
        else if (milkPressure > 2f && milkPressure <= 3f)
        {
            milkPressureLights[0].GetComponent<MilkPressureLight>().SwitchOn();
            milkPressureLights[1].GetComponent<MilkPressureLight>().SwitchOn();
            milkPressureLights[2].GetComponent<MilkPressureLight>().SwitchOn();
            milkPressureLights[3].GetComponent<MilkPressureLight>().SwitchOff();
            milkPressureLights[4].GetComponent<MilkPressureLight>().SwitchOff();
            milkPressureLights[5].GetComponent<MilkPressureLight>().SwitchOff();
            milkPressureLights[6].GetComponent<MilkPressureLight>().SwitchOff();
        }
        else if (milkPressure > 3f && milkPressure <= 4f)
        {
            milkPressureLights[0].GetComponent<MilkPressureLight>().SwitchOn();
            milkPressureLights[1].GetComponent<MilkPressureLight>().SwitchOn();
            milkPressureLights[2].GetComponent<MilkPressureLight>().SwitchOn();
            milkPressureLights[3].GetComponent<MilkPressureLight>().SwitchOn();
            milkPressureLights[4].GetComponent<MilkPressureLight>().SwitchOff();
            milkPressureLights[5].GetComponent<MilkPressureLight>().SwitchOff();
            milkPressureLights[6].GetComponent<MilkPressureLight>().SwitchOff();
        }
        else if (milkPressure > 4f && milkPressure <= 6f)
        {
            milkPressureLights[0].GetComponent<MilkPressureLight>().SwitchOn();
            milkPressureLights[1].GetComponent<MilkPressureLight>().SwitchOn();
            milkPressureLights[2].GetComponent<MilkPressureLight>().SwitchOn();
            milkPressureLights[3].GetComponent<MilkPressureLight>().SwitchOn();
            milkPressureLights[4].GetComponent<MilkPressureLight>().SwitchOn();
            milkPressureLights[5].GetComponent<MilkPressureLight>().SwitchOff();
            milkPressureLights[6].GetComponent<MilkPressureLight>().SwitchOff();
        }
        else if (milkPressure > 6f && milkPressure <= 8f)
        {
            milkPressureLights[0].GetComponent<MilkPressureLight>().SwitchOn();
            milkPressureLights[1].GetComponent<MilkPressureLight>().SwitchOn();
            milkPressureLights[2].GetComponent<MilkPressureLight>().SwitchOn();
            milkPressureLights[3].GetComponent<MilkPressureLight>().SwitchOn();
            milkPressureLights[4].GetComponent<MilkPressureLight>().SwitchOn();
            milkPressureLights[5].GetComponent<MilkPressureLight>().SwitchOn();
            milkPressureLights[6].GetComponent<MilkPressureLight>().SwitchOff();
        }
        else if (milkPressure > 8f && milkPressure <= 10f)
        {
            milkPressureLights[0].GetComponent<MilkPressureLight>().SwitchOn();
            milkPressureLights[1].GetComponent<MilkPressureLight>().SwitchOn();
            milkPressureLights[2].GetComponent<MilkPressureLight>().SwitchOn();
            milkPressureLights[3].GetComponent<MilkPressureLight>().SwitchOn();
            milkPressureLights[4].GetComponent<MilkPressureLight>().SwitchOn();
            milkPressureLights[5].GetComponent<MilkPressureLight>().SwitchOn();
            milkPressureLights[6].GetComponent<MilkPressureLight>().SwitchOn();
        }
    }
    //Milk
    //Syrup
    public void SetSyrupSelection()
    {
        int rnd = Random.Range(0, syrupTypes.Length);
        targetSyrupType = syrupTypes[rnd];
        syrup.text = syrup.text + "\n" + syrupTypes[rnd];
    }
    public void CheckSyrupInput(string syrup)
    {
        if (targetSyrupType==syrup)
        {
            Cup.instance.canReleaseCup = true;
            UpdateScore(5);
            ThumbsUp();
            NextStep();
        }
        else if (canCheckWrongSyrup)
        {
            ThumbsDown();
            UpdateScore(-5);
        }
    }



        //Syrup
        //Shot
        public void SetShotSelection()
        {
            int rnd = Random.Range(1, touchButtons.Length);
            targetShot = rnd;
            shots.text = shots.text + "\n" + rnd;
            foreach (GameObject coffeeFlow in coffeeFlows)
            {
                coffeeFlow.SetActive(false);
            }
        }


        public void SetEspressoMachineTouchButtonsEnabled(int buttonQuantity = 0, string button1Text = "N/A", string button2Text = "N/A", string button3Text = "N/A")
        {
            string[] buttonTexts = { button1Text, button2Text, button3Text };
            for (int i = 0; i < buttonQuantity; i++)
            {
                touchButtons[i].SetActive(true);
                touchButtons[i].transform.GetChild(0).GetComponent<Text>().text = buttonTexts[i];
            }
        }
        public void SetEspressoMachineTouchButtonsDisabled()
        {
            foreach (GameObject touchButton in touchButtons)
            {
                touchButton.SetActive(false);
            }
        }
        public void CheckShotInput(int shotInput)
        {
            if (shotInput == targetShot)
            {
                SecondStepAction();
                UpdateScore(5);
                ThumbsUp();
            }
            else
            {
                UpdateScore(-5);
                ThumbsDown();
            }
        }
        //Shot

        //TouchButtons

        public void ResetTouchButtons()
        {
            foreach (var touchButton in touchButtons)
            {
                DisableTouchButtons();
                touchButton.GetComponent<Image>().color = Color.white;
            }
        }
        public void EnableTouchButtons()
        {
            foreach (var touchButton in touchButtons)
            {
                touchButton.GetComponent<Button>().interactable = true;
            }
        }
        public void DisableTouchButtons()
        {
            foreach (var touchButton in touchButtons)
            {
                touchButton.GetComponent<Button>().interactable = false;
            }
        }
        //TouchButtons


        //Screen

        public void UpdateLCDScreen(string text, int fontSize)
        {
            screenText.text = text;
            screenText.fontSize = fontSize;
        }


        //Screen
    }
