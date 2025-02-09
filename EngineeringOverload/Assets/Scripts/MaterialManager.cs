using UnityEngine;
using UnityEngine.UI; // For TextMeshPro
using TMPro;

using System.Collections;
using System.Collections.Generic;

public class MaterialManager : MonoBehaviour
{
    public TextMeshProUGUI mechanicalText;
    public TextMeshProUGUI electricalText;
    public TextMeshProUGUI softwareText;
    public TextMeshProUGUI civilText;
    public TextMeshProUGUI mechatronicsText;
    public TextMeshProUGUI chemicalText;
    public TextMeshProUGUI systemsText;

    private int mechanicalCount = 0;
    private int electricalCount = 0;
    private int softwareCount = 0;
    private int civilCount = 0;
    private int mechatronicsCount = 0;
    private int chemicalCount = 0;
    private int systemsCount = 0;

    void Start()
    {
        // Start Coroutines to increment materials at different intervals
        StartCoroutine(IncrementMaterial(mechanicalText, 1f, () => mechanicalCount++, () => mechanicalCount));
        StartCoroutine(IncrementMaterial(electricalText, 2f, () => electricalCount++, () => electricalCount));
        StartCoroutine(IncrementMaterial(softwareText, 3f, () => softwareCount++, () => softwareCount));
        StartCoroutine(IncrementMaterial(civilText, 4f, () => civilCount++, () => civilCount));
        StartCoroutine(IncrementMaterial(mechatronicsText, 5f, () => mechatronicsCount++, () => mechatronicsCount));
        StartCoroutine(IncrementMaterial(chemicalText, 6f, () => chemicalCount++, () => chemicalCount));
        StartCoroutine(IncrementMaterial(systemsText, 7f, () => systemsCount++, () => systemsCount));
    }

    // Coroutine to increment material at a specific interval
    private IEnumerator IncrementMaterial(TextMeshProUGUI text, float interval, System.Action increment, System.Func<int> getCount)
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            increment(); // Increase the material count
            text.text = getCount().ToString("D4"); // Ensure 4-digit formatting
        }
    }
}

