using UnityEngine;

namespace Ikigai.DontTouchTheSpikes
{
    public class SpawnBonus : MonoBehaviour
    {
        [SerializeField]
        private GameObject bonusPrefab;

        private Bounds bounds;

        // Start is called before the first frame update
        void Start()
        {
            GameManager.Instance.HitWall += Spawn;
            bounds = this.GetComponent<SpriteRenderer>().bounds;
        }

        void Spawn(string t)
        {
            if (GameManager.Instance.GetBonusInScene() > 0)
                return;

            var px = Random.Range(bounds.min.x, bounds.max.x);
            var py = Random.Range(bounds.min.y, bounds.max.y);
            Vector2 pos = new Vector3(px, py);
            Instantiate(bonusPrefab, pos, Quaternion.identity);
            GameManager.Instance.SetBonusInScene(1);
        }
    }
}
