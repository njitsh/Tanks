using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;
using System.Text;
using System.IO;



public class OptionsController : MonoBehaviour
{
    string fileName = "users.json";
    string path;

    public TMP_InputField input;
    public TMP_Dropdown dropDown;

    List<string> userList;
    int i = 0;
    User[] user = new User[10];

    public User[] User { get => User1; set => User1 = value; }
    public User[] User1 { get => user; set => user = value; }

    // Start is called before the first frame update
    void Start()
    {

        path = Application.persistentDataPath + "/" + fileName;
        if (!File.Exists(path))
        {
            File.AppendAllText(path, "All users");
        }
        ReadData();
        
        print(path);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SaveData()
    {
        i = 0;
        print(User[i]._name);
        User[i]._name = input.text;
        string contents = JsonUtility.ToJson(User, true);
        File.AppendAllText(path, contents);
        print("saving");
        i++;
    }
    public void ReadData()
    {
        try
        {
            if (File.Exists(path))
            {
                print(path);
                string contents = File.ReadAllText(path);
                print(contents);
                user[i] = JsonUtility.FromJson<User>(contents);
                PopulateList();
            }
            else
            {
                print("no file found");
                User = new User[i];
            }   
        }
        catch (Exception)
        {
            throw;
        }

    }
    public void PopulateList()
    {
        userList = new List<string> { };
        print(User[i]._name);
        userList.Add(User[i]._name);

        dropDown.AddOptions(userList);

    }
}
