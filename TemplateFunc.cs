using PotionCraft.Npc.Parts;
using PotionCraft.QuestSystem;
using PotionCraft.ScriptableObjects.Ingredient;
using System.Collections.Generic;
using UnityEngine;

namespace UniqueCustomers
{
    class TemplateFunc
    {
        public static NpcTemplate CopyNpcTemplate(string templateCopyName, string customName)
        {
            // Grab the template from the list by its name
            var oldTemplate = NpcTemplate.GetByName(templateCopyName);

            // Create a new Npc Template object
            NpcTemplate customTemplate = ScriptableObject.CreateInstance<NpcTemplate>();

            // Give the new template a custom name
            customTemplate.name = customName;

            // Copy the appearance from the template and apply it to the custom one
            customTemplate.appearance = oldTemplate.appearance;

            // Clone the base parts and parts to randomize from the old template
            customTemplate.baseParts = (NonAppearancePart[])oldTemplate.baseParts.Clone();
            customTemplate.partsToRandomize = (PartContainerGroup<NonAppearancePart>[])oldTemplate.partsToRandomize.Clone();

            return customTemplate;
        }

        public static void AddMandatoryRequirements(Quest quest, Dictionary<string, string> requirements)
        {
            quest.mandatoryRequirements.Capacity = requirements.Count;

            quest.mandatoryRequirements.Clear();

            // Loop through the mandatory requirements for the customer
            foreach (KeyValuePair<string, string> mReqs in requirements)
            {
                // Create the mandatory requirement
                QuestRequirementInQuest customMandReq = QuestRequirementInQuest.GetByName(mReqs.Key);

                // Add it to the customer quest
                quest.mandatoryRequirements.Add(customMandReq);

                // Add the ingredient name if needed
                if(mReqs.Value != null)
                {
                    customMandReq.ingredient = Ingredient.GetByName(mReqs.Value);
                }
            }

            quest.useListMandatoryRequirements = true;
        }

        public static void AddOptionalRequirements(Quest quest, Dictionary<string, string> requirements)
        {
            // Set the optional quest capacity to however many requirements we have
            quest.optionalRequirements.Capacity = requirements.Count;

            quest.optionalRequirements.Clear();

            // Loop through the optional requirements for the customer
            foreach (KeyValuePair<string, string> oReqs in requirements)
            {
                // Create the optional requirement
                QuestRequirementInQuest customOptReq = QuestRequirementInQuest.GetByName(oReqs.Key);

                // Add it to the customer quest
                quest.optionalRequirements.Add(customOptReq);

                // Add the ingredient name if needed
                customOptReq.ingredient = Ingredient.GetByName(oReqs.Value);
            }

            quest.useListOptionalRequirements = true;
        }
    }
}
