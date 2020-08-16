using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SaveData
{
    public int[] password;
}

public class SavingAndLoading : MonoBehaviour
{
    public Text[] passwordNumbers;
    public string fileName = "SaveData.txt";

    private string path;
    private BinaryFormatter formatter = new BinaryFormatter();

    void Start()
    {
        path = Application.persistentDataPath + fileName;
    }

    public void IncreaseNumber (int number)
    {
        int newNum = int.Parse(passwordNumbers[number].text) + 1;
        if (newNum == 10)
        {
            newNum = 0;
        }
        passwordNumbers[number].text = newNum.ToString();
    }

    public void DecreaseNumber (int number)
    {
        int newNum = int.Parse(passwordNumbers[number].text) - 1;
        if (newNum == -1)
        {
            newNum = 9;
        }
        passwordNumbers[number].text = newNum.ToString();
    }

    public void ResetPassword ()
    {
        for (int i = 0; i < passwordNumbers.Length; i++)
        {
            passwordNumbers[i].text = "0";
        }
        print("Reset!");
    }

    public void SavePassword ()
    {
        FileStream file = new FileStream(path, FileMode.Create);
        SaveData data = new SaveData();
        data.password = new int[passwordNumbers.Length];
        for (int i = 0; i < passwordNumbers.Length; i++)
        {
            data.password[i] = int.Parse(passwordNumbers[i].text);
        }
        formatter.Serialize(file, data);
        file.Close();
        print("Save!");
    }

    public void LoadPassword ()
    {
        if (File.Exists(path))
        {
            FileStream file = new FileStream(path, FileMode.Open);
            SaveData data = (SaveData)formatter.Deserialize(file);
            for (int i = 0; i < passwordNumbers.Length; i++)
            {
                passwordNumbers[i].text = data.password[i].ToString();
            }
            file.Close();
        }
        else
        {
            for (int i = 0; i < passwordNumbers.Length; i++)
            {
                passwordNumbers[i].text = "0";
            }
            SavePassword();
        }
        print("Load!");
    }

    public void DeleteData()
    {
        File.Delete(path);
        ResetPassword();
        SavePassword();
        print("Deleted!");
    }
}
