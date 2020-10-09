using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    [SerializeField]
    UnityEngine.UI.Text npcNameText = null;
    [SerializeField]
    UnityEngine.UI.Text npcDialouge = null;
    [SerializeField]
    UnityEngine.GameObject dialougeBox = null;

    // text file to dialouge box loading variables
    TextAsset textFile;
    string text;
    string[,] lines;
    int currentLine = 0;
    int maxLines = 0;

    GameObject current = null;
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Initialize();
        dialougeBox.SetActive(false);
        //SoundManager.PlaySound(SoundManager.Sound.BackgroundMusic);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit) && hit.collider.tag == "Player" && dialougeBox.activeSelf == false)
            {
                // Assigns current to the companion hit
                Debug.Log("Player Hit");
                current = hit.collider.gameObject;

                // Assigns Text file to variables
                if(current.GetComponent<BaseCompanion>().GetStat(BaseCompanion.Statistic.Dialouge) < current.GetComponent<BaseCompanion>().Filenames.Length)
                {
                    SetCompanionDialouge(current);
                    currentLine = 0;

                    // incrementing Affection
                    current.GetComponent<BaseCompanion>().ModifyStat(BaseCompanion.Statistic.Affection, 1);
                    Debug.Log(current.GetComponent<BaseCompanion>().GetStat(BaseCompanion.Statistic.Affection));

                    // Assigns Dialouge to true and sets params
                    dialougeBox.SetActive(true);
                    AdvanceDialouge();
                }
            }
            else if(dialougeBox.activeSelf == true)
            {
                AdvanceDialouge();
            }
        }
    }

    void SetCompanionDialouge(GameObject _companion)
    {
        string[] temp;
        textFile = Resources.Load<TextAsset>(current.GetComponent<BaseCompanion>().Filenames[current.GetComponent<BaseCompanion>().GetStat(BaseCompanion.Statistic.Dialouge)]); // Loads file
        current.GetComponent<BaseCompanion>().ModifyStat(BaseCompanion.Statistic.Dialouge, 1);
        text = textFile.ToString(); // Converts to string
        temp = text.Split('\n'); // Splits per newline
        maxLines = temp.Length;

        lines = new string[temp.Length,2];

        for (int i = 0; i < temp.Length; i++)
        {
            string[] temp2;
            temp2 = temp[i].Split(';');

            lines[i,0] = temp2[0];
            lines[i,1] = temp2[1];
        }
    }

    void AdvanceDialouge()
    {
        // Plays sound effect for feedback
        SoundManager.PlaySound(SoundManager.Sound.Boop);

        npcNameText.text = lines[currentLine,0];
        npcDialouge.text = lines[currentLine,1];
        currentLine++;

        if (currentLine >= maxLines)
        {
            // Resetting everything
            dialougeBox.SetActive(false);
            currentLine = 0;
            maxLines = 0;
        }
    }
}
