using UnityEngine;

public class Inventory : MonoBehaviour
{
    private int m_material1 = 0;
    private int m_material2 = 0;
    private int m_material3 = 0;



    public int Material1 { get => m_material1; set { m_material1 = value; } }
    public int Material2 { get => m_material2; set { m_material2 = value; } }
    public int Material3 { get => m_material3; set { m_material3 = value; } }


}
