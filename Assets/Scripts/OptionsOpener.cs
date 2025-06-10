using UnityEngine;

public class OptionsOpener : MonoBehaviour
{
    public GameObject optionsPanel;

    public void OpenOptionsPanel()
    {
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(true);
        }
    }
}
