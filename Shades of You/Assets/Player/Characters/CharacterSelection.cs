using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectCharacter(CharacterData characterData)
    {
        Player.Instance.SetActiveCharacter(characterData);
        // Load the level
    }
}
