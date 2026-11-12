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
        accessories,
        none
    }

    public enum MiniGamePlace
    {
        camping,
        park,
        beach,
        tv
    }

    public enum MiniGameGenre
    {
        fantastic,
        romantic,
        horrific,
        historic,
        dramatic,
        none
    }

    public enum GamePhase
    {
        answeringQuestion,
        voting
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
