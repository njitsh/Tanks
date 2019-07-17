using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;
using System.Text;
using System.IO;



public class OptionsScript : MonoBehaviour
{

    // UI Components
    public TMP_Dropdown dropDown;
    public TMP_InputField userInput;
    public UserData user;
    // Custom made options
    List<string> userOptions;
    // Data in wich the user will be saved
    string userDataJson;
    // Class to write data to text
    StreamWriter StreamWriter;
    string path;
    // Start is called before the first frame update
    void Start()
    {
       
        // Set path
        path = Application.dataPath +  "/" +  "_user_prefs.json";
        SaveUser();
        FillList();
        print(dropDown);
       
       
    }
    public class UserData
    {
        public string _name;
        public UserData(string name)
        {
            _name = name;
        }
        public string SaveUser()
        {
            return JsonUtility.ToJson(this, false);
        }
    }
    /// <summary>
    /// Save the user to a JSON file in their assets folder
    /// </summary>
    public void SaveUser()
    {
        // Create the file

        if (File.Exists(path))
        {
            print(path);
            // Make temporary user without settings
            print(userInput.text);
            user = new UserData(userInput.text);
            print(user._name);
            // Convert user to JSON
            userDataJson = user.SaveUser();
            //userDataJson = JsonUtility.ToJson(user._name);
            print(userDataJson);
            // Write to JSON text file

            File.AppendAllText(path, userDataJson);
            print(userDataJson);
            print("saving");
        }
        else
        {
            File.WriteAllText(path, "Users");
        }

    }
    /// <summary>
    /// Get data from a json folder
    /// </summary>
    public UserData ReadData()
    {

        string contents = File.ReadAllText(path);
        user = JsonUtility.FromJson<UserData>(contents);
        print("JSON DATA" + user);
        return user;

    }
    /// <summary>
    /// Fill the dropdown list
    /// </summary>
    void FillList()
    {
        // Get all users
        
        // Create new list
        userOptions = new List<string> {};
        //userOptions.Add(ReadData()._name);
       //userOptions.Add(); 
        dropDown.AddOptions(userOptions);
    }
   
    
    // Update is called once per frame
    void Update()
    {

    }
}

