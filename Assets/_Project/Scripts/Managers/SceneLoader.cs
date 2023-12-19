using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Image curtain;
    public static SceneLoader Instance { get; private set; }
    private float transitionTime = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
    }
    public void LoadSceneWithTransition(string sceneName,AudioClip transitionStartClip = null, AudioClip transitionEndClip = null)
    {
        if (curtain == null)
        {
            curtain = GetComponentInChildren<Image>();
        }

        StartCoroutine(Transition(sceneName, transitionStartClip,transitionEndClip));
    }
    private IEnumerator Transition(string sceneName, AudioClip transitionStartClip = null, AudioClip transitionEndClip = null)
    {
        // Fade out
        float elapsedTime = 0f;
        Color initialColor = curtain.color;
        Color targetColor = new Color(initialColor.r, initialColor.g, initialColor.b, 1f);
        if(transitionStartClip!=null)
            SoundManager.Instance.PlaySoundEffect(transitionStartClip);
        while (elapsedTime < transitionTime)
        {
            elapsedTime += Time.deltaTime;
            curtain.color = Color.Lerp(initialColor, targetColor, elapsedTime / transitionTime);
            yield return null;
        }

        // Load the scene
        SceneManager.LoadScene(sceneName);

        // Fade in
        elapsedTime = 0f;
        initialColor = targetColor;
        targetColor = new Color(initialColor.r, initialColor.g, initialColor.b, 0f);
        if(transitionEndClip!=null)
            SoundManager.Instance.PlaySoundEffect(transitionEndClip);
        while (elapsedTime < transitionTime)
        {
            elapsedTime += Time.deltaTime;
            curtain.color = Color.Lerp(initialColor, targetColor, elapsedTime / transitionTime);
            yield return null;
        }
    }

}
