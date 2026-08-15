using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class TMPBreathingGlow : MonoBehaviour
{
    [Header("高亮颜色")]
    [SerializeField]
    private Color glowColor = new Color(0.2f, 0.7f, 1f, 1f);

    [Header("呼吸效果")]
    [Tooltip("最低亮度")]
    [Range(0f, 1f)]
    [SerializeField]
    private float minIntensity = 0.15f;

    [Tooltip("最高亮度")]
    [Range(0f, 1f)]
    [SerializeField]
    private float maxIntensity = 0.7f;

    [Tooltip("呼吸速度")]
    [Range(0.1f, 10f)]
    [SerializeField]
    private float breathingSpeed = 2f;

    [Header("Glow 参数")]
    [Range(-1f, 1f)]
    [SerializeField]
    private float glowOffset = 0f;

    [Range(0f, 1f)]
    [SerializeField]
    private float glowInner = 0.05f;

    [Range(0f, 1f)]
    [SerializeField]
    private float glowOuter = 0.25f;

    [Range(0.01f, 1f)]
    [SerializeField]
    private float glowPower = 0.5f;

    private TMP_Text tmpText;
    private Material glowMaterial;

    private static readonly int GlowColorID =
        Shader.PropertyToID("_GlowColor");

    private static readonly int GlowOffsetID =
        Shader.PropertyToID("_GlowOffset");

    private static readonly int GlowInnerID =
        Shader.PropertyToID("_GlowInner");

    private static readonly int GlowOuterID =
        Shader.PropertyToID("_GlowOuter");

    private static readonly int GlowPowerID =
        Shader.PropertyToID("_GlowPower");


    private void Awake()
    {
        InitMaterial();
    }

    private void Start()
    {
        ApplyGlowSettings();
    }

    private void Update()
    {
        UpdateBreathing();
    }


    private void InitMaterial()
    {
        tmpText = GetComponent<TMP_Text>();

        if (tmpText == null || tmpText.fontSharedMaterial == null)
            return;

        // 创建独立材质，避免修改其他使用相同字体的文字
        glowMaterial = new Material(tmpText.fontSharedMaterial);

        glowMaterial.name =
            tmpText.fontSharedMaterial.name + "_BreathingGlow";

        tmpText.fontMaterial = glowMaterial;

        // 开启 TMP Glow
        glowMaterial.EnableKeyword("GLOW_ON");

        tmpText.UpdateMeshPadding();
    }


    private void ApplyGlowSettings()
    {
        if (glowMaterial == null)
            return;

        glowMaterial.SetFloat(
            GlowOffsetID,
            glowOffset
        );

        glowMaterial.SetFloat(
            GlowInnerID,
            glowInner
        );

        glowMaterial.SetFloat(
            GlowOuterID,
            glowOuter
        );

        glowMaterial.SetFloat(
            GlowPowerID,
            glowPower
        );

        tmpText.UpdateMeshPadding();
    }


    private void UpdateBreathing()
    {
        if (glowMaterial == null)
            return;

        // sin 范围：
        // -1 ~ 1

        float sinValue =
            Mathf.Sin(Time.unscaledTime * breathingSpeed);

        // 转换成：
        // 0 ~ 1

        float t =
            sinValue * 0.5f + 0.5f;

        // 计算当前高亮强度
        float intensity =
            Mathf.Lerp(
                minIntensity,
                maxIntensity,
                t
            );

        Color currentColor = glowColor;

        // 用 Alpha 控制 Glow 强弱
        currentColor.a *= intensity;

        glowMaterial.SetColor(
            GlowColorID,
            currentColor
        );
    }


    // =========================
    // 外部调用接口
    // =========================

    /// <summary>
    /// 修改高亮颜色
    /// </summary>
    public void SetGlowColor(Color color)
    {
        glowColor = color;
    }


    /// <summary>
    /// 修改呼吸速度
    /// </summary>
    public void SetBreathingSpeed(float speed)
    {
        breathingSpeed =
            Mathf.Max(0.01f, speed);
    }


    /// <summary>
    /// 修改呼吸亮度范围
    /// </summary>
    public void SetIntensity(
        float min,
        float max)
    {
        minIntensity =
            Mathf.Clamp01(min);

        maxIntensity =
            Mathf.Clamp01(max);
    }


    /// <summary>
    /// 修改 Glow 范围
    /// </summary>
    public void SetGlowSize(float size)
    {
        glowOuter =
            Mathf.Clamp01(size);

        if (glowMaterial != null)
        {
            glowMaterial.SetFloat(
                GlowOuterID,
                glowOuter
            );

            tmpText.UpdateMeshPadding();
        }
    }


    private void OnDestroy()
    {
        if (glowMaterial == null)
            return;

#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            DestroyImmediate(glowMaterial);
        }
        else
#endif
        {
            Destroy(glowMaterial);
        }
    }
}