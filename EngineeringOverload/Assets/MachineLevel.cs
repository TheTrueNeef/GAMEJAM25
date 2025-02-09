using NUnit.Framework.Constraints;
using System.Diagnostics;
using UnityEngine;

public class MachineLevel : MonoBehaviour
{
    public int machineLevel = 1;
    public GameObject[] machines = new GameObject[3];
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        machines[0].SetActive(true);
        machines[1].SetActive(false);
        machines[2].SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        switch (machineLevel)
        {
            case 1:
                machines[0].SetActive(true);
                machines[1].SetActive(false);
                machines[2].SetActive(false);
                break;
                    case 2:
                machines[0].SetActive(false);
                machines[1].SetActive(true);
                machines[2].SetActive(false);
                break;
            case 3:
                machines[0].SetActive(false);
                machines[1].SetActive(false);
                machines[2].SetActive(true);
                break;
        }
    }
    
}
