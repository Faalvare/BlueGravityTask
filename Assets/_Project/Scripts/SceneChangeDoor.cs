using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneChangeDoor : MonoBehaviour
{
    [SerializeField] private string SceneName;
    [SerializeField] private AudioClip doorOpenAudio;
    [SerializeField] private AudioClip doorCloseAudio;
    [SerializeField] private UnityEvent OnEnter;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SceneLoader.Instance.LoadSceneWithTransition(SceneName, doorOpenAudio, doorCloseAudio);
            OnEnter?.Invoke();
        }
    }
}
