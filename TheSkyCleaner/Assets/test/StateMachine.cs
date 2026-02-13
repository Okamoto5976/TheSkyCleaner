
//using UnityEngine;
//using UnityEngine.InputSystem; //Input.System
//using System.Collections;      //コルーチン

//public class SimpleStateMachine_InputSystem : MonoBehaviour
//{
//    private enum State
//    {
//        Idle,
//        Move,
//        Attack,
//        Jump
//    }

//    private State current = State.Idle;


//    void FixedUpdate()
//    {
//        // デバイスが無い環境（プレイ直後など）でのNRE回避
//        var keyboard = Keyboard.current;
//        var mouse = Mouse.current;
//        if (keyboard == null || mouse == null) return;

//        switch (current)
//        {
//            case State.Idle:
//                IdleUpdate(keyboard, mouse);
//                break;

//            case State.Move:
//                MoveUpdate(keyboard, mouse);
//                break;

//            case State.Attack:
//                AttackUpdate();
//                break;

//            case State.Jump:
//                JumpUpdate();
//                break;
//        }
//    }

//    void IdleUpdate(Keyboard kb, Mouse ms)
//    {
//        Debug.Log("状態：Idle");

//        // 移動入力（WASD いずれかが押されている）
//        if (IsMovePressed(kb))
//        {
//            current = State.Move;
//            return;
//        }

//        // 左クリックで攻撃（1フレーム押下）
//        if (ms.leftButton.wasPressedThisFrame)
//        {
//            current = State.Attack;
//            return;
//        }

//        // スペースでジャンプ（1フレーム押下 or 押下中いずれか好みで）
//        if (kb.spaceKey.wasPressedThisFrame)
//        {
//            current = State.Jump;
//            return;
//        }
//    }

//    void MoveUpdate(Keyboard kb, Mouse ms)
//    {
//        Debug.Log("状態：Move");

//        if (!IsMovePressed(kb))
//        {
//            current = State.Idle;
//            return;
//        }

//        if (ms.leftButton.wasPressedThisFrame)
//        {
//            current = State.Attack;
//            return;
//        }

//        if (kb.spaceKey.wasPressedThisFrame)
//        {
//            current = State.Jump;
//            return;
//        }
//    }

//    void AttackUpdate()
//    {
//        Debug.Log("状態：Attack");
//        StartCoroutine(ReturnToIdle());
//    }

//    void JumpUpdate()
//    {
//        Debug.Log("状態：Jump"); StartCoroutine(ReturnToIdle());
//    }

//    IEnumerator ReturnToIdle()
//    {
//        current = State.Idle;
//        yield return new WaitForSeconds(0.3f);

//    }

//    bool IsMovePressed(Keyboard kb)
//    {
//        return kb.wKey.isPressed || kb.sKey.isPressed || kb.aKey.isPressed || kb.dKey.isPressed;
//    }
//}


// IState.cs

// StateMachine.cs