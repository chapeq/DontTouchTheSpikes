using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;

namespace Ikigai.DontTouchTheSpikes
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI TextScore;
        [SerializeField]
        private TextMeshProUGUI TextHighScore;
        [SerializeField]
        private TextMeshProUGUI TextEndScore;
        [SerializeField]
        private TextMeshProUGUI TextEndHighScore;

        private int score = 0;
        private string saveFile;
        private FileStream dataStream;
        private BinaryFormatter converter = new BinaryFormatter();
        GameData data;

        // Start is called before the first frame update
        void Start()
        {
            GameManager.Instance.scorePoints += AddPoints;
            GameManager.Instance.GameEnd += SaveScore;
            saveFile = Application.persistentDataPath + "/gamedata.data";
            data = new GameData();
            readFile();
            TextHighScore.text = "Best Score : " + string.Format("{0:00}", data.highScore);
        }


        private void AddPoints()
        {
            score++;
            TextScore.text = string.Format("{0:00}", score);
        }

        private void SaveScore()
        {
            if (data.highScore < score)
            {
                data.highScore = score;
                writeFile();
            }
            TextEndHighScore.text = "Best Score : " + string.Format("{0:00}", data.highScore);
            TextEndScore.text = " Score : " + string.Format("{0:00}", score);
        }

        public void readFile()
        {
            if (File.Exists(saveFile))
            {
                dataStream = new FileStream(saveFile, FileMode.Open);
                data = converter.Deserialize(dataStream) as GameData;
                dataStream.Close();
            }
        }

        public void writeFile()
        {
            dataStream = new FileStream(saveFile, FileMode.Create);
            converter.Serialize(dataStream, data);
            dataStream.Close();
        }
    }

    [System.Serializable]
    public class GameData
    {
        public int highScore = 0;
    }
}
