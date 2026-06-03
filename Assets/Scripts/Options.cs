using TMPro;
using UnityEngine;

public class Options : MonoBehaviour
{
    public static int difficulty = 1;
    public TMP_Text difficultyText;


    public void increaseDifficulty()
    {
        if (difficulty < 2)
        {
            difficulty++;
        }
        
    }

    public void decreaseDifficulty()
    {
        if (difficulty > 0)
        {
            difficulty--;
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (difficulty == 0)
        {
            difficultyText.text = "Easy";
        }
        if (difficulty == 1)
        {
            difficultyText.text = "Normal";
        }
        if (difficulty == 2)
        {
            difficultyText.text = "Hard";
        }
    }
}
