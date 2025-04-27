using UnityEngine;

public class HelpOverlay : MonoBehaviour
{
    private bool Enabled = false;
    public void ButtonPress()
    {
        Enabled = true;
        gameObject.SetActive(Enabled); 
    }
    private void Update()
    {
        if (Enabled = true && Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.anyKey)
        {
            Enabled = false;
            gameObject.SetActive(Enabled);
        }
    }
}
