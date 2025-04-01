using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class NPCQuiz : MonoBehaviour
{
    public GameObject quizUI;
    public TextMeshProUGUI questionText;
    public Button[] answerButtons;
    public GameObject failPopup;
    public DoorLock doorToUnlock;
    public TextMeshProUGUI feedbackText;  // Feedback message (correct/incorrect)

    private int currentQuestionIndex = 0;
    private List<Question> questions;
    private int selectedAnswerIndex = -1; // To track currently selected answer
    private Color originalButtonColor; // Store original button color to reset

    private void Start()
    {
        Debug.Log("NPCQuiz script initialized.");

        // Auto-assign UI elements if missing
        if (questionText == null)
        {
            questionText = GameObject.Find("QuestionText")?.GetComponent<TextMeshProUGUI>();
            if (questionText == null)
                Debug.LogError("QuestionText not found in the scene! Assign it manually.");
        }

        if (quizUI == null)
        {
            quizUI = GameObject.Find("QuizPanel");
            if (quizUI == null)
                Debug.LogError("QuizPanel not found in the scene! Assign it manually.");
        }

        if (failPopup != null) failPopup.SetActive(false);

        // Sample phishing email questions (10 questions)
        questions = new List<Question>
        {
            new Question("Which email address is most likely a phishing attempt?",
                new string[]{"support@bank.com", "help@yourbank.com", "support@bank-secure.com", "bank@secure.com"}, 2, "Phishing emails often use slight variations of legitimate domain names."),

            new Question("What should you check before clicking an email link?",
                new string[]{"Shortened URLs", "Exact domain name", "Click first, check later", "Ignore the link"}, 1, "Always check the domain name to ensure it's legitimate."),

            new Question("Which is a common phishing tactic?",
                new string[]{"Sending jokes", "Asking for login credentials", "Just saying Hello", "None of these"}, 1, "Phishing emails often try to steal your login credentials."),

            new Question("What is a suspicious request in an email?",
                new string[]{"Sending pictures", "Requesting money or credentials", "Sending promotional offers", "None of these"}, 1, "Requests for personal information like money or credentials are a red flag."),

            new Question("What is often included in a phishing email?",
                new string[]{"Personalized greeting", "Generic greeting", "File attachments", "Email signature"}, 1, "Phishing emails often use generic greetings such as 'Dear Customer'."),

            new Question("What is a safe way to verify an email?",
                new string[]{"Click on links directly", "Verify by contacting the company directly", "Trust the sender's domain", "Reply and ask questions"}, 1, "Always verify by contacting the company directly using official contact methods."),

            new Question("Which URL should you trust in an email?",
                new string[]{"http://secure.bank.com", "https://secure.bank.com", "http://bank.com", "https://bank-secure.com"}, 1, "Only trust URLs that begin with 'https://' which indicate a secure connection."),

            new Question("What should you never do in a phishing email?",
                new string[]{"Download attachments", "Read the email", "Ignore the email", "Report the email"}, 0, "Never download attachments from unknown or suspicious sources."),

            new Question("What is a red flag in a phishing email?",
                new string[]{"Too good to be true offer", "Request for personal information", "Urgency in the subject", "All of these"}, 3, "Phishing emails often use urgency and offer deals that seem too good to be true."),

            new Question("Which of these can be considered phishing?",
                new string[]{"Request for personal information via SMS", "Random email with links", "Unknown email offering prizes", "All of these"}, 3, "Phishing can occur through email, SMS, or other forms of communication."),

        };

        if (quizUI != null) quizUI.SetActive(false);
        if (feedbackText != null) feedbackText.gameObject.SetActive(false); // Hide feedback initially
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShowQuestion();
        }
    }

    void ShowQuestion()
    {
        if (quizUI != null)
        {
            quizUI.SetActive(true);
            selectedAnswerIndex = -1; // Reset selection
            feedbackText.gameObject.SetActive(false);  // Hide feedback text initially
            DisplayQuestion();
        }
        else
        {
            Debug.LogError("Quiz UI Panel is not assigned!");
        }
    }

    void DisplayQuestion()
    {
        if (currentQuestionIndex < questions.Count)
        {
            questionText.text = questions[currentQuestionIndex].question;
            for (int i = 0; i < answerButtons.Length; i++)
            {
                answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = questions[currentQuestionIndex].options[i];
                answerButtons[i].onClick.RemoveAllListeners();
                int index = i;
                answerButtons[i].onClick.AddListener(() => SelectAnswer(index));
                answerButtons[i].GetComponent<Image>().color = Color.white; // Reset color to default
            }
        }
        else
        {
            // Quiz Passed
            quizUI.SetActive(false);
            if (doorToUnlock != null)
                doorToUnlock.UnlockDoor();
        }
    }

    void SelectAnswer(int index)
    {
        selectedAnswerIndex = index;
        HighlightButton(index);
        CheckAnswer();
    }

    void HighlightButton(int index)
    {
        // Reset color for all buttons
        foreach (Button button in answerButtons)
        {
            button.GetComponent<Image>().color = Color.white; // Reset to original color
        }

        // Highlight the selected button with blue color
        answerButtons[index].GetComponent<Image>().color = Color.blue;
    }

    void CheckAnswer()
    {
        if (selectedAnswerIndex == -1)
        {
            Debug.Log("No answer selected.");
            return;
        }

        // Show feedback
        feedbackText.gameObject.SetActive(true);

        if (selectedAnswerIndex == questions[currentQuestionIndex].correctAnswer)
        {
            feedbackText.text = "Correct! " + questions[currentQuestionIndex].explanation;
            feedbackText.color = Color.green;
        }
        else
        {
            feedbackText.text = "Incorrect! Correct answer: " + questions[currentQuestionIndex].options[questions[currentQuestionIndex].correctAnswer] + ".\n" + questions[currentQuestionIndex].explanation;
            feedbackText.color = Color.red;
        }

        // Move to the next question after 5 seconds
        Invoke("NextQuestion", 5f);
    }

    void NextQuestion()
    {
        currentQuestionIndex++;
        if (currentQuestionIndex < questions.Count)
        {
            DisplayQuestion();
        }
        else
        {
            quizUI.SetActive(false);
            if (doorToUnlock != null)
                doorToUnlock.UnlockDoor();
        }
    }

    private void Update()
    {
        if (quizUI.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) SelectAnswer(0);
            if (Input.GetKeyDown(KeyCode.Alpha2)) SelectAnswer(1);
            if (Input.GetKeyDown(KeyCode.Alpha3)) SelectAnswer(2);
            if (Input.GetKeyDown(KeyCode.Alpha4)) SelectAnswer(3);
            if (Input.GetKeyDown(KeyCode.Return)) CheckAnswer(); // Submit with Enter key
        }
    }
}

// Question Data Structure
[System.Serializable]
public class Question
{
    public string question;
    public string[] options;
    public int correctAnswer;
    public string explanation; // Explanation for the question

    public Question(string q, string[] opts, int correct, string exp)
    {
        question = q;
        options = opts;
        correctAnswer = correct;
        explanation = exp;
    }
}
