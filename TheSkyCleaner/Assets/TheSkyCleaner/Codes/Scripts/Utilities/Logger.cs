using UnityEngine;

public class Logger : MonoBehaviour
{
    [SerializeField] private bool m_isEnabled = true;
    [SerializeField] private string m_prefix;
    [SerializeField] private Color m_color = Color.white;

    private string m_hexColor;

    private void OnValidate()
    {
        m_hexColor = "#"+ColorUtility.ToHtmlStringRGBA(m_color);
    }

    public void Log(string message, Object context)
    {
        if (m_isEnabled)
        {
            Debug.Log($"<color~{m_hexColor}>{m_prefix}: {message}", context);
        }
    }
}
