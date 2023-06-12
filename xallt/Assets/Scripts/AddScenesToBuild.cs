using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class AddScenesToBuild : EditorWindow
{
    [MenuItem("Tools/AddScenesToBuild")]

    static void Init()
    {
        List<EditorBuildSettingsScene> editorBuildSettingsScenes = new List<EditorBuildSettingsScene>();
        List<string> SceneList = new List<string>();
        string MainFolder = "Assets/Scenes/AllScenesAdditively";


        DirectoryInfo d = new DirectoryInfo(@MainFolder);
        FileInfo[] Files = d.GetFiles("*.unity"); //Getting unity files

        foreach (FileInfo file in Files)
        {
            Debug.Log("file name:" + file.Name);
            SceneList.Add(file.Name);
        }


        for (int i = 0; i < SceneList.Count; i++)
        {
            string scenePath = MainFolder + "/" + SceneList[i];
            Debug.Log("i = " + i);
            Debug.Log("scene path:" + scenePath);
            editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(scenePath, true));
        }

        EditorBuildSettings.scenes = editorBuildSettingsScenes.ToArray();
    }

}