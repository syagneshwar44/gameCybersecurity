
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
    public TextMeshProUGUI feedbackText; // Feedback message (correct/incorrect)
    public GameObject player; // Player object reference

    private int currentQuestionIndex = 0;
    private List<Question> questions;
    private int selectedAnswerIndex = -1; // Track selected answer
    private bool isPlayerNear = false; // Track if the player is near

    private void Start()
    {
        Debug.Log("NPCQuiz script initialized.");

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
                        new Question("What is the primary goal of phishing emails?",
                new string[]{"To entertain", "To steal personal information", "To provide security updates", "To promote social media"}, 1, "Phishing emails aim to trick users into revealing personal information."),

            new Question("Which of the following is an example of vishing?",
                new string[]{"A fake email from your bank", "A fraudulent call asking for your OTP", "A website asking for login details", "A pop-up ad offering a prize"}, 1, "Vishing refers to voice phishing, where attackers use phone calls to steal information."),

            new Question("What is a common sign of a phishing website?",
                new string[]{"A lock icon in the URL bar", "Misspelled domain name", "HTTPS in the URL", "A professional-looking design"}, 1, "Phishing websites often use misspelled domains to appear legitimate."),

            new Question("How can you confirm if an email from your bank is legitimate?",
                new string[]{"Click the link and check", "Call the bank using official contact details", "Reply to the email", "Forward it to a friend"}, 1, "Always contact the bank directly using their official phone number."),

            new Question("What does a phishing attack often try to steal?",
                new string[]{"Your photos", "Your credentials and financial data", "Your browser history", "Your contacts"}, 1, "Phishing attacks aim to steal sensitive information like credentials and financial data."),

            new Question("What should you do if you receive a suspicious phone call asking for your bank details?",
                new string[]{"Give the details to avoid trouble", "Hang up and call the bank directly", "Ask them for more details", "Ignore and hope they don’t call again"}, 1, "Always verify requests by calling the bank directly on an official number."),

            new Question("Which of the following is NOT a typical phishing method?",
                new string[]{"Emails with fake links", "Fake phone calls", "Fake pop-up ads", "Legitimate SMS from your bank"}, 3, "Legitimate SMS from your bank should not be confused with smishing, which is fraudulent text messaging."),

            new Question("How can you protect yourself from phishing scams?",
                new string[]{"By using a strong antivirus", "By clicking on only verified links", "By verifying URLs and senders", "By responding quickly to urgent emails"}, 2, "Always verify the sender and URL before clicking on any links."),

            new Question("Which type of phishing is targeted at a specific individual or organization?",
                new string[]{"Mass phishing", "Spear phishing", "Clone phishing", "Whaling"}, 1, "Spear phishing targets specific individuals or organizations."),

            new Question("What should you check before providing personal information over the phone?",
                new string[]{"The caller's phone number", "The caller's background", "The legitimacy of the request", "The time of the call"}, 2, "Always verify the legitimacy of the request before sharing personal details."),

            new Question("What is smishing?",
                new string[]{"Phishing through SMS", "Phishing through social media", "Phishing through fake websites", "Phishing through fake phone calls"}, 0, "Smishing is phishing through fraudulent SMS messages."),

            new Question("Which of the following should NOT be done when receiving an unexpected email from your bank?",
                new string[]{"Click on links in the email", "Call the bank directly", "Check for grammatical errors", "Verify the sender's email address"}, 0, "Never click on links in unexpected emails claiming to be from your bank."),

            new Question("What is the best way to report a phishing attempt?",
                new string[]{"Ignore it", "Forward it to friends", "Report it to your IT team or cybersecurity authority", "Reply to the email asking for verification"}, 2, "Reporting phishing attempts helps prevent future attacks."),

            new Question("Which of the following is NOT a good cybersecurity practice?",
                new string[]{"Using different passwords for different accounts", "Clicking on unknown links", "Enabling two-factor authentication", "Updating passwords regularly"}, 1, "Clicking on unknown links increases the risk of phishing attacks."),

            new Question("What is a common way to identify a phishing email?",
                new string[]{"It contains an urgent request", "It is well-written", "It is sent by your friend", "It has a common subject line"}, 0, "Phishing emails often create urgency to trick users into acting quickly."),

            new Question("How does vishing differ from phishing?",
                new string[]{"Vishing is done via phone calls", "Vishing is done via email", "Phishing only occurs in person", "Vishing uses social media"}, 0, "Vishing is voice phishing conducted via phone calls."),

            new Question("What is a fake phone call pretending to be from tech support an example of?",
                new string[]{"Phishing", "Vishing", "Smishing", "Hacking"}, 1, "Vishing involves fraudulent phone calls asking for sensitive information."),

            new Question("Why do cybercriminals use social engineering in phishing attacks?",
                new string[]{"To trick users into revealing information", "To write better emails", "To increase their website traffic", "To improve their grammar"}, 0, "Social engineering manipulates users into providing sensitive information."),

            new Question("What should you do if you accidentally clicked on a phishing link?",
                new string[]{"Close the page and hope for the best", "Enter your credentials quickly", "Immediately change your password and scan for malware", "Ignore it"}, 2, "Changing your password and scanning for malware reduces the risk of compromise."),

            new Question("What should you do if you receive an email from your 'bank' that you weren’t expecting?",
                new string[]{"Click on the link to check", "Call your bank using the number in the email", "Verify with your bank using official contact details", "Reply to the email for clarification"}, 2, "Always verify emails with your bank using official contact details."),

            new Question("How can you recognize an email that might be a phishing attempt?",
                new string[]{"It is poorly formatted", "It contains urgent requests", "It asks for sensitive information", "All of the above"}, 3, "Phishing emails often have poor formatting, urgency, and requests for sensitive data."),

            new Question("Which of these could be a sign of vishing?",
                new string[]{"A phone call asking for your OTP", "An email asking you to reset your password", "A text message with a verification link", "A website pop-up asking for credentials"}, 0, "Vishing involves fraudulent phone calls, often asking for sensitive details."),

            new Question("Which of these is a best practice to avoid phishing?",
                new string[]{"Click on links in emails", "Verify the sender's details before responding", "Use the same password everywhere", "Respond quickly to urgent requests"}, 1, "Always verify the sender's identity before responding."),
       
        
        };

        if (quizUI != null) quizUI.SetActive(false);
        if (feedbackText != null) feedbackText.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            ShowQuestion();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            quizUI.SetActive(false); // Hide quiz panel when the player moves away
        }
    }

    void ShowQuestion()
    {
        if (quizUI != null && isPlayerNear)
        {
            quizUI.SetActive(true);
            selectedAnswerIndex = -1; // Reset selection
            feedbackText.gameObject.SetActive(false);
            DisplayQuestion();
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
                answerButtons[i].GetComponent<Image>().color = Color.white; // Reset button color
            }
        }
        else
        {
            // Quiz completed, unlock the door if needed
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
            button.GetComponent<Image>().color = Color.white;
        }

        // Highlight selected button
        answerButtons[index].GetComponent<Image>().color = Color.green;
    }

    void CheckAnswer()
    {
        if (selectedAnswerIndex == -1)
        {
            Debug.Log("No answer selected.");
            return;
        }

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

        Invoke("NextQuestion", 5f); // Move to the next question after 5 seconds
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
            if (Input.GetKeyDown(KeyCode.Return)) CheckAnswer(); // Submit answer with Enter key
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
    public string explanation;

    public Question(string q, string[] opts, int correct, string exp)
    {
        question = q;
        options = opts;
        correctAnswer = correct;
        explanation = exp;
    }
}
