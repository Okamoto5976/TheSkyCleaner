using UnityEngine;

public class Logger : MonoBehaviour
{
    [SerializeField] private bool m_isEnabled = true;
    [SerializeField] private string m_prefix;
    [SerializeField] private Color m_color = Color.white;

    private string m_hexColor;

    private static int m_logCount;

    private void OnValidate()
    {
        m_hexColor = "#"+ColorUtility.ToHtmlStringRGBA(m_color);
    }

    private void LateUpdate()
    {
        m_logCount = 0;
    }

    public void Log(string message, Object context)
    {
        if (m_isEnabled)
        {
            Debug.Log($"[{m_logCount}] <color={m_hexColor}>{m_prefix}</color>: {context.name} -> {message}", context);
            m_logCount++;
        }
    }
}
