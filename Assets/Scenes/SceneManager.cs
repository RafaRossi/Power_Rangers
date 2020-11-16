using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : Manager<SceneManager>
{
    private string sceneName;

    private Animator _animator;
    private Animator Animator
    {
        get
        {
            if(!_animator)
            {
                _animator = GetComponent<Animator>();
            }
            return _animator;
        }
    }

    [SerializeField] private AnimationClip fade_in = null;
    [SerializeField] private AnimationClip fade_out = null;


    public void ChangeScene(string sceneName)
    {
        this.sceneName = sceneName;
        StartCoroutine(ChangeScene());
    }

    private IEnumerator ChangeScene()
    {
        Animator.Play(fade_out.name);

        yield return new WaitForSeconds(fade_out.length);
        AsyncOperation operation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);

        while(!operation.isDone)
        {
            yield return null;
        }


        Animator.Play(fade_in.name);
        yield return new WaitForSeconds(fade_in.length);
    }
}
