using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;

public class Menu : Manager<Menu>
{
    [SerializeField] private List<Animator> rangersAnimators = new List<Animator>();
    [SerializeField] private List<Characters> characters = new List<Characters>();

    [SerializeField] private List<Hero> heroes = new List<Hero>();

    [SerializeField] private ExtraButton buttonPrefab = null;
    [SerializeField] private CharacterSelectionButton characterButton = null;

    [SerializeField] private Text characterName = null;
    [SerializeField] private Text characterBiography = null;

    [SerializeField] private TMP_Text optionName = null;
    [SerializeField] private TMP_Text rangerName = null;

    [SerializeField] private Transform content = null;
    [SerializeField] private Transform rangerHolder = null;

    [SerializeField] private GameObject model = null;

    [SerializeField] private GameObject mainMenuObjetcs = null;

    [SerializeField] private string nextLevel = "Level";

    [SerializeField] private PlayerProfile player = null;

    private void Start()
    {
        foreach (Animator animator in rangersAnimators)
        {
            animator.SetFloat("Speed", Random.Range(0.3f, 0.5f));
        }

        foreach (Characters character in characters)
        {
            Instantiate(buttonPrefab, content).Initialize(character);
        }

        foreach (Hero hero in heroes)
        {
            Instantiate(characterButton, rangerHolder).Initialize(hero);
        }

        UpdateText(characters[0]);
        SelectRanger(heroes[0]);
    }

    public void UpdateText(Characters character)
    {
        characterName.text = character.characterFullName;
        characterBiography.text = character.characterBackstory;

        ChangeModel(character);
    }

    public void UpdateOptionText(string text)
    {
        optionName.text = text;
    }

    public void UpdateRangerText(Hero hero)
    {
        rangerName.text = hero.human.humanName + ", the " + hero.ranger.rangerName+ ".";
    }

    public void SelectRanger(Hero hero)
    {
        player.currentHero = hero;
        UpdateRangerText(hero);

        ChangeModel(hero.ranger);
    }

    public void ChangeModel(Characters character)
    {
        Material[] _materials = model.GetComponentInChildren<SkinnedMeshRenderer>().materials;
        _materials[0] = character.characterMaterial;
        model.GetComponentInChildren<SkinnedMeshRenderer>().materials = _materials;
    }

    public void StartGame()
    {
        SceneManager.Instance.ChangeScene(nextLevel);
    }

    public void ToggleMainMenu(bool enable)
    {
        mainMenuObjetcs.SetActive(enable);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
