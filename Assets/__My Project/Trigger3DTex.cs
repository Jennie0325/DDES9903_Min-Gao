using System.Collections;
using UnityEngine;
using TMPro;

public class Trigger3DText : MonoBehaviour
{
    [Header("3D Text")]
    [SerializeField] private TMP_Text text3D;

    [TextArea(2, 5)]
    [SerializeField] private string textContent = "Hello World!";

    [Header("Delay Settings")]
    [SerializeField] private float delayTime = 3f;

    [Header("Trigger Settings")]
    [SerializeField] private string playerTag = "Player";

    [SerializeField] private bool triggerOnlyOnce = true;

    private bool hasTriggered = false;

    private void Start()
    {
        // 游戏开始时隐藏文字
        if (text3D != null)
        {
            text3D.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag))
            return;

        if (triggerOnlyOnce && hasTriggered)
            return;

        hasTriggered = true;

        StartCoroutine(ShowTextAfterDelay());
    }

    private IEnumerator ShowTextAfterDelay()
    {
        // 等待指定秒数
        yield return new WaitForSeconds(delayTime);

        if (text3D != null)
        {
            // 修改文字
            text3D.text = textContent;

            // 显示3D文字
            text3D.gameObject.SetActive(true);
        }
    }
}