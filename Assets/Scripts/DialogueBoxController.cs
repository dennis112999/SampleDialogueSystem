using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using Dialogue.Sample;

namespace Dialogue
{
    public class DialogueBoxController : MonoBehaviour
    {
        #region Singleton

        public static DialogueBoxController instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        #endregion Singleton

        [Header("UI Element")]
        [SerializeField] TextMeshProUGUI dialogueText;
        [SerializeField] TextMeshProUGUI nameText;
        [SerializeField] GameObject dialogueBox;
        [SerializeField] GameObject answerBox;
        [SerializeField] Button[] answerButtons;

        public static event Action OnDialogueStarted;
        public static event Action OnDialogueEnded;

        bool skipLineTriggered;
        bool answerTriggered;
        int answerIndex;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dialogueTree"></param>
        /// <param name="startSection"></param>
        /// <param name="name"></param>
        public void StartDialogue(DialogueTree dialogueTree, int startSection, string name)
        {
            ResetBox();
            nameText.text = name + "...";
            dialogueBox.SetActive(true);
            OnDialogueStarted?.Invoke();
            StartCoroutine(RunDialogue(dialogueTree, startSection));
        }

        /// <summary>
        /// Runs the dialogue for the specified section of the dialogue tree.
        /// Displays the dialogue text line by line and handles branching based on user answers.
        /// </summary>
        /// <param name="dialogueTree">The dialogue tree containing sections and dialogue text.</param>
        /// <param name="section">The current section index within the dialogue tree.</param>
        /// <returns>IEnumerator for coroutine.</returns>
        IEnumerator RunDialogue(DialogueTree dialogueTree, int section)
        {
            // Check if the dialogueTree or its sections are null to avoid null reference errors
            if (dialogueTree == null || dialogueTree.sections == null || dialogueTree.sections.Length <= section)
            {
                Debug.LogError("DialogueTree or sections are null or out of bounds.");
                yield break;
            }

            // Iterate through the dialogue lines for the current section
            for (int i = 0; i < dialogueTree.sections[section].dialogue.Length; i++)
            {
                dialogueText.text = dialogueTree.sections[section].dialogue[i];
                dialogueText.GetComponent<TextAnimation>().StartAnimation();

                // Wait for the player to skip the line (usually by pressing a key or button)
                while (!skipLineTriggered)
                {
                    yield return null;
                }

                // Reset the skip trigger for the next line
                skipLineTriggered = false;
            }

            // Check if the dialogue section is marked as ending after dialogue
            if (dialogueTree.sections[section].endAfterDialogue)
            {
                dialogueBox.SetActive(false);
                OnDialogueEnded?.Invoke();
                yield break;
            }

            dialogueText.text = dialogueTree.sections[section].branchPoint.question;
            dialogueText.GetComponent<TextAnimation>().StartAnimation();
            ShowAnswers(dialogueTree.sections[section].branchPoint);

            // Wait for the player to select an answer
            while (!answerTriggered)
            {
                yield return null;
            }

            answerBox.SetActive(false);
            answerTriggered = false;

            int nextSection = dialogueTree.sections[section].branchPoint.answers[answerIndex].nextElement;

            StartCoroutine(RunDialogue(dialogueTree, nextSection));
        }

        void ResetBox()
        {
            StopAllCoroutines();
            dialogueBox.SetActive(false);
            answerBox.SetActive(false);
            skipLineTriggered = false;
            answerTriggered = false;
        }

        /// <summary>
        /// Displays the selectable answers from the given branch point and sets up click listeners.
        /// </summary>
        /// <param name="branchPoint">The branch point containing the answers to be shown</param>
        void ShowAnswers(BranchPoint branchPoint)
        {
            // Reveals the selectable answers and sets their text values
            answerBox.SetActive(true);
            for (int i = 0; i < 3; i++)
            {
                if (i < branchPoint.answers.Length)
                {
                    answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = branchPoint.answers[i].answerLabel;
                    answerButtons[i].gameObject.SetActive(true);

                    // Remove any previously added listeners to avoid stacking them
                    answerButtons[i].onClick.RemoveAllListeners();

                    // Add listener with deferred invocation
                    int answerIndex = branchPoint.answers[i].nextElement;
                    answerButtons[i].onClick.AddListener(() => AnswerQuestion(answerIndex));
                }
                else
                {
                    answerButtons[i].gameObject.SetActive(false);
                }
            }
        }

        public void SkipLine()
        {
            skipLineTriggered = true;
        }

        private void AnswerQuestion(int answer)
        {
            answerIndex = answer;
            answerTriggered = true;
        }
    }

}