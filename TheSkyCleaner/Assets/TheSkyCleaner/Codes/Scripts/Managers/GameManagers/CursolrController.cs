using UnityEngine;
using UnityEngine.InputSystem;

public class CursolrController : MonoBehaviour
{
    [SerializeField] RectTransform m_cursor;
    [SerializeField] private float m_speed = 10f;
    [SerializeField] private bool m_isMainSceneCheck;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Update()
    {
        m_cursor.position = Mouse.current.position.ReadValue();

        //Vector2 stick = Gamepad.current?.leftStick.ReadValue() ?? Vector2.zero;

        //if (stick != Vector2.zero)
        //{
        //    Vector2 pos = Mouse.current.position.ReadValue();
        //    pos += stick * m_speed * Time.deltaTime;
        //    Mouse.current.WarpCursorPosition(pos);
        //}
    }
}
