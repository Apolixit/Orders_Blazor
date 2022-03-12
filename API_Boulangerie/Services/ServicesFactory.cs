using System;
using System.Collections.Generic;
using System.Text;

namespace API_Boulangerie.Services
{
    public class ServicesFactory
    {
        public IProduitServices ProduitServices {
            get {
                return new ProduitServices();
            }
        }

        public ICommandeServices CommandesServices
        {
            get
            {
                return new CommandeServices();
            }
        }

        public IClientServices ClientServices
        {
            get
            {
                return new ClientServices();
            }
        }

        //public IUtilsServices UtilsServices
        //{
        //    get
        //    {
        //        return new UtilsServices();
        //    }
        //}
    }
}