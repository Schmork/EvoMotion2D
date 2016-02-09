using UnityEngine;
using System.Collections;

public class ToolTipHandler : MonoBehaviour {

    public EvoMotion2D.Cell.CellHandler cellHandler;
    public float AlphaChange;
    float backgroundScaleFactor = 19f;

    public static float TextToBackgroundAlphaRatio = 0.3f;
    	
	// Update is called once per frame
	void Update ()
    {
        var textMesh = GetComponentInChildren<TextMesh>();
        var spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (textMesh.color.a < 0.1f)    // tooltip has faded out; no further processing
        {
            textMesh.color = changeAlpha(textMesh.color, -textMesh.color.a);                    // set alpha to 0
            spriteRenderer.color = changeAlpha(spriteRenderer.color, -spriteRenderer.color.a);  // set alpha to 0
            return;
        }

        textMesh.color = changeAlpha(textMesh.color, AlphaChange);
        spriteRenderer.color = changeAlpha(spriteRenderer.color, AlphaChange * TextToBackgroundAlphaRatio);

        var format = "{0:0.0}";

        var text = cellHandler.name + "\n";
        text += "age: " + (int)cellHandler.Age + "\n";
        text += "gen: " + cellHandler.Generation + "\n";
        text += "kids: " + cellHandler.Children;
        textMesh.text = text;
                
        var bounds = textMesh.GetComponent<Renderer>().bounds;
        GetComponentInChildren<SpriteRenderer>().transform.localScale = new Vector3(bounds.extents.x * backgroundScaleFactor, bounds.extents.y * backgroundScaleFactor, 1);
    }

    Color changeAlpha(Color color, float change)
    {
        return new Color(color.r,
                         color.g,
                         color.b,
                         color.a + change);
    }
}
