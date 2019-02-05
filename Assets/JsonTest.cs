using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Json;
using System.IO;
using System;
using Newtonsoft.Json;

public class JsonTest : MonoBehaviour
{
    // ボタンオブジェクト
    public Button save_btn;
    public Button load_btn;

    void Start()
    {
        // ボタンオブジェクトがクリックされた際のイベントリスナ
        save_btn.onClick.AddListener(() => SaveJSON());
        load_btn.onClick.AddListener(() => LoadJSON());
    }

    // JSONデータの保存
    private void SaveJSON()
    {
        // データ定義
        PersonList person_list = new PersonList();
        person_list.ListName = "金剛型";
        person_list.Persons = new List<Person>();

        // データ作成
        List<string> name_list = new List<string>() {"金剛", "比叡", "榛名" ,"霧島" };
        int age = 24;
        foreach (string name in name_list)
        {
            person_list.Persons.Add(new Person() {
                Name = name,
                Age = age,
                Sex = "W"
            });
            age -= 1;
        }

        // JSONデータに成形
        using (var ms = new MemoryStream())
        using (var sr = new StreamReader(ms))
        {
            // JSON形式にシリアライズ
            var serializer = new DataContractJsonSerializer(typeof(PersonList));
            serializer.WriteObject(ms, person_list);
            ms.Position = 0;

            var json = sr.ReadToEnd();

            // JSONファイルに書き込み
            // Application.dataPathはプロジェクトのAssetsディレクトリを指す
            File.WriteAllText(Application.dataPath + "/sample.json", json);

        }
        Debug.Log("JSONファイルを保存しました");
    }

    // JSONファイルの読み込みとデバッグ表示
    private void LoadJSON()
    {
        // ファイルを読み込む
        string[] allText = File.ReadAllLines(Application.dataPath + "/sample.json");
        Debug.Log("JSONファイルを読み込みました");

        // JSONデシリアライズ
        var person = JsonConvert.DeserializeObject<PersonList>(allText[0]);

        // デバッグ表示
        Debug.Log(person.ListName);
        foreach (Person p in person.Persons)
        {
            Debug.Log(p.Name + "-" + p.Age + "-" + p.Sex);
        }
    }
}

// データの定義
public class PersonList
{
    public string ListName { get; set; }
    public List<Person> Persons { get; set; }
}

public class Person
{
    public string Name { get; set; }
    public string Sex { get; set; }
    public int Age { get; set; }
}
