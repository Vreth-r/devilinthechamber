using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class Reticle : MonoBehaviour
{
    public float baseGap = 1f;

    private VisualElement root;
    private VisualElement top, bottom, left, right;

    void OnEnable()
    {
        var doc = GetComponent<UIDocument>();
        var r = doc.rootVisualElement;

        root = r.Q<VisualElement>("reticle-root");

        top = root.Q<VisualElement>("pip-top");
        bottom = root.Q<VisualElement>("pip-bottom");
        left = root.Q<VisualElement>("pip-left");
        right = root.Q<VisualElement>("pip-right");

        ForceVisibleFallback(root, top, bottom, left, right);
        UIEvents.UpdateReticleVisibility += SetVisibility;
        // SetSpread(0); // do this later
    }

    void ForceVisibleFallback(VisualElement root, VisualElement top, VisualElement bottom, VisualElement left, VisualElement right)
    {
        root.style.position = Position.Absolute;
        root.style.left = 0; root.style.right = 0; root.style.top = 0; root.style.bottom = 0;
        root.style.justifyContent = Justify.Center;
        root.style.alignItems = Align.Center;
        root.pickingMode = PickingMode.Ignore;

        SetupPip(top, 3, 14);
        SetupPip(bottom, 3, 14);
        SetupPip(left, 14, 3);
        SetupPip(right, 14, 3);
    }

    void SetupPip(VisualElement ve, float w, float h)
    {
        if (ve == null) return;
        ve.style.position = Position.Absolute;
        ve.style.width = w;
        ve.style.height = h;
        ve.style.backgroundColor = Color.white;
    }

    public void SetSpread(float spread)
    {
        float gap = baseGap + spread;

        if (top != null) top.style.bottom = gap;
        if (bottom != null) bottom.style.top = gap;
        if (left != null) left.style.right = gap;
        if (right != null) right.style.left = gap;
    }

    void SetVisibility (bool newVisibility)
    {
        root.visible = newVisibility;
    }
}
