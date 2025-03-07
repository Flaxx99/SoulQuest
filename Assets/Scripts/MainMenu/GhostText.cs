using UnityEngine;
using TMPro;

public class GhostlyTextEffect : MonoBehaviour
{
    public TextMeshProUGUI text;
    private float alpha = 1f;
    private bool fadingOut = true;

    void Update()
    {
        alpha += (fadingOut ? -1 : 1) * Time.deltaTime * 0.5f;
        if (alpha <= 0.4f) fadingOut = false;
        if (alpha >= 1f) fadingOut = true;

        text.color = new Color(text.color.r, text.color.g, text.color.b, alpha);
    }
}
