public class AutoConsume : BaseRessource
{
    public int consuption = 0;
    public int sellPrice =1;

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
