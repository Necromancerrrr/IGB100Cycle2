using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class Cursor : MonoBehaviour
{
    public Camera mainCam;
    public ParticleSystem CursorParticles;
    public bool Click = false;
    public float Colour = 1;
    void Update()
    {
        TrackMouse();
        DetectClick();
        UpdateColour();
    }
    void TrackMouse()
    {
        Vector2 mousePos = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        transform.position = mousePos;
    }
    void DetectClick()
    {
        if (Input.GetMouseButton(0))
        {
            Click = true;
        }
        else
        {
            Click = false;
        }
    }
    void UpdateColour()
    {
        if (Click)
        {
            Colour -= Time.deltaTime;
        }
        else
        {
            Colour += Time.deltaTime;
        }
        Colour = Mathf.Clamp(Colour, 0, 1);
        CursorParticles.startColor = new Color(Colour, Colour, Colour);
    }
}
