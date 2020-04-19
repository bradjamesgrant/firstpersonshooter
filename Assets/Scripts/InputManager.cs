using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public delegate void MovementDelegate(Vector2 v1, Vector2 v2);
    public delegate void InputDelegate();

    public event MovementDelegate Movement;
    public event InputDelegate Space;
    public event InputDelegate StoppedMoving;
    public event InputDelegate Shoot;
    public event InputDelegate Reload;

    public static InputManager instance = null;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {

        
        
        if (Input.GetMouseButtonDown(0))
        {
            Shoot?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload?.Invoke();
        }
    }

    private void FixedUpdate()
    {
        Vector2 inputVec = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Vector2 inputRaw = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (inputRaw != Vector2.zero)
        {
            Movement?.Invoke(inputVec, inputRaw);
        }
        else
        {
            StoppedMoving?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Space?.Invoke();
        }
    }
}
