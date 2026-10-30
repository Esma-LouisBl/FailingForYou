using UnityEngine;

public class Enums : MonoBehaviour
{
    public enum MiniGameType
    {
        textual,
        photographic,
        drawing,
        sound,
        clothing,
        accessories
    }

    public enum MiniGamePlace
    {
        camping,
        park,
        beach
    }

    public enum MiniGameGenre
    {
        fantastic,
        romantic,
        horrific,
        historic,
        dramatic
    }
    
    public enum SelectedUI
    {
        connectionUIWelcome,
        connectionUIMenu,
        connectionUIOptions,
        connectionUICredits,
        connectionUIConnecting
    }
}
