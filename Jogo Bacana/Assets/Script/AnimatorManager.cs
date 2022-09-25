using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    public Animator animator;

    public void SetFalse(){
        animator.SetBool("isMudarHumor", false);
    }
}
