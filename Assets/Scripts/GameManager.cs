using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ikigai.DontTouchTheSpikes
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        public event Action GameStart, GameEnd, scorePoints;
        public event Action<string> HitWall;

        public AudioSource audioSource;

        [SerializeField]
        private AudioClip StartSound;
        [SerializeField]
        private AudioClip EndSound;
        [SerializeField]
        private AudioClip WallSound;

        private bool isReady = true;
        private bool isPlaying = false;
        private int bonusInScene = 0;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
        }

        private void Update()
        {
            if (isReady)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    StartGame();
                }
            }
        }

        public void StartGame()
        {
            isReady = false;
            isPlaying = true;
            GameStart?.Invoke();
            audioSource.PlayOneShot(StartSound);
        }

        public void EndGame()
        {
            GameEnd?.Invoke();
            audioSource.PlayOneShot(EndSound);
            isPlaying = false;
        }

        public void WallHit(string wall)
        {
            if (!isPlaying)
                return;
            HitWall?.Invoke(wall);
            audioSource.PlayOneShot(WallSound);
        }

        public void IncrementScore()
        {
            if (!isPlaying)
                return;
            scorePoints?.Invoke();
        }

        public void Replay()
        {
            isReady = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public int GetBonusInScene()
        {
            return bonusInScene;
        }

        public void SetBonusInScene(int value)
        {
            bonusInScene = value;
        }

    }

}


