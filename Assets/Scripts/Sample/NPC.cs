using UnityEngine;

namespace Dialogue.Sample
{
    public class NPC : MonoBehaviour
    {
        // This field is exposed in the Inspector but cannot be modified directly from outside the class
        [SerializeField] bool firstInteraction = true;
        public bool FirstInteraction
        {
            get => firstInteraction;
            private set => firstInteraction = value;
        }

        // Serialized field for the start position used for repeat interactions
        [SerializeField] int repeatStartPosition;

        public string npcName;
        public DialogueTree dialogueAsset;

        // Start position property that decides the starting point based on whether it's the first interaction
        [HideInInspector]
        public int StartPosition
        {
            get
            {
                if (firstInteraction)
                {
                    firstInteraction = false;
                    return 0;
                }
                else
                {
                    return repeatStartPosition;
                }
            }
        }
    }
}
