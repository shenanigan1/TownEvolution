public class Selection
{
    ECaseType m_goodSelection;
    ECaseType m_badSelection;
    bool m_isGood = true;

    public Selection(ECaseType goodSelection, ECaseType badSelection)
    {
        m_badSelection = badSelection;
        m_goodSelection = goodSelection;    
    }

    public ECaseType GetSelectionTile()
    {
        return m_isGood ? m_goodSelection : m_badSelection;
    }

    public void SetIsGood(bool isGood)
    { 
        m_isGood=isGood;
    }

    public bool GetIsGood()
    {
        return m_isGood;
    }
}
