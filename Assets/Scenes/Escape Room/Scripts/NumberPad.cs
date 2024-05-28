using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class NumberPad : MonoBehaviour
{
    [Header("NumberPad Data")]
    public TextMeshProUGUI text;
    public GameObject attachPoint;
    public GameObject keyCard;

    private string code = "1337";
    private string codeForNow = "";

    private Color originalTextColor;

    private bool waitingForReset = false;
    private bool alreadyGaveKeycard = false;
    
    public void EnterDigit(string number)
    {
        if(!waitingForReset && !alreadyGaveKeycard)
        {
            if(number == "back")
            {
                if(codeForNow.Length > 0)
                {
                    codeForNow = codeForNow.Substring(0, codeForNow.Length - 1);
                    text.text = codeForNow;
                }
                
            }
            else if(number == "clear")
            { 
                text.color = Color.blue;
                text.text = "Resetting code";
                waitingForReset = true;
                StartCoroutine(ResetTextAfterDelay(2f));
            }
            else //is a real digit
            {
                codeForNow += number;
                text.text = codeForNow;
                if (codeForNow.Length == 4)
                {
                    originalTextColor = text.color;
                    if (codeForNow == code)
                    {
                        text.color = Color.green;
                        text.text = "Code is correct!";
                        Instantiate(keyCard, attachPoint.transform.position, attachPoint.transform.rotation);
                        var sound = this.GetComponent<PlayDoorSound>();
                        sound.PlayCorrect();
                        alreadyGaveKeycard = true;
                        StartCoroutine(ResetTextAfterCorrectCodeDelay(4f));

                    }
                    else
                    {
                        text.color = Color.red;
                        text.text = "Code is invalid!";
                        var sound = this.GetComponent<PlayDoorSound>();
                        sound.PlayWrong();
                        waitingForReset = true;
                        StartCoroutine(ResetTextAfterDelay(4f));
                    }
                }
            }
        }
        
    }

    private IEnumerator ResetTextAfterDelay(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Reset the text and color
        text.color = originalTextColor;
        text.text = "";
        codeForNow = "";
        waitingForReset = false;
    }

    private IEnumerator ResetTextAfterCorrectCodeDelay(float delay)
    {
        // Wait for the specified delay
        yield return new WaitForSeconds(delay);

        // Reset the text and color
        text.color = originalTextColor;
        text.text = "Pick up the keycard from below!";
    }
}
