using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionButton : Button
{
    private Hero hero;

    public void Initialize(Hero hero)
    {
        this.hero = hero;
        GetComponent<Image>().color = hero.ranger.rangerColor;
    }
    
    public void ChooseCharacter()
    {
        Menu.Instance.SelectRanger(hero);
    }
}
