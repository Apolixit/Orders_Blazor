using System;
using System.Collections.Generic;
using System.Text;

namespace API_Boulangerie.Services
{
    public interface ICommandeServices
    {
        Data.DbResponse<IEnumerable<Commande>> Search(CommandeServices.SearchArgument criteria);
        Data.DbResponse<Commande> Get(int ID_Commande);
        Data.DbResponse<Commande> Save(Commande commande);
        Data.DbResponse<Commande> Delete(Commande commande);
        Data.DbResponse<Commande.Statut> UpdateStatut(Commande commande, Commande.Statut newStatut);
    }
}
