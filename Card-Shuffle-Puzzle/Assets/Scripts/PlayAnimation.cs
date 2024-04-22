using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimation : MonoBehaviour
{
    public Animator buttonTransition;

    private void Start()
    {
        buttonTransition = this.GetComponent<Animator>();
    }

    public void OnClickPuzzleButton()
    {
        buttonTransition.SetTrigger("click");
    }

    public void PuzzleMatched()
    {
        buttonTransition.SetTrigger("matched");
    }
}
