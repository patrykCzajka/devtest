using UnityEngine;

public class GameOverView : MonoBehaviour
{
    [SerializeField] GameObject thisPanelGO;

    public void SetPanelActive() {
        thisPanelGO.SetActive(true);
    }
    public void HidePanel() {
        thisPanelGO.SetActive(false);
    }
}
