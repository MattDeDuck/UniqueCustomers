using PotionCraft.DialogueSystem.Dialogue;
using PotionCraft.Npc.Parts;
using PotionCraft.QuestSystem;
using PotionCraft.ScriptableObjects;
using System.Collections.Generic;
using System;
using System.Linq;

namespace UniqueCustomers
{
    class Customer
    {
        public static void CreateCustomer(string templateToCopy, string templateNpcName, string questName, string questText, List<string> customDesired, int dayToAppear, bool hasCustomDialogue, Dictionary<string, string> mandatory = null, Dictionary<string, string> optional = null, string discussionText = null, string secondText = null)
        {
            // Get the NPC template to create our customer from
            NpcTemplate customTemplate = TemplateFunc.CopyNpcTemplate(templateToCopy, templateNpcName);

            if (customTemplate == null)
            {
                Plugin.Log.LogError($"Could not find template '{templateToCopy}'");
                return;
            }

            // Add the customer quest text to the localisation data, checking if it doesn't exist
            LocalisationMachine.CreateEntry("quest_text_" + questName, questText);

            // Grab the quest from the custom template
            Quest customQuest = customTemplate.baseParts.OfType<Quest>().First();

            // Give the customer a name
            customQuest.name = questName;

            // Mandatory Requirements
            if (mandatory != null && mandatory.Count < 3)
            {
                TemplateFunc.AddMandatoryRequirements(customQuest, mandatory);
            }

            // Optional Requirements
            if (optional != null && optional.Count < 3)
            {
                TemplateFunc.AddOptionalRequirements(customQuest, optional);
            }

            // Create a list to add the desired effects to
            List<PotionEffect> deToAdd = new();
            deToAdd.Clear();

            foreach (var de in customDesired)
            {
                // Search for effect name
                PotionEffect newEffect = PotionEffect.GetByName(de);
                // Add it to the list
                deToAdd.Add(newEffect);
            }

            // Add the desired effects for the quest
            customQuest.desiredEffects = deToAdd.ToArray();

            if(hasCustomDialogue)
            {
                DialogueData customTemplateDialogue = DialogueMachine.GetDialogueData(customTemplate);
                int dialogueIndex = Array.FindIndex(customTemplate.baseParts, x => x is DialogueData);
                customTemplate.baseParts[dialogueIndex] = DialogueMachine.CreateDialogueData(customTemplate, discussionText, secondText);
            }

            NpcTemplate.allNpcTemplates.templates.Add(customTemplate);
            Plugin.customNpcTemplates.Add(Tuple.Create(customTemplate, customTemplate.name), dayToAppear);
            Plugin.Log.LogInfo($"NPC: {customTemplate.name} created >> Day {dayToAppear}");
        }
    }
}