using System;
using System.Collections.Generic;
using System.Text;

namespace API_Orders.Services
{
    public class ServicesFactory : IServiceFactory
    {
        private static ServicesFactory instance = null;
        public static ServicesFactory Instance
        {
            get
            {
                if (instance == null) instance = new ServicesFactory();
                return instance;
            } 
        }

        private ServicesFactory() { }

        public GetFlatProductCategory Produits {
            get 
            {
                return ProductServices.Instance;
            }
        }

        public IOrderServices Commandes
        {
            get
            {
                return OrderServices.Instance;
            }
        }

        public IClientServices Clients
        {
            get
            {
                return ClientServices.Instance;
            }
        }

        public IUtilsServices Utils
        {
            get
            {
                return UtilsServices.Instance;
            }
        }
    }
}