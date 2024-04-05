using System.Collections;
using System.Collections.Generic;
using TabTabs.NamChanwoo;
using UnityEngine;
using UnityEngine.Animations;
using Spine;
using Spine.Unity;
using UnityEngine.SceneManagement;

public class PlayerBase : MonoBehaviour
{
    [SerializeField]
    public static Animator PlayerAnim;
    [SerializeField]
    public static Transform PlayerTransform;

    void Start()
    {
        if (PlayerAnim == null)
        {
            PlayerAnim = GetComponent<Animator>();
        }

        PlayerTransform = GetComponent<Transform>();

        Debug.Log("playerAnim: " + PlayerAnim);
        Debug.Log("playerTrans:" + PlayerTransform);
    }

}
