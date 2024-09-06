using UnityEngine;

namespace Dialogue.Sample
{
    public class Player : MonoBehaviour
    {
        bool inConversation;

        [SerializeField] public NPC npc;

        #region MonoBehaviour

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Interact();
            }
        }

        private void OnEnable()
        {
            DialogueBoxController.OnDialogueStarted += JoinConversation;
            DialogueBoxController.OnDialogueEnded += LeaveConversation;
        }

        private void OnDisable()
        {
            DialogueBoxController.OnDialogueStarted -= JoinConversation;
            DialogueBoxController.OnDialogueEnded -= LeaveConversation;
        }

        #endregion MonoBehaviour

        void Interact()
        {
            if (inConversation)
            {
                DialogueBoxController.instance.SkipLine();
            }
            else
            {
                if (!npc.FirstInteraction) return;
                DialogueBoxController.instance.StartDialogue(npc.dialogueAsset, npc.StartPosition, npc.npcName);
            }
        }

        #region Event

        void JoinConversation()
        {
            inConversation = true;
        }

        void LeaveConversation()
        {
            inConversation = false;
        }

        #endregion Event
    }

}