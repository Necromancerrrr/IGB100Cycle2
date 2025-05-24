using UnityEngine;

public class HelpOverlay : MonoBehaviour
{
    private int State = 0;

    [SerializeField] GameObject helpScreen1;
    [SerializeField] GameObject helpScreen2;
    [SerializeField] GameObject helpScreen3;
    [SerializeField] GameObject helpScreen4; // New screen added

    public void ButtonPress()
    {
        SetScreen1();
    }
    public void MovingForward()
    {
        switch (State)
        {
            case 1:
                SetScreen2();
                break;
            case 2:
                SetScreen3();
                break;
            case 3:
                SetScreen4();
                break;
            case 4:
                SetNoScreens();
                break;
            default:
                SetNoScreens();
                break;
        }
    }
    public void MovingBack()
    {
        switch (State)
        {
            case 1:
                SetNoScreens();
                break;
            case 2:
                SetScreen1();
                break;
            case 3:
                SetScreen2();
                break;
            case 4:
                SetScreen3();
                break;
            default:
                SetNoScreens();
                break;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.S))
        {
            MovingForward();
        }
        else if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.W))
        {
            MovingBack();
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
        helpScreen3.SetActive(false);
        helpScreen4.SetActive(false);
    }

    void SetScreen2()
    {
        State = 2;
        helpScreen1.SetActive(false);
        helpScreen2.SetActive(true);
        helpScreen3.SetActive(false);
        helpScreen4.SetActive(false);
    }

    void SetScreen3()
    {
        State = 3;
        helpScreen1.SetActive(false);
        helpScreen2.SetActive(false);
        helpScreen3.SetActive(true);
        helpScreen4.SetActive(false);
    }

    void SetScreen4()
    {
        State = 4;
        helpScreen1.SetActive(false);
        helpScreen2.SetActive(false);
        helpScreen3.SetActive(false);
        helpScreen4.SetActive(true);
    }

    void SetNoScreens()
    {
        State = 0;
        helpScreen1.SetActive(false);
        helpScreen2.SetActive(false);
        helpScreen3.SetActive(false);
        helpScreen4.SetActive(false);
    }
}