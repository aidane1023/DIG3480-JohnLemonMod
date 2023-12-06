using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float turnSpeed = 20f;
    public AudioSource walkingAudio;
    public AudioSource gaspAudio;
    public GameObject notification;

    Animator m_Animator;
    Rigidbody m_Rigidbody;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;
    bool terrified = false;

    void Start ()
    {
        m_Animator = GetComponent<Animator> ();
        m_Rigidbody = GetComponent<Rigidbody> ();

        notification.SetActive(false);
        InvokeRepeating("Freeze", Random.Range(10.0f, 16.0f), Random.Range(10.0f, 16.0f));
    }

    void FixedUpdate ()
    {
        float horizontal = Input.GetAxis ("Horizontal");
        float vertical = Input.GetAxis ("Vertical");
        
        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize ();

        bool hasHorizontalInput = !Mathf.Approximately (horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately (vertical, 0f);
        bool isWalking = hasHorizontalInput || hasVerticalInput;
        m_Animator.SetBool ("IsWalking", isWalking);
        
        if (isWalking && !terrified)
        {
            if (!walkingAudio.isPlaying)
            {
                walkingAudio.Play();
            }
        }
        else
        {
            walkingAudio.Stop ();
        }

        Vector3 desiredForward = Vector3.RotateTowards (transform.forward, m_Movement, turnSpeed * Time.deltaTime, 0f);
        m_Rotation = Quaternion.LookRotation (desiredForward);
    }

    void OnAnimatorMove ()
    {
        m_Rigidbody.MovePosition (m_Rigidbody.position + m_Movement * m_Animator.deltaPosition.magnitude);
        m_Rigidbody.MoveRotation (m_Rotation);
    }

    public void Stop ()
    {
        CancelInvoke();
        walkingAudio.Stop ();
        gaspAudio.Stop();
        notification.SetActive(false);
    }

    void Freeze ()
    {
        terrified = true;
        gaspAudio.Play();
        notification.SetActive(true);
        m_Animator.enabled = false;
        StartCoroutine("UnFreeze");
    }

    IEnumerator UnFreeze ()
    {
        yield return new WaitForSeconds(3f);
        gaspAudio.Stop();
        notification.SetActive(false);
        m_Animator.enabled = true;
        terrified = false;
    }
}
