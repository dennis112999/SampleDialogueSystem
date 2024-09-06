using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue
{
    [CreateAssetMenu]
    public class DialogueTree : ScriptableObject
    {
        public DialogueSection[] sections;
    }

    /// <summary>
    /// 
    /// </summary>
    [System.Serializable]
    public struct DialogueSection
    {
        [TextArea]
        public string[] dialogue;
        public bool endAfterDialogue;
        public BranchPoint branchPoint;
    }

    /// <summary>
    /// 
    /// </summary>
    [System.Serializable]
    public struct BranchPoint
    {
        [TextArea]
        public string question;
        public Answer[] answers;
    }

    /// <summary>
    /// 
    /// </summary>
    [System.Serializable]
    public struct Answer
    {
        public string answerLabel;
        public int nextElement;
    }
}
