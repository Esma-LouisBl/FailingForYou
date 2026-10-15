using System.IO;
using UnityEngine;
using UnityEditor;

public class QuestionsImporter
{
    //Editor Menu
    private static string _csvPath = "/Scripts/EditorUtilities/QuestionsCSV.csv"; //Location of the CSV file
    [MenuItem("Questions/Import Questions")]

    
    //Function
    public static void ImportQuestions()
    {
        string[] allLines = File.ReadAllLines(Application.dataPath + _csvPath);

        foreach (string line in allLines)
        {
            string[] fields = line.Split(';');
            
            MiniGameData miniGameData = ScriptableObject.CreateInstance<MiniGameData>();
            miniGameData.question = fields[0];
            miniGameData.key = fields[4];
            switch (fields[1])
            {
                case("dramatic"):
                    miniGameData.genre = Enums.MiniGameGenre.dramatic;
                    break;
                case("historic"):
                    miniGameData.genre = Enums.MiniGameGenre.historic;
                    break;
                case("romantic"):
                    miniGameData.genre = Enums.MiniGameGenre.romantic;
                    break;
                case("fantastic"):
                    miniGameData.genre = Enums.MiniGameGenre.fantastic;
                    break;
                case("horrific"):
                    miniGameData.genre = Enums.MiniGameGenre.horrific;
                    break;
            }

            switch (fields[2])
            {
                case("beach"):
                    miniGameData.place = Enums.MiniGamePlace.beach;
                    break;
                case("park"):
                    miniGameData.place = Enums.MiniGamePlace.park;
                    break;
                case("camping"):
                    miniGameData.place = Enums.MiniGamePlace.camping;
                    break;
            }

            switch (fields[3])
            {
                case("textual"):
                    miniGameData.type = Enums.MiniGameType.textual;
                    break;
                case("drawing"):
                    miniGameData.type = Enums.MiniGameType.drawing;
                    break;
                case("sound"):
                    miniGameData.type = Enums.MiniGameType.sound;
                    break;
                case("photographic"):
                    miniGameData.type = Enums.MiniGameType.photographic;
                    break;
            }
            
            AssetDatabase.CreateAsset(miniGameData, $"Assets/Data/Questions/{miniGameData.type}/{miniGameData.place}/{miniGameData.genre}/{miniGameData.key}.asset");
        }
        
        AssetDatabase.SaveAssets();
    }
}