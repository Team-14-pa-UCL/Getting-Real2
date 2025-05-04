namespace Umove_AS.UI
{
    /// <summary> En hel side (titel + punkter) der kan pushes på navigation‑stakken. </summary>
    public readonly record struct MenuPage(string Title, IReadOnlyList<MenuItem> Items);
}
