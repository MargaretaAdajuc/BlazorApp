namespace PaySys.Server.Application.Promotion
{
    public interface IPromotionManager
    {
        decimal GetDefaultAmount(string currency);
    }
}
