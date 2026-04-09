namespace NPC
{
    public enum NPCAppearanceConditionType
    {
        None,
        
        //Number of items discovered in the current level
        ItemsDiscovered,
        
        //Number of items crafted in the current level
        ItemsCrafted,

        //Percent of successful deliveries to NPCs
        NPCSuccessRate
    }
}