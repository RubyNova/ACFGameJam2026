namespace CraftingAPI
{
    public record CraftingResult
    {
        public bool Success { get; private set; }
        public ItemConfig Item { get; private set; }

        public CraftingResult(bool success, ItemConfig item)
        {
            Success = success;
            Item = item;
        }
    }
}