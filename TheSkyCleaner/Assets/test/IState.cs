using UnityEngine;
using System.Collections.Generic;
public interface IState2434
{
    void Enter();
    void Update();
    void Exit();
}


public class StateMachine
{
    private IState2434 _current;
    public IState2434 Current => _current; //直接値を変えないように別の変数に格納

    public void ChangeState(IState2434 next)
    {
        if (_current == next) return;
        _current?.Exit();
        _current = next;
        _current?.Enter();
    }

    public void Tick() => _current?.Update();
}


// IdleState.cs

public class IdleState : IState2434
{
    private readonly PlayerController2434 _ctx;

    public IdleState(PlayerController2434 ctx) => _ctx = ctx;

    public void Enter()
    {
        _ctx.SetAnim("Idle", true);
    }

    public void Update()
    {
        // 水平入力が入ったら移動へ
        if (Mathf.Abs(_ctx.InputX) > 0.1f)
        {
            _ctx.SM.ChangeState(_ctx.Move);
            return;
        }

        // 攻撃ボタンで攻撃へ
        if (_ctx.AttackPressed)
        {
            _ctx.SM.ChangeState(_ctx.Attack);
            return;
        }
    }

    public void Exit()
    {
        _ctx.SetAnim("Idle", false);
    }
}


// MoveState.cs

public class MoveState : IState2434
{
    private readonly PlayerController2434 _ctx;

    public MoveState(PlayerController2434 ctx) => _ctx = ctx;

    public void Enter()
    {
        _ctx.SetAnim("Run", true);
    }

    public void Update()
    {
        // 入力から移動
        Vector3 v = new Vector3(_ctx.InputX, 0, _ctx.InputY) * _ctx.moveSpeed * Time.deltaTime;
        _ctx.transform.position += v;

        // 入力が消えたらIdleへ
        if (Mathf.Approximately(_ctx.InputX, 0f) && Mathf.Approximately(_ctx.InputY, 0f))
        {
            _ctx.SM.ChangeState(_ctx.Idle);
            return;
        }

        // 攻撃でAttackへ（移動中でもOK）
        if (_ctx.AttackPressed)
        {
            _ctx.SM.ChangeState(_ctx.Attack);
            return;
        }
    }

    public void Exit()
    {
        _ctx.SetAnim("Run", false);
    }
}


// AttackState.cs

public class AttackState : IState2434
{
    private readonly PlayerController2434 _ctx;
    private float _timer;

    public AttackState(PlayerController2434 ctx) => _ctx = ctx;

    public void Enter()
    {
        _timer = _ctx.attackDuration;
        _ctx.SetAnim("Attack", true);
        // ここで当たり判定の有効化やSE再生など
        _ctx.EnableHitbox(true);
    }

    public void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            // 攻撃が終わったら入力状況で遷移先を決定
            if (Mathf.Abs(_ctx.InputX) > 0.1f || Mathf.Abs(_ctx.InputY) > 0.1f)
                _ctx.SM.ChangeState(_ctx.Move);
            else
                _ctx.SM.ChangeState(_ctx.Idle);
        }
    }

    public void Exit()
    {
        _ctx.SetAnim("Attack", false);
        _ctx.EnableHitbox(false);
    }
}


// PlayerController2434.cs

public class PlayerController2434 : MonoBehaviour
{
    [Header("Params")]
    public float moveSpeed = 5f;
    public float attackDuration = 0.4f;

    [Header("Refs")]
    public Animator animator;
    public GameObject hitbox; // 攻撃判定用（任意）

    // 入力値（ここでは旧Inputを簡易使用。新InputSystemなら適宜置換）
    public float InputX { get; private set; }
    public float InputY { get; private set; }
    public bool AttackPressed { get; private set; }

    // ステートマシン本体と各ステート
    public StateMachine SM { get; private set; }
    public IdleState Idle { get; private set; }
    public MoveState Move { get; private set; }
    public AttackState Attack { get; private set; }

    void Awake()
    {
        SM = new StateMachine();
        Idle = new IdleState(this);
        Move = new MoveState(this);
        Attack = new AttackState(this);
    }

    void Start()
    {
        SM.ChangeState(Idle);
    }

    void Update()
    {
        // 入力取得（※ Input Systemを使うならそちらに差し替え）
        InputX = Input.GetAxisRaw("Horizontal");
        InputY = Input.GetAxisRaw("Vertical");
        AttackPressed = Input.GetKeyDown(KeyCode.J); // 例：Jで攻撃

        SM.Tick();
    }

    // ヘルパー -------------------------------------
    public void SetAnim(string stateName, bool on)
    {
        if (!animator) return;
        animator.SetBool(stateName, on);
    }

    public void EnableHitbox(bool on)
    {
        if (hitbox) hitbox.SetActive(on);
    }
}

