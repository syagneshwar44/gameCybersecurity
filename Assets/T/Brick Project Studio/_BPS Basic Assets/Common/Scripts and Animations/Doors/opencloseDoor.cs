using System.Collections;
using UnityEngine.InputSystem; // Import for Player Input
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 


namespace SojaExiles
{
    public class opencloseDoor : MonoBehaviour
    {
        public Animator openandclose;
        public bool open;
        public Transform Player;

        public GameObject quizPanel; // Assign UI Panel
       public TMP_Text questionText; // Assign Question Text
        public Button[] answerButtons; // Assign Answer Buttons
        public TMP_Text resultText; // Assign Result Text

    private PlayerInput playerInput; // Reference to Player Inpu
        private bool playerNearby = false;
        private string correctAnswer = "A"; // Set correct answer for the quiz

        void Start()
        {
    open = false;
    if (quizPanel != null)
        quizPanel.SetActive(false); // Hide the quiz at the start
        }

        void OnMouseOver()
        {
            if (Player)
            {
                float dist = Vector3.Distance(Player.position, transform.position);
                if (dist < 15)
                {
                    if (Input.GetMouseButtonDown(0)) // When player clicks on the door
                    {
                        if (!open)
                        {
                            ShowQuiz(); // Start quiz instead of opening immediately
                        }
                        else
                        {
                            StartCoroutine(closing()); // Close if already open
                        }
                    }
                }
            }
        }

       void ShowQuiz()
{
    quizPanel.SetActive(true);
            playerInput.enabled = false; // Disable player movement
    questionText.text = "What is phishing?";

    answerButtons[0].GetComponentInChildren<TMP_Text>().text = "A. A cyber attack";
    answerButtons[1].GetComponentInChildren<TMP_Text>().text = "B. A fishing method";
    answerButtons[2].GetComponentInChildren<TMP_Text>().text = "C. A computer brand";

    answerButtons[0].onClick.RemoveAllListeners();
    answerButtons[1].onClick.RemoveAllListeners();
    answerButtons[2].onClick.RemoveAllListeners();

    answerButtons[0].onClick.AddListener(() => CheckAnswer("A"));
    answerButtons[1].onClick.AddListener(() => CheckAnswer("B"));
    answerButtons[2].onClick.AddListener(() => CheckAnswer("C"));
}


        void CheckAnswer(string selectedAnswer)
        {
            if (selectedAnswer == correctAnswer)
            {
                resultText.text = "Correct! The door is opening...";
                quizPanel.SetActive(false);
                            playerInput.enabled = true; // Enable player movement
                StartCoroutine(opening());
            }
            else
            {
                resultText.text = "Wrong answer! Try again.";
            }
        }
IEnumerator opening()
{
    print("You are opening the door");
    openandclose.Play("Opening");
    open = true;
    GetComponent<BoxCollider>().enabled = false; // Disable collider when opening
    yield return new WaitForSeconds(.5f);
}

IEnumerator closing()
{
    print("You are closing the door");
    openandclose.Play("Closing");
    open = false;
    GetComponent<BoxCollider>().enabled = true; // Enable collider when closing
    yield return new WaitForSeconds(.5f);
}

    }
}
