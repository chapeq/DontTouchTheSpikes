using UnityEngine;

namespace Ikigai.DontTouchTheSpikes
{
    public class BonusBehaviour : MonoBehaviour
    {
        [SerializeField]
        private AudioClip bonusSound;
        [SerializeField]
        private GameObject PlusPrefab;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Canvas canvas = FindObjectOfType<Canvas>();
                GameObject obj = Instantiate(PlusPrefab); // TODO : Object Pooling
                obj.transform.SetParent(canvas.transform, false);
                var anim = obj.GetComponent<Ikigai.DontTouchTheSpikes.Animations.PlusOneAnimation>();
                anim.PlaceInWorld(gameObject.transform.position);

                GameManager.Instance.audioSource.PlayOneShot(bonusSound);
                GameManager.Instance.IncrementScore();
                GameManager.Instance.SetBonusInScene(0);

                Destroy(gameObject);
            }
        }
    }
}