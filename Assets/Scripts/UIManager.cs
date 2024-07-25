using UnityEngine;

namespace Ikigai.DontTouchTheSpikes
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject StartUI;
        [SerializeField]
        private GameObject EndUI;

        // Start is called before the first frame update
        void Start()
        {
            GameManager.Instance.GameStart += HideStartUI;
            GameManager.Instance.GameEnd += ShowEndUI;
            ShowStartUI();
        }

        void ShowStartUI()
        {
            StartUI.SetActive(true);
        }

        void HideStartUI()
        {
            StartUI.SetActive(false);
        }

        void ShowEndUI()
        {

            EndUI.SetActive(true);
        }
    }
}
