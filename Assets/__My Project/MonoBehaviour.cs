using System.Collections;
using UnityEngine;

public class DelayedObjectAppear : MonoBehaviour
{
    [Header("需要延迟出现的物体")]
    public GameObject targetObject;

    [Header("触发后等待多少秒")]
    public float delayTime = 10f;

    [Header("是否只能触发一次")]
    public bool triggerOnlyOnce = true;

    private bool hasTriggered = false;

    private void Start()
    {
        // 游戏开始时隐藏目标物体
        if (targetObject != null)
        {
            targetObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (triggerOnlyOnce && hasTriggered)
            return;

        hasTriggered = true;
        StartCoroutine(ShowObjectAfterDelay());
    }

    private IEnumerator ShowObjectAfterDelay()
    {
        yield return new WaitForSeconds(delayTime);

        if (targetObject != null)
        {
            targetObject.SetActive(true);
        }
    }
}