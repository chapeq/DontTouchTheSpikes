using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ikigai.DontTouchTheSpikes
{
    public class SpikesManager : MonoBehaviour
    {
        [SerializeField]
        private Transform[] rightSpikes;
        [SerializeField]
        private Transform[] leftSpikes;

        [SerializeField]
        private int nbSpikeToSpawn = 2;
        [SerializeField]
        private int nbSpikeToSpawnMax = 7;

        private int cptHit = 0;

        // Start is called before the first frame update
        private void Start()
        {
            GameManager.Instance.GameStart += HideSpikes;
            GameManager.Instance.HitWall += SpawnSpikes;

            UnityEngine.Random.InitState((int)DateTime.Now.Ticks);
            cptHit = 0;
        }

        private void HideSpikes()
        {
            foreach (Transform t in rightSpikes)
            {
                StartCoroutine(MoveLeft(t.gameObject, true));
            }
            foreach (Transform t in leftSpikes)
            {
                StartCoroutine(MoveRight(t.gameObject, true));
            }
        }


        private void SpawnSpikes(string wall)
        {
            cptHit++;
            if (cptHit >= 5 && nbSpikeToSpawn <= nbSpikeToSpawnMax)
            {
                nbSpikeToSpawn++;
                cptHit = 0;
            }

            if (wall == "RightWall")
            {
                List<int> rValues = new List<int>();
                for (int i = 0; i < nbSpikeToSpawn; i++)
                {
                    rValues.Add(UniqueRandomInt(rValues, 0, leftSpikes.Length));
                }
                for (int i = 0; i < leftSpikes.Length; i++)
                {
                    if (!rValues.Contains(i))
                    {
                        if (leftSpikes[i].gameObject.activeSelf)
                        {
                            StartCoroutine(MoveRight(leftSpikes[i].gameObject, true));
                        }
                    }
                }
                foreach (int val in rValues)
                {
                    if (!leftSpikes[val].gameObject.activeSelf)
                        StartCoroutine(MoveLeft(leftSpikes[val].gameObject, false));
                }
            }
            else if (wall == "LeftWall")
            {
                List<int> lValues = new List<int>();
                for (int i = 0; i < nbSpikeToSpawn; i++)
                {
                    lValues.Add(UniqueRandomInt(lValues, 0, rightSpikes.Length));
                }
                for (int i = 0; i < rightSpikes.Length; i++)
                {
                    if (!lValues.Contains(i))
                    {
                        if (rightSpikes[i].gameObject.activeSelf)
                        {
                            StartCoroutine(MoveLeft(rightSpikes[i].gameObject, true));
                        }
                    }
                }
                foreach (int val in lValues)
                {
                    if (!rightSpikes[val].gameObject.activeSelf)
                        StartCoroutine(MoveRight(rightSpikes[val].gameObject, false));
                }
            }
        }


        IEnumerator MoveRight(GameObject spike, bool hide)
        {
            if (!hide)
            {
                spike.SetActive(true);
            }
            float elapsedTime = 0;
            Vector3 startingPos = spike.transform.position;
            Vector3 endPose = (spike.transform.position + new Vector3(-0.5f, 0, 0));
            while (elapsedTime < 0.5f)
            {
                spike.transform.position = Vector3.Lerp(startingPos, endPose, (elapsedTime / 0.5f));
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            spike.transform.position = endPose;
            if (hide)
            {
                spike.SetActive(false);
            }

        }

        IEnumerator MoveLeft(GameObject spike, bool hide)
        {
            if (!hide)
            {
                spike.SetActive(true);
            }
            float elapsedTime = 0;
            Vector3 startingPos = spike.transform.position;
            Vector3 endPose = (spike.transform.position + new Vector3(0.5f, 0, 0));
            while (elapsedTime < 0.5f)
            {
                spike.transform.position = Vector3.Lerp(startingPos, endPose, (elapsedTime / 0.5f));
                elapsedTime += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            spike.transform.position = endPose;
            if (hide)
            {
                spike.SetActive(false);
            }

        }


        private int UniqueRandomInt(List<int> usedValues, int min, int max)
        {
            int val = UnityEngine.Random.Range(min, max);
            while (usedValues.Contains(val))
            {
                val = UnityEngine.Random.Range(min, max);
            }
            return val;
        }
    }
}
