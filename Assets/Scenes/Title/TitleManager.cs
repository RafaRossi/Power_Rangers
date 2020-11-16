using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms;

public class TitleManager : Manager<TitleManager>
{
    public List<Animator> rangersAnimators = new List<Animator>();
    [SerializeField] private string nextScene = "Menu";

    private void Start()
    {
        foreach(Animator animator in rangersAnimators)
        {
            animator.SetFloat("Speed", Random.Range(0.3f, 0.5f));
        }
    }

    private void Update()
    {
        if(Input.anyKeyDown)
        {
            SceneManager.Instance.ChangeScene(nextScene);
        }
    }
}
