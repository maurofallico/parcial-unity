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
