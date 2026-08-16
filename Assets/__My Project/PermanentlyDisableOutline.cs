using UnityEngine;

public class PermanentlyDisableOutline : MonoBehaviour
{
    [Header("需要永久关闭的 Outline 组件")]
    public Behaviour outlineComponent;

    private bool hasBeenDisabled = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasBeenDisabled)
            return;

        if (other.CompareTag("Player") && outlineComponent != null)
        {
            outlineComponent.enabled = false;
            hasBeenDisabled = true;
        }
    }
}