using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class OrderedButtonController : MonoBehaviour
{

    public string order;

    private string guess = "";
    public bool randomOrder = false;
    public int codeLength = 3;
    public int buttonsConnected;
    public TextMeshPro code;

    private int presses = 0;

    [SerializeField]
    private EventTriggerBase eTB;
    private bool completed = false;

    private void Start()
    {
        if (randomOrder)
            order = Randomize();
        code.text = order;
        Debug.Log(order);
    }

    public void NextButton(int number)
    {
        if (completed)
            return;
        //Debug.Log("Check1");
        int a = order[presses] - 48;
        //Debug.Log("Presses: " + presses);
        //Debug.Log("Incoming Number: " + number);
        //Debug.Log("Current Number: " + a);
        if (number!=a)
        {
            presses = 0;
            guess = "";
            return;
        }
        //Debug.Log("Check2");

        guess += number;
        Debug.Log(guess);
        if (guess.Equals(order))
        {
            eTB.Trigger();
            completed = true;
        }
        presses++;
        
    }

    public string Randomize()
    {
        string randomCode = "";
        /*
        List<int> numbsAvaliable = new List<int> { 1, 2, 3 };

        for (int i = 0; i < numbsAvaliable.Count; i++)
        {
            int temp = numbsAvaliable[i];
            int randomIndex = UnityEngine.Random.Range(i, numbsAvaliable.Count);
            numbsAvaliable[i] = numbsAvaliable[randomIndex];
            numbsAvaliable[randomIndex] = temp;
        }
        for (int i = 0; i < numbsAvaliable.Count; i++)
        {
            
            randomCode += numbsAvaliable[i];
            
        }
        code.text = randomCode;*/

        List<int> code = new List<int>();
        for (int i = 0; i < codeLength; i++)
        {
            //code.Add(Random.Range(1, buttonsConnected+1));
            randomCode += (int)Random.Range(1, buttonsConnected + 1);
        }

        return randomCode;

    }

    

}
