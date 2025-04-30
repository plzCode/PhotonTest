using UnityEngine;
using UnityEngine.UI;
public class LayoutMatch : MonoBehaviour
{
    public LayoutElement layoutElement;
    public RectTransform Target;

    public bool MatchHeight = true;
    public bool MatchWidth = false;

    void Update()
    {
        if (MatchHeight)
        {
            if (layoutElement.minHeight != Target.sizeDelta.y)
            {
                layoutElement.minHeight = Target.sizeDelta.y;
            }
        }

        if (MatchWidth)
        {
            if (layoutElement.minWidth != Target.sizeDelta.x)
            {
                layoutElement.minWidth = Target.sizeDelta.x;
            }
        }
    }
}