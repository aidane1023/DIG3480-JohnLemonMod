using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameEnding : MonoBehaviour
{
    public float fadeDuration = 1f;
    public float displayImageDuration = 1f;
    public float timeLimit = 120f;
    public GameObject player;
    public CanvasGroup exitBackgroundImageCanvasGroup;
    public AudioSource exitAudio;
    public CanvasGroup caughtBackgroundImageCanvasGroup;
    public AudioSource caughtAudio;

    [SerializeField] TextMeshProUGUI countdownText;

    bool m_IsPlayerAtExit;
    bool m_IsPlayerCaught;
    float m_Timer;
    float timeRemaining;
    bool m_HasAudioPlayed;
    PlayerMovement playerScript;
    
    void Start ()
    {
        timeRemaining = timeLimit;
        playerScript = player.GetComponent<PlayerMovement>();
    }

    void OnTriggerEnter (Collider other)
    {
        if (other.gameObject == player)
        {
            m_IsPlayerAtExit = true;
        }
    }

    public void CaughtPlayer ()
    {
        m_IsPlayerCaught = true;
    }

    void Update ()
    {
        timeRemaining -= 1 * Time.deltaTime;
        countdownText.text = timeRemaining.ToString("0"); 

        if (timeRemaining <= 0)
        {
            timeRemaining = 0;
            countdownText.gameObject.SetActive(false);
            playerScript.Stop();
            EndLevel(caughtBackgroundImageCanvasGroup, true, caughtAudio);
        }

        if (m_IsPlayerAtExit)
        {
            countdownText.gameObject.SetActive(false);
            playerScript.Stop();
            EndLevel (exitBackgroundImageCanvasGroup, false, exitAudio);
        }
        else if (m_IsPlayerCaught)
        {
            countdownText.gameObject.SetActive(false);
            playerScript.Stop();
            EndLevel (caughtBackgroundImageCanvasGroup, true, caughtAudio);
        }
    }

    void EndLevel (CanvasGroup imageCanvasGroup, bool doRestart, AudioSource audioSource)
    {
        if (!m_HasAudioPlayed)
        {
            audioSource.Play();
            m_HasAudioPlayed = true;
        }
            
        m_Timer += Time.deltaTime;
        imageCanvasGroup.alpha = m_Timer / fadeDuration;

        if (m_Timer > fadeDuration + displayImageDuration)
        {
            if (doRestart)
            {
                SceneManager.LoadScene (0);
            }
            else
            {
                Application.Quit ();
            }
        }
    }
}
