using UnityEngine;
using System.Collections.Generic;

public class ArmController : MonoBehaviour
{
    [SerializeField] private ReticleController m_reticleController;

    [SerializeField] private List<Arm> m_arms;
    [SerializeField] private float m_speed;
    [SerializeField] private int m_attack;
    [SerializeField] private int m_maxCount;

    [SerializeField] private Transform m_player;

    private List<bool> m_activeArms = new List<bool>();
    private List<int> m_enemiesId = new List<int>();

    private void Start()
    {
        var m_maxCount = m_reticleController.MaxCount;

        for (int i = 0; i < m_maxCount; i++)
        {
            m_arms[i].gameObject.SetActive(true);
            m_activeArms.Add(true);
        }
    }

    private void Update()
    {
        
    }

    public void ArmShot()
    {
        var m_SaveEnemies = m_reticleController.SaveEnemies;

        foreach (var enemies in m_SaveEnemies)
        {
            int id = enemies.GameObject.GetInstanceID();

            if (m_enemiesId.Contains(id)) continue;

            int index = Getbool();
            if (index == -1) break; //Žg‚¦‚é‚à‚Ì‚ª‚È‚¢

            m_activeArms[index] = false;

            Arm arm = m_arms[index];

            m_enemiesId.Add(id);

            if (enemies is IDamage)
            {
                Debug.Log("Enemy!");
            }
            else
            {
                Debug.Log("Trash!");
            }


            arm.MoveToEnemy(enemies,m_player,
                m_speed,m_attack, id, index);
        }
    }

    private int Getbool()
    {
        for (int i = 0; i < m_activeArms.Count; i++)
        {
            if (m_activeArms[i] == true)
                return i;
        }
        return -1;
    }

    public void Return(int id,int index)
    {
        m_enemiesId.Remove(id);
        m_activeArms[index] = true;
    }
}
