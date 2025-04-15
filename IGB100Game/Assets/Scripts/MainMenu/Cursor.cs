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
    }
    // Locates the mouse and teleports object to its location.
    void TrackMouse()
    {
        Vector2 mousePos = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        transform.position = mousePos;
    }
    // Alters the colour between white and black based on mouse click.
    void DetectClick()
    {
        if (Input.GetMouseButton(0))
        {
            Colour -= Time.deltaTime;
        }
        else
        {
            Colour += Time.deltaTime;
        }
        Colour = Mathf.Clamp(Colour, 0, 1);
        var main = CursorParticles.main;
        main.startColor = new Color(Colour, Colour, Colour);
    }
}
