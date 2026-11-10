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

    public enum GamePhase
    {
        answeringQuestion,
        voting,
        tie
    }
    
    public enum SelectedUI
    {
        clientConnectionUIWelcome,
        clientConnectionUICode,
        hostConnectionUIWelcome,
        hostConnectionUIMenu,
        hostConnectionUIOptions,
        hostConnectionUICredits,
        hostConnectionUIConnecting
    }

    public enum CharacterName
    {
        Boubou,
        Bog,
        Kouc,
        Pacc,
        Pito,
        Kame,
        None
    }
}
