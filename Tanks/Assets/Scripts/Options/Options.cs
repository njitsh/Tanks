using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    string fileName = "users.json";
    string path;

    public TMP_InputField input;
    public TMP_Dropdown dropDown;
    public TMP_Text sensitivityLabel;
    public Slider slider;
    User[] users;
    List<string> userList;
    
    int i = 1;
    // Start is called before the first frame update
    void Start()
    {
        // Creating user installation
        path = Application.persistentDataPath + "/" + fileName;
        LoadFile();
        User[] users = new User[i];
    }



    #region UserCreation
    // Update is called once per frame
    void Update()
    {
        
    }
    public void SaveData()
    {
        User[] users = new User[i +1];
        users[i] = new User();
        users[i]._name = input.text;
        string contents = JsonHelper.ToJson(users,true);
        File.WriteAllText(path, contents);
      //  dropDown.ClearOptions();
        userList.Clear();
        dropDown.RefreshShownValue();
    }
    public void ReadData()
    {
        if (File.Exists(path))
        {
            string contents = File.ReadAllText(path);
            users = JsonHelper.FromJson<User>(contents);
            PopulateList();
        }
    }
    public void PopulateList()
    {
        userList.Add(users[i]._name);
        dropDown.AddOptions(userList);
        i++;
    }
    /// <summary>
    /// On launch, load all values previously saved in the users file
    /// </summary>
    public void LoadFile()
    {
        if (File.Exists(path))
        {
            userList = new List<string> { };
            string contents = File.ReadAllText(path);

            users = JsonHelper.FromJson<User>(contents);
         
            foreach (User user in users)
            {
                userList.Add(user._name);
            }
            dropDown.AddOptions(userList);
        }
    }   
    public void SaveDropdown()
    {
        int amountOfOptions = 0;
        List<TMP_Dropdown.OptionData> optionDataList = new List<TMP_Dropdown.OptionData>();
        // Count the amount of users currently in the dropdown
        foreach (TMP_Dropdown.OptionData option in dropDown.options)
        {
            amountOfOptions++;
            print(amountOfOptions);
            print(option.text);
        }

        User[] allUsers = new User[amountOfOptions];

        
        for (int i = 0; i < amountOfOptions; i++)
        {
            print(i);
            allUsers[i] = new User();
            allUsers[i]._name = dropDown.options[i].text;
        }
        string contents = JsonHelper.ToJson(allUsers, true);
        File.WriteAllText(path, contents);
    }
    #region Options
    public void SensitivityChange()
    {
        sensitivityLabel.text = "Sensitivity: " + slider.value.ToString();
        
    }
    public void SaveOptionChanges()
    {
        print(userList[dropDown.value]);
        string currentUser = userList[dropDown.value];
        // hier gebruik ik dus een custom constructor (user)
       // User user = new User();
        //user.sensitivity = 
        // TODO now enter all the options that belong to the user   
    }
    #endregion
}

#endregion

/// <summary>
/// An overwrite class for the JsonUtility. This class is made for processing arrays to json.
/// </summary>
public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}


