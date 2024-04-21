using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleButtonController : MonoBehaviour
{

    [SerializeField]
    private Transform puzzleField;

    [SerializeField]
    private GameObject buttonPrefab;

    public List<Button> puzzleButtons = new List<Button>();

    private void Awake()
    {
        for(int i=0; i<12; i++)
        {
            GameObject go = Instantiate(buttonPrefab);
            go.transform.SetParent(puzzleField, false);
            go.name = "button " + i;

        }
    }

    private void Start()
    {
        GetButtons();
    }

    private void GetButtons()
    {
        GameObject[] gameObject = GameObject.FindGameObjectsWithTag("Puzzle-Button");

        for(int i=0; i<gameObject.Length; i++)
        {
            puzzleButtons.Add(gameObject[i].GetComponent<Button>());
        }
    }
}
