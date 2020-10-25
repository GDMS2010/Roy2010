using System;
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
    [SerializeField]
    UnityEngine.UI.Text dialogueOptionA = null;
    [SerializeField]
    UnityEngine.UI.Text dialogueOptionB = null;
    [SerializeField]
    UnityEngine.UI.Text dialogueOptionC = null;
    [SerializeField]
    UnityEngine.GameObject dialougeOptionsBox = null;
    [SerializeField]
    UnityEngine.GameObject continueButton = null;
    [SerializeField]
    UnityEngine.UI.Image characterPortrait  = null;
    [SerializeField]
    UnityEngine.GameObject characterPortraitObject = null;
    [SerializeField]
    UnityEngine.GameObject ventureButton = null;
    [SerializeField]
    UnityEngine.GameObject craftTable = null;

    // text file to dialouge box loading variables
    TextAsset textFile;
    string text;
    string[,] lines;
    int nextIndex = 0;
    int optionAIndex = 0;
    int optionBIndex = 0;
    int optionCIndex = 0;
    int optionAAffection = 0;
    int optionBAffection = 0;
    int optionCAffection = 0;

    GameObject current = null;
    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Initialize();
        dialougeBox.SetActive(false);
        dialougeOptionsBox.SetActive(false);
        characterPortraitObject.SetActive(false);
        ventureButton.SetActive(true);
        craftTable.SetActive(false);

        // Setting cursor to visible and confined
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
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
                    nextIndex = 0;

                    // incrementing Affection
                    current.GetComponent<BaseCompanion>().ModifyStat(BaseCompanion.Statistic.Affection, 1);
                    Debug.Log(current.GetComponent<BaseCompanion>().GetStat(BaseCompanion.Statistic.Affection));

                    // Assigns Dialouge to true and sets params
                    dialougeBox.SetActive(true);
                    ventureButton.SetActive(false);
                    AdvanceDialouge(nextIndex);
                }
            }
            else if(Physics.Raycast(ray, out hit) && hit.collider.tag == "Craft" && dialougeBox.activeSelf == false)
            {
                craftTable.SetActive(true);
                ventureButton.SetActive(false);
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

        lines = new string[temp.Length,14];

        for (int i = 0; i < temp.Length; i++)
        {
            string[] temp2;
            temp2 = temp[i].Split(';');

            for (int j = 0; j < temp2.Length; j++)
            {
                lines[i, j] = temp2[j];
            }
        }
    }

    void AdvanceDialouge(int _index)
    {
        if (_index < 0)
        {
            // Resetting everything
            dialougeBox.SetActive(false);
            dialougeOptionsBox.SetActive(false);
            characterPortraitObject.SetActive(false);
            ventureButton.SetActive(true);
            nextIndex = 0;
            return;
        }

        // Plays sound effect for feedback
        SoundManager.PlaySound(SoundManager.Sound.Boop);

        npcNameText.text = lines[_index,0];
        npcDialouge.text = lines[_index,1];
        characterPortraitObject.SetActive(true);

        if (lines[_index, 2].Equals("false"))
        {
            dialougeOptionsBox.SetActive(false);
            continueButton.SetActive(true);
            nextIndex = Int32.Parse(lines[_index, 3]);
            characterPortrait.sprite = Resources.Load<Sprite>(lines[_index, 4]);
            return;
        }
        else
        {
            dialougeOptionsBox.SetActive(true);
            continueButton.SetActive(false);

            dialogueOptionA.text = lines[_index, 3];
            dialogueOptionB.text = lines[_index, 6];
            dialogueOptionC.text = lines[_index, 9];

            optionAIndex = Int32.Parse(lines[_index, 4]);
            optionBIndex = Int32.Parse(lines[_index, 7]);
            optionCIndex = Int32.Parse(lines[_index, 10]);

            optionAAffection = Int32.Parse(lines[_index, 5]);
            optionBAffection = Int32.Parse(lines[_index, 8]);
            optionCAffection = Int32.Parse(lines[_index, 11]);

            characterPortrait.sprite = Resources.Load<Sprite>(lines[_index, 12]);
        }
    }

    // Pass 1 for continue, 2 for option A, 3 for option B, 4 for option C
    public void ContinueButton(int _button)
    {
        switch (_button)
        {
            default:
                break;

            case 1:
                AdvanceDialouge(nextIndex);
                break;

            case 2:
                current.GetComponent<BaseCompanion>().ModifyStat(BaseCompanion.Statistic.Affection, optionAAffection);
                AdvanceDialouge(optionAIndex);
                break;

            case 3:
                current.GetComponent<BaseCompanion>().ModifyStat(BaseCompanion.Statistic.Affection, optionBAffection);
                AdvanceDialouge(optionBIndex);
                break;

            case 4:
                current.GetComponent<BaseCompanion>().ModifyStat(BaseCompanion.Statistic.Affection, optionCAffection);
                AdvanceDialouge(optionCIndex);
                break;

            case 5:
                craftTable.SetActive(false);
                ventureButton.SetActive(true);
                break;
        }
    }
}
