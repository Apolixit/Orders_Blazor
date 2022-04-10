namespace API_Orders.Services
{
    public interface IServiceFactory
    {
        IOrderServices Commandes { get; }
        IClientServices Clients { get; }
        GetFlatProductCategory Produits { get; }
        IUtilsServices Utils { get; }
    }
}
