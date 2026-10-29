using UnityEngine;

public class CharacterBehaviour : MonoBehaviour
{
    //Contains Data for characters, other scripts have to override it for each character
    public string characterName;
    public Sprite characterImage;

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
