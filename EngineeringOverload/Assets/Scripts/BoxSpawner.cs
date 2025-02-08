using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject box;
    public float speed = 5f;
    void Start()
    {
        GameObject boxClone = Instantiate(box, transform.position, Quaternion.identity);
        Rigidbody boxRB = boxClone.GetComponent<Rigidbody>();
        if (boxRB != null) {
            boxRB.linearVelocity = -transform.right * speed;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
