using UnityEngine;
using TMPro;

public class WobblyText : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TMP_Text textComponent;
    public float x_scale = 0.01f;
    public float sin_peak = 2f;
    public float speed = 5f;

    void Update()
    {
        textComponent.ForceMeshUpdate();
        var textInfo = textComponent.textInfo;

        for(int i = 0; i < textInfo.characterCount; ++i)
        {
            var charInfo = textInfo.characterInfo[i];

            if (!charInfo.isVisible)
            {
                continue;
            }

            var verts = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;

            for(int j = 0; j < 4; ++j)
            {
                var orig = verts[charInfo.vertexIndex + j];
                verts[charInfo.vertexIndex + j] = orig + new Vector3(0, Mathf.Sin(Time.time * speed + orig.x* x_scale) * sin_peak, 0);
            }

        }

        for (int i = 0; i < textInfo.meshInfo.Length; ++i) 
        { 
            var meshInfo = textInfo.meshInfo[i];

            meshInfo.mesh.vertices = meshInfo.vertices;
            textComponent.UpdateGeometry(meshInfo.mesh, i);
        }
    }
}
