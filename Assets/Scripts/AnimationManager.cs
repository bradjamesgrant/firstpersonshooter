using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public static AnimationManager instance = null;
    public Animator anim;
    int run;

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

    private void Start()
    {
        run = Animator.StringToHash("run");
        InputManager.instance.Movement += SetRunAnim;
    }

    void SetRunAnim(Vector2 input, Vector2 inputRaw)
    {
            
    }
}
