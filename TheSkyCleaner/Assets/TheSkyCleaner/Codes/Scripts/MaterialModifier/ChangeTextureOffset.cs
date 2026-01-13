using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class ChangeTextureOffset : MonoBehaviour
{
    [SerializeField] private Renderer m_renderer;

    private MaterialPropertyBlock m_mpb;
    private void Awake()
    {
        m_mpb = new();
        m_renderer = GetComponent<Renderer>();
    }

    public void SetXOffset(float val)
    {
        Vector4 textureOffset = m_renderer.material.GetVector("_MainTex_ST");
        textureOffset.z = val;
        m_mpb.SetVector("_MainTex_ST", textureOffset);
        m_renderer.SetPropertyBlock(m_mpb);
    }

    public void SetYOffset(float val)
    {
        Vector4 textureOffset = m_renderer.material.GetVector("_MainTex_ST");
        textureOffset.w = val;
        m_mpb.SetVector("_MainTex_ST", textureOffset);
        m_renderer.SetPropertyBlock(m_mpb);
    }

}
