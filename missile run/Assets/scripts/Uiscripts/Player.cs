using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public characterDatabase characterDB;

    public SpriteRenderer artworksprite;

    private int selectedOption = 0;

    
    void Start()
    {
        if (!PlayerPrefs.HasKey("selectedOption"))
        {
            selectedOption = 1;
        }
        else
        {
            Load();
        }

        UpdateCharacter(selectedOption);
    }

    private void UpdateCharacter(int selectedOption)
    {
        Character character = characterDB.GetCharacter(selectedOption);
        artworksprite.sprite = character.characterSprite;

        if(gameObject!= null && gameObject.GetComponent<PolygonCollider2D>() != null)
        {
            gameObject.GetComponent<PolygonCollider2D>().points = character.Player_collider2D.points;
        }
        
    }

    private void Load()
    {
        selectedOption = PlayerPrefs.GetInt("selectedOption");
    }
}
