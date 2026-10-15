using UnityEngine;

[CreateAssetMenu(fileName = "Question", menuName = "Data")]

public class MiniGameData : ScriptableObject
{
    public string question;
    public string key;
    public Enums.MiniGameType type;
    public Enums.MiniGamePlace place;
    public Enums.MiniGameGenre genre;
}