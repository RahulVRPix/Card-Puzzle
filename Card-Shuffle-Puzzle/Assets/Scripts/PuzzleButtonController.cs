using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PuzzleButtonController : MonoBehaviour
{
    public AudioClip[] audioClips;
    public AudioSource audioSource;

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

    [SerializeField]
    private TextMeshProUGUI matchTxt;
    [SerializeField]
    private TextMeshProUGUI turnTxt;
    [SerializeField]
    private TextMeshProUGUI scoreTxt;
    [SerializeField]
    private TextMeshProUGUI highscoreTxt;
    [SerializeField]
    private TextMeshProUGUI gameOverScoreTxt;

    [SerializeField]
    private GameObject scorePanel;
    [SerializeField]
    private GameObject gameOverPanel;
    int scoreCount = 0;

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
        //PlayerPrefs.DeleteAll();
        highscoreTxt.text = PlayerPrefs.GetInt("highscore", 0).ToString();

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
            btn.onClick.AddListener(() => PuzzleClick(btn));
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

    private void PuzzleClick(Button btn)
    {
        audioSource.clip = audioClips[1];
        audioSource.Play();

        PlayAnimation obj = btn.GetComponent<PlayAnimation>();
        obj.OnClickPuzzleButton();

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
            turnTxt.text = "" + countClick;

            StartCoroutine(CheckIfPuzzleMatch());
        }
    }

    IEnumerator CheckIfPuzzleMatch()
    {
        PlayAnimation obj1 = puzzleButtons[firstClickIndex].GetComponent<PlayAnimation>();
        PlayAnimation obj2 = puzzleButtons[secondClickIndex].GetComponent<PlayAnimation>();

        yield return new WaitForSeconds(0.5f);

        if(firstClickPuzzle == secondClickPuzzle)
        {
            Debug.Log("Puzzle Matched!");
            audioSource.clip = audioClips[0];
            audioSource.Play();
            yield return new WaitForSeconds(0.2f);

            //Set button interactable false
            puzzleButtons[firstClickIndex].interactable = false;
            puzzleButtons[secondClickIndex].interactable = false;

            //Play puzzle matched animation
            obj1.PuzzleMatched();
            obj2.PuzzleMatched();

            countCorrectClick++;
            matchTxt.text = "" + countCorrectClick;

            scoreCount += 5;
            scoreTxt.text = "" + scoreCount;            

            if (countCorrectClick == gameGuesses)
            {
                UpdateScore(scoreCount);
                Debug.Log("Game Finished!");
                scorePanel.SetActive(false);
                gameOverPanel.SetActive(true);
            }
        }
        else
        {
            yield return new WaitForSeconds(0.35f);

            puzzleButtons[firstClickIndex].image.sprite = backgroundImage;
            puzzleButtons[secondClickIndex].image.sprite = backgroundImage;

            if(scoreCount > 0 && scoreCount!=1)
            {
                scoreCount -= 2;
                scoreTxt.text = "" + scoreCount;
            }
        }

        yield return new WaitForSeconds(0.2f);
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

    void UpdateScore(int score)
    {
        gameOverScoreTxt.text = "" + score;
        highscoreTxt.text = PlayerPrefs.GetInt("highscore", 0).ToString();

        if (score > PlayerPrefs.GetInt("highscore",0))
        {
            PlayerPrefs.SetInt("highscore", score);
            highscoreTxt.text = PlayerPrefs.GetInt("highscore", 0).ToString();
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Main");
    }
}
