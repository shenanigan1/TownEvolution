public class AutoConsume : BaseRessource, IProduction
{
    private int m_consuption = 0;
    private int m_sellPrice = 1;
    public int consuption { get { return m_consuption; } set { m_consuption = value; } }
    public int sellPrice { get { return m_sellPrice; } set { m_sellPrice = value; } }

    public virtual int UpdateProduction()
    {
        int production = quantity - consuption;

        if(production > 0)
        {
            return (production) * sellPrice;
        }
        return production;
    }
}

public interface IProduction
{
    public int consuption { get; set; }
    public int sellPrice { get; set; }
    public int UpdateProduction() { return 0; }
}