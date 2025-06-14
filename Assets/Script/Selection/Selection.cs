/// <summary>
/// Represents a selection state with two tile types:
/// one for a "good" selection and one for a "bad" selection.
/// </summary>
public class Selection
{
    private readonly ECaseType m_goodSelection;
    private readonly ECaseType m_badSelection;
    private bool m_isGood = true;

    /// <summary>
    /// Initializes a new instance of the Selection class with specified tile types.
    /// </summary>
    /// <param name="goodSelection">Tile type for good selection.</param>
    /// <param name="badSelection">Tile type for bad selection.</param>
    public Selection(ECaseType goodSelection, ECaseType badSelection)
    {
        m_goodSelection = goodSelection;
        m_badSelection = badSelection;
    }

    /// <summary>
    /// Gets the current selection tile type depending on whether the selection is good.
    /// </summary>
    public ECaseType GetSelectionTile()
    {
        return m_isGood ? m_goodSelection : m_badSelection;
    }

    /// <summary>
    /// Sets the selection state.
    /// </summary>
    /// <param name="isGood">True if the selection is good; otherwise false.</param>
    public void SetIsGood(bool isGood)
    {
        m_isGood = isGood;
    }

    /// <summary>
    /// Returns whether the current selection state is good.
    /// </summary>
    public bool GetIsGood()
    {
        return m_isGood;
    }
}
