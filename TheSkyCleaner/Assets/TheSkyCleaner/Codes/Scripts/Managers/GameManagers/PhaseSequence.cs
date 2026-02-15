using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(menuName = "Scriptable Objects/Phase/PhaseSequence")]
public class PhaseSequence : ScriptableObject
{

    public List<GamePhase> m_phase;

}
