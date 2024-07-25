using System.Collections;
using UnityEngine;


namespace Ikigai.DontTouchTheSpikes
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField]
        private float speed;
        [SerializeField]
        private float jumpForce;
        [SerializeField]
        private AudioClip JumpSound;
        [SerializeField]
        private PhysicsMaterial2D bouncyMat;

        private Rigidbody2D rb;
        private bool canMove;
        private bool alreadyCollide;
        private int cptHit = 0;
        private float maxSpeed;
        private ParticleSystem trail;

        // Start is called before the first frame update
        void Start()
        {
            GameManager.Instance.GameStart += CanMove;
            rb = GetComponent<Rigidbody2D>();
            trail = this.GetComponentInChildren<ParticleSystem>();
            canMove = false;
            alreadyCollide = false;
            cptHit = 0;
            maxSpeed = speed * 2.0f;
        }

        private void Update()
        {
            if (!canMove)
                return;

            if (Input.GetMouseButtonDown(0))
            {
                rb.velocity = new Vector2(0, jumpForce);
                GameManager.Instance.audioSource.PlayOneShot(JumpSound);
            }
            transform.Translate(Vector2.right * (speed * Time.deltaTime));
        }

        private void CanMove()
        {
            canMove = true;
            trail.Play();
        }

        private void IncreaseSpeed()
        {
            if (speed >= maxSpeed)
                return;

            cptHit++;

            if (cptHit >= 5)
            {
                speed = speed + (speed * 0.2f);
                cptHit = 0;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("RightWall") || collision.gameObject.CompareTag("LeftWall"))
            {
                transform.Rotate(0, 180, 0);
                GameManager.Instance.WallHit(collision.gameObject.tag);
                GameManager.Instance.IncrementScore();
                IncreaseSpeed();
            }
            else if (collision.gameObject.CompareTag("Spikes"))
            {
                if (alreadyCollide)
                    return;

                canMove = false;
                StartCoroutine(DeathAnim());
                GameManager.Instance.EndGame();
                alreadyCollide = true;
            }
        }

        IEnumerator DeathAnim()
        {
            trail.Stop();
            this.GetComponent<Animator>().SetBool("IsDead", true);
            this.GetComponent<Collider2D>().sharedMaterial = bouncyMat;
            yield return new WaitForSeconds(3.0f);
            this.GetComponent<Collider2D>().enabled = false;
            yield return new WaitForSeconds(8.0f);
            Destroy(rb);
        }
    }
}
