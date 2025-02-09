using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum State {
        Running,
        Attacking
    }

    State currentState = State.Running;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Animator anim;
    public float moveSpeed = 2f;
    private float timer = 1.5f;
    bool rotating = false;
    bool grabbed = false;
    void Start()
    {
        anim = GetComponent<Animator>();

        if (anim == null)
        {
            Debug.LogError("No Animator component found on Enemy!");
            return;
        }
        
        anim.SetBool("walking", true);
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.Running:
                Run();
                break;
            case State.Attacking:
                Attack();
                break;
        }
    }

    void OnTriggerEnter(Collider building)
    {
        if (building.CompareTag("Attackable")){
            currentState = State.Attacking;
            anim.SetBool("walking", false);
        }
    }

    private void Run()
    {
        anim.speed = moveSpeed;
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
    }

    Quaternion targetRotation;
    private void Attack()
    {
        anim.speed = 1f;

        if (!grabbed) {
            grabbed = true;
            anim.SetTrigger("grab");
        }
        if (timer <= 0f) {
            if (!rotating){
                targetRotation = Quaternion.Euler(0, transform.eulerAngles.y + 180, 0);
                rotating = true;
            }
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
            if (Quaternion.Angle(transform.rotation, targetRotation) < 1f)
            {
                transform.rotation = targetRotation;
                rotating = false;
                anim.SetBool("walking", true);
                currentState = State.Running;
            }
        } else {
            timer -= Time.deltaTime;
        }
    }
}
