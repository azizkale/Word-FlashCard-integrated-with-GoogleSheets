using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControl : MonoBehaviour
{
    public static Animator scoreAnimotor;
    void Start()
    {
        //scoreAnimotor.SetTrigger("parScore");
    }

    public static void scoreAnimation()
    {
        scoreAnimotor.GetComponent<Animator>().Play("parScore", -1, 0);
    }
}
