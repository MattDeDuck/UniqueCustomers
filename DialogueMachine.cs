using PotionCraft.DialogueSystem.Dialogue;
using PotionCraft.DialogueSystem.Dialogue.Data;
using PotionCraft.Npc.Parts;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

namespace UniqueCustomers
{
    class DialogueMachine
    {
        public static DialogueData CreateDialogueData(NpcTemplate template, string discussionOptionText, string secondText)
        {
            var dialogueData = ScriptableObject.CreateInstance<DialogueData>();

            var startDialogueNodeData = new StartDialogueNodeData
            {
                guid = Guid.NewGuid().ToString(),
            };
            dialogueData.startDialogue = startDialogueNodeData;

            var potionRequestNodeData = new PotionRequestNodeData
            {
                guid = Guid.NewGuid().ToString(),
                morePort = new AnswerData
                {
                    guid = Guid.NewGuid().ToString(),
                    key = template.name + "__morePort__data",
                    text = string.Empty,
                },
            };
            LocalisationMachine.CreateEntry($"{template.name}__morePort__data", discussionOptionText);
            dialogueData.potionRequests.Add(potionRequestNodeData);

            var secondDialoguedata = new DialogueNodeData
            {
                guid = Guid.NewGuid().ToString(),
                key = template.name + "__second__text",
                answers = new List<AnswerData>
                {
                    new AnswerData
                    {
                        guid = Guid.NewGuid().ToString(),
                        key = "__second_data_back",
                        text = string.Empty,
                    },
                },
            };
            LocalisationMachine.CreateEntry($"__second_data_back", "Back to the start");
            LocalisationMachine.CreateEntry($"{template.name}__second__text", secondText);
            dialogueData.dialogues.Add(secondDialoguedata);

            var endNodeData = new EndOfDialogueNodeData
            {
                guid = Guid.NewGuid().ToString(),
            };
            dialogueData.endsOfDialogue.Add(endNodeData);


            // start => potion request
            dialogueData.edges.Add(new EdgeData
            {
                output = startDialogueNodeData.guid,
                input = potionRequestNodeData.guid,
            });

            // potion request => second
            dialogueData.edges.Add(new EdgeData
            {
                output = potionRequestNodeData.morePort.guid,
                input = secondDialoguedata.guid,
            });

            // second answer => potion
            dialogueData.edges.Add(new EdgeData
            {
                output = secondDialoguedata.answers[0].guid,
                input = potionRequestNodeData.guid,
            });

            // potion => end
            dialogueData.edges.Add(new EdgeData
            {
                output = potionRequestNodeData.guid,
                input = endNodeData.guid,
            });

            return dialogueData;
        }

        public static DialogueData GetDialogueData(NpcTemplate template)
        {
            return template.baseParts.OfType<DialogueData>().First();
        }

        public static void SetDialogueData(NpcTemplate template, DialogueData dialogueData)
        {
            int dialogueIndex = Array.FindIndex(template.baseParts, x => x is DialogueData);
            template.baseParts[dialogueIndex] = dialogueData;
        }
    }
}
