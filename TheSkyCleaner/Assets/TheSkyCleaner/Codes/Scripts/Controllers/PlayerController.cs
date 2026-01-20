using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private UnityEvent<float> m_onMoveHorizontal;  //平行移動 float変換
    [SerializeField] private UnityEvent<float> m_onMoveVertical;    //垂直移動 float変換
    [SerializeField] private UnityEvent m_onMainActionTap;          //マウスhidari クリック入力
    [SerializeField] private UnityEvent m_onMainActionHoldStarted;  //マウス右ホールド
    [SerializeField] private UnityEvent m_onMainActionHoldCancelled;//マウス右ホールドキャンセル
    [SerializeField] private UnityEvent<float> m_onChangeSpeed;     //プレイヤー速度変化

    public void MoveHorizontal(float dir)
    {
        m_onMoveHorizontal.Invoke(dir);
    }

    public void MoveVertical(float dir)
    {
        m_onMoveVertical.Invoke(dir);
    }

    public void ChangeSpeed(float dir)
    {
        m_onChangeSpeed.Invoke(dir);
    }

    /// <summary>
    /// 左クリックした時にインスペクターでいじった関数呼び出し
    /// </summary>
    public void MainActionTap()
    {
        m_onMainActionTap.Invoke();
    }

    public void MainActionHoldSetState(bool state) //メイン制御?
    {
        if (state)
        {
            Debug.Log("Hold Started");
            m_onMainActionHoldStarted.Invoke();
        }
        else
        {
            Debug.Log("Hold Cancelled");
            m_onMainActionHoldCancelled.Invoke();
        }
    }
}
