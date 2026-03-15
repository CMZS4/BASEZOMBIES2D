using UnityEngine;
using TMPro;

public class FragmentManager : MonoBehaviour
{
    public static FragmentManager instance;

    public int fragmentCount = 0;
    public TextMeshProUGUI fragmentText;

    void Awake()
    {
        instance = this;
    }

    public void AddFragment()
    {
        fragmentCount++;
        UpdateUI();
    }

    public void ResetFragments()
    {
        fragmentCount = 0;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (fragmentText != null)
            fragmentText.text = "Fragments: " + fragmentCount;
    }

    public int GetFragments()
    {
        return fragmentCount;
    }
}