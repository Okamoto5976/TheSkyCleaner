using System.Collections;
using System.Linq;
using UnityEngine;

/// <summary>
/// Animator の bool パラメータ（待機/攻撃1/攻撃2/攻撃3）をランダムに切り替え、
/// 指定条件を満たすまで繰り返すローパー。
/// 使い方：Animator に bool パラメータを4つ用意し、配列にその名前を入れるだけ。
/// </summary>
public class BoolAnimation : MonoBehaviour
{
    [Header("制御対象")]
    [SerializeField] private Animator animator;

    [Tooltip("操作する Animator の bool パラメータ名（例：Idle, Attack1, Attack2, Attack3）。配列の要素はいずれも一意。")]
    [SerializeField] private string[] boolParameterNames = { "Idle2", "Attack", "Attack1", "Attack2" };

    [Header("ランダム切替設定")]
    [Tooltip("同じアニメを連続で選ばないようにする")]
    [SerializeField] private bool avoidRepeat = true;

    [Tooltip("Idle の出現比率（0で同率、数値を大きくするほど Idle が出やすくなる）")]
    [Range(0f, 5f)][SerializeField] private float idleWeight = 0f;

    [Header("待機方法の選択")]
    [Tooltip("true: 現在のステートが終わるまで待つ（非ループ前提） / false: 固定秒")]
    [SerializeField] private bool isWaitByClipEnd = true;

    [Tooltip("固定秒で回す場合の各アニメ再生時間（秒）。配列長は boolParameterNames と一致させると個別設定可。1要素なら全てに適用。")]
    [SerializeField] private float[] fixedDurations = { 0.8f };

    [Header("開始時の動作")]
    [Tooltip("Start() で自動開始する")]
    [SerializeField] private bool playOnStart = true;

    private int lastIndex = -1;
    private Coroutine loopRoutine;
    private bool stopRequested = false;

    private void Reset()
    {
        if (animator == null) animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (playOnStart) StartLoop();
    }

    /// <summary>ループ開始</summary>
    public void StartLoop()
    {
        stopRequested = false;
        if (loopRoutine != null) StopCoroutine(loopRoutine);
        loopRoutine = StartCoroutine(LoopCoroutine());
    }

    /// <summary>ループ停止要求（即時に止まるのではなく、切り替えポイントで停止）</summary>
    public void RequestStop()
    {
        stopRequested = true;
    }

    /// <summary>
    /// 外部条件で停止したいときはこの関数を継承クラスでオーバーライドするか、
    /// コンポーネント参照から RequestStop() を呼ぶ。
    /// 例：HP<=0 / ターゲット発見 / 時間経過 など。
    /// </summary>
    protected virtual bool ShouldStop()
    {
        return stopRequested;
    }

    private IEnumerator LoopCoroutine()
    {
        if (animator == null || boolParameterNames == null || boolParameterNames.Length == 0)
            yield break;

        // 初期化：全てOFF → IdleだけON（最初の要素を待機とみなす）
        SetExclusiveBool(0);

        while (!ShouldStop())
        {
            // 次に再生するインデックスを選ぶ
            int next = ChooseNextIndex();
            SetExclusiveBool(next);

            // 再生が終わるまで待つ
            if (isWaitByClipEnd)
            {
                yield return WaitForCurrentStateToFinish(next);
            }
            else
            {
                yield return new WaitForSeconds(GetFixedDuration(next));
            }

            lastIndex = next;

            // 停止条件チェック（ループ末尾で評価）
            if (ShouldStop()) break;
        }

        // 止めるときは Idle に戻す（配列の0番を待機と想定）
        if (boolParameterNames.Length > 0)
        {
            SetExclusiveBool(0);
        }
        loopRoutine = null;
    }

    /// <summary>一つだけ true にして他を false にする</summary>
    private void SetExclusiveBool(int trueIndex)
    {
        for (int i = 0; i < boolParameterNames.Length; i++)
        {
            if (string.IsNullOrEmpty(boolParameterNames[i])) continue;
            animator.SetBool(boolParameterNames[i], i == trueIndex);
        }
    }

    /// <summary>次に再生するインデックスをランダム選択（Idle の重みと連続回避に対応）</summary>
    private int ChooseNextIndex()
    {
        int count = boolParameterNames.Length;
        if (count == 1) return 0;

        // 重みテーブル作成（0番=Idle に重みを加える）
        float[] weights = Enumerable.Repeat(1f, count).ToArray();
        if (idleWeight > 0f && count >= 1) weights[0] += idleWeight;

        // 連続回避：直前の要素の重みを0に
        if (avoidRepeat && lastIndex >= 0 && lastIndex < count) weights[lastIndex] = 0f;

        // 重み合計
        float total = weights.Sum();
        if (total <= 0f)
        {
            // すべて0になってしまった場合は素直にランダム（重みなし）
            int idx;
            do { idx = Random.Range(0, count); }
            while (avoidRepeat && count > 1 && idx == lastIndex);
            return idx;
        }

        // ルーレット選択
        float r = Random.Range(0f, total);
        float acc = 0f;
        for (int i = 0; i < count; i++)
        {
            acc += weights[i];
            if (r <= acc) return i;
        }
        return count - 1;
    }

    /// <summary>
    /// 現在のステートが1ループ完了するまで待つ。
    /// ※ 非ループ前提。ループ設定のクリップの場合は Exit Time 付き遷移を使うか fixedDurations を利用してください。
    /// </summary>
    private IEnumerator WaitForCurrentStateToFinish(int index, int layer = 0)
    {
        // 1フレーム待ってステート反映
        yield return null;

        AnimatorStateInfo st = animator.GetCurrentAnimatorStateInfo(layer);

        // 現在のステートが切り替わるまで待ち（保険）
        float safety = 0f;
        while (safety < 0.5f) // 0.5秒以内に切り替わる想定
        {
            var now = animator.GetCurrentAnimatorStateInfo(layer);
            if (now.fullPathHash != st.fullPathHash) { st = now; break; }
            safety += Time.deltaTime;
            yield return null;
        }

        // 非ループ前提で normalizedTime < 1 を待つ
        while (!st.loop && st.normalizedTime < 1f)
        {
            yield return null;
            st = animator.GetCurrentAnimatorStateInfo(layer);
        }
    }

    /// <summary>固定秒方式の再生時間を取得（配列が1要素なら全アニメ共通）</summary>
    private float GetFixedDuration(int index)
    {
        if (fixedDurations == null || fixedDurations.Length == 0) return 0.8f;
        if (fixedDurations.Length == 1) return Mathf.Max(0f, fixedDurations[0]);
        if (index < 0 || index >= fixedDurations.Length) return Mathf.Max(0f, fixedDurations[fixedDurations.Length - 1]);
        return Mathf.Max(0f, fixedDurations[index]);
    }
}
