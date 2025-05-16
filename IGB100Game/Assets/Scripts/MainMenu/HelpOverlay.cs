using UnityEngine;

public class HelpOverlay : MonoBehaviour
{
    private int State = 0;
    [SerializeField] GameObject helpScreen1;
    [SerializeField] GameObject helpScreen2;
    public void ButtonPress()
    {
        SetScreen1();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            switch(State)
            {
                case 1:
                    SetScreen2();
                    break;
                case 2:
                    SetNoScreens();
                    break;
                default:
                    SetNoScreens();
                    break;
            }
        }
        else if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            switch (State)
            {
                case 1:
                    SetNoScreens();
                    break;
                case 2:
                    SetScreen1();
                    break;
                default:
                    SetNoScreens();
                    break;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            SetNoScreens();
        }
    }
    void SetScreen1()
    {
        State = 1;
        helpScreen1.SetActive(true);
        helpScreen2.SetActive(false);
    }
    void SetScreen2()
    {
        State = 2;
        helpScreen1.SetActive(false);
        helpScreen2.SetActive(true);
    }
    void SetNoScreens()
    {
        State = 0;
        helpScreen1.SetActive(false);
        helpScreen2.SetActive(false);
    }
}
