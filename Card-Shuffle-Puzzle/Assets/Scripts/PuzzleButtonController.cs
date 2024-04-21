using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleButtonController : MonoBehaviour
{
    public Sprite backgroundImage;
    [SerializeField]
    private Transform puzzleField;
    [SerializeField]
    private GameObject buttonPrefab;

    public List<Button> puzzleButtons = new List<Button>();
    public Sprite[] puzzleIcons;
    public List<Sprite> gamePuzzleIcons = new List<Sprite>();

    private bool firstClick, secondClick;
    private int firstClickIndex, secondClickIndex;
    private string firstClickPuzzle, secondClickPuzzle;

    private int countClick;
    private int countCorrectClick;
    private int gameGuesses;


    private void Awake()
    {
        for(int i=0; i<12; i++)
        {
            GameObject go = Instantiate(buttonPrefab);
            go.transform.SetParent(puzzleField, false);
            go.name = ""+i;

        }
    }

    private void Start()
    {
        GetButtons();
        ButtonListner();
        AddGamePuzzles();
        RandomPuzzles(gamePuzzleIcons);

        gameGuesses = puzzleButtons.Count / 2;
    }

    private void GetButtons()
    {
        GameObject[] gameObject = GameObject.FindGameObjectsWithTag("Puzzle-Button");

        for(int i=0; i<gameObject.Length; i++)
        {
            puzzleButtons.Add(gameObject[i].GetComponent<Button>());
            puzzleButtons[i].image.sprite = backgroundImage;
        }
    }

    private void ButtonListner()
    {
        foreach(Button btn in puzzleButtons)
        {
            btn.onClick.AddListener(() => PuzzleClick());
        }
    }

    private void AddGamePuzzles()
    {
        int index = 0;
        for(int i=0; i<puzzleButtons.Count; i++)
        {
            if(index ==  puzzleButtons.Count/2)
            {
                index = 0;
            }
            gamePuzzleIcons.Add(puzzleIcons[index]);
            index++;
        }
    }

    private void PuzzleClick()
    {
        string name = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name;
        Debug.Log("Button Clicked!!! " + name); ;

        if(firstClick == false)
        {
            firstClick = true;

            firstClickIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            puzzleButtons[firstClickIndex].image.sprite = gamePuzzleIcons[firstClickIndex];
            firstClickPuzzle = gamePuzzleIcons[firstClickIndex].name;
        }
        else if(secondClick == false)
        {
            secondClick = true;

            secondClickIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);
            puzzleButtons[secondClickIndex].image.sprite = gamePuzzleIcons[secondClickIndex];
            secondClickPuzzle = gamePuzzleIcons[secondClickIndex].name;

            countClick++;
            StartCoroutine(CheckIfPuzzleMatch());
        }
    }

    IEnumerator CheckIfPuzzleMatch()
    {
        yield return new WaitForSeconds(1);

        if(firstClickPuzzle == secondClickPuzzle)
        {
            Debug.Log("Puzzle Matched!");

            yield return new WaitForSeconds(0.5f);
            puzzleButtons[firstClickIndex].interactable = false;
            puzzleButtons[secondClickIndex].interactable = false;

            puzzleButtons[firstClickIndex].image.color = new Color(0, 0, 0, 0);
            puzzleButtons[secondClickIndex].image.color = new Color(0, 0, 0, 0);

            countCorrectClick++;

            if(countCorrectClick == gameGuesses)
            {
                Debug.Log("Game Finished!");
            }
        }
        else
        {
            yield return new WaitForSeconds(0.5f);

            puzzleButtons[firstClickIndex].image.sprite = backgroundImage;
            puzzleButtons[secondClickIndex].image.sprite = backgroundImage;
        }

        yield return new WaitForSeconds(0.5f);
        firstClick = secondClick = false;
    }

    private void RandomPuzzles(List<Sprite> list)
    {
        for(int i=0; i<list.Count; i++)
        {
            Sprite temp = list[i];
            int j = Random.Range(0, list.Count);
            list[i] = list[j];
            list[j] = temp;
        }
    }
}
