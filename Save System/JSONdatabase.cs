using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class JSONdatabase : MonoBehaviour
{
    public static JSONdatabase instance;
    public List<PlayerProps> scoreList;
    private bool isBetter;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            return;
        }
    }
    private void Start()
    {
        scoreList = new List<PlayerProps>();
        for (int i = 0; i < 5; i++)
        {
            if (File.Exists(Application.dataPath + "/databaseFile" + i + ".json"))
            {
                string json_read = File.ReadAllText(Application.dataPath + "/databaseFile" + i + ".json");
                PlayerProps data = JsonUtility.FromJson<PlayerProps>(json_read);

                scoreList.Add(data);
            }
        }
        print(scoreList.Count);
        scoreList = scoreList.OrderBy(x => x.score).ToList();
        print(scoreList.Count);
        DontDestroyOnLoad(gameObject);
        DisplayScoreTable(scoreList);
    }
    public void Save(PlayerProps data)
    {
        print(data.score);
        for (int i = 0; i < 5; i++)
        {
            if(!File.Exists(Application.dataPath + "/databaseFile" + i + ".json"))
            {
                data.JsonId = i;
                string json_write = JsonUtility.ToJson(data, true);
                File.WriteAllText(Application.dataPath + "/databaseFile" + i + ".json", json_write);
                return;
            }
        }
        for (int i = 0; i < 5; i++)
        {
            print(scoreList[i].score);
            print(i + " " + scoreList[i].JsonId + " " + scoreList[i].name);
            string json_read = File.ReadAllText(Application.dataPath + "/databaseFile" + scoreList[i].JsonId + ".json"); // starts from the lowest
            PlayerProps _data = JsonUtility.FromJson<PlayerProps>(json_read);
            print(_data.name +" "+_data.score);
           if (data.score > _data.score)
           {
                string json_write = JsonUtility.ToJson(data, true);
                File.WriteAllText(Application.dataPath + "/databaseFile" + scoreList[i].JsonId + ".json", json_write);
                break;
           }
            
        }
    }
    public void Load()
    {
        for (int i = 0; i < 5; i++)
        {
            if (!File.Exists(Application.dataPath + "/databaseFile" + i + ".json"))
            {
                return;
            }
            else
            {
                string json_read = File.ReadAllText(Application.dataPath + "/databaseFile" + i + ".json");
                PlayerProps data = JsonUtility.FromJson<PlayerProps>(json_read);
                scoreList.Add(data);
            }

        }
        DisplayScoreTable(scoreList);

    }
    public void DisplayScoreTable(List<PlayerProps> array)
    {
        MenuManager.Instance.LoadDatas();   
    }
}
