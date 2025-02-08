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
    float baseY;

    public float moveSpeed = 2f;
    public float bobSpeed = 7f;
    public float bobAmplitude = 0.5f;
    void Start()
    {
        baseY = transform.position.y;
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
        }
    }

    private void Run()
    {
        float newY = baseY + Mathf.Sin(Time.time * bobSpeed) * bobAmplitude;

        // Move forward continuously
        transform.position += transform.forward * moveSpeed * Time.deltaTime;
        
        // Apply new Y position
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    private void Attack()
    {
        transform.eulerAngles += new Vector3(0,2,0);
    }
}
