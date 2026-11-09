using UnityEngine;

public class CharacterBehaviour : MonoBehaviour
{
    //Contains Data for characters, other scripts have to override it for each character
    public Enums.CharacterName characterName = Enums.CharacterName.None;
    public Sprite characterSprite;

    public virtual void PlayIdle()
    {
        
    }
    
    public virtual void PlaySurprised()
    {
        
    }
    
    public virtual void PlayHappy()
    {
        
    }
}
