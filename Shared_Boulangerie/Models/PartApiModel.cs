using API_Boulangerie.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shared_Boulangerie.Models.Api
{
    public class PartApiModel
    {
        private static ServicesFactory _services => new ServicesFactory();
        public int id_part { get; set; }
        public int nbPart { get; set; }
        public string nom { get; set; }

        #region Mapping avec la base

        //public static PartApiModel _FromBase(Part part)
        //{
        //    if (part == null) return null;
        //    return new PartApiModel() {
        //        id_part = part.ID_Part,
        //        nbPart = part.NbPart,
        //        nom = part.NomPart
        //    };
        //}

        //public Part _ToBase()
        //{
        //    return new Part()
        //    {
        //        ID_Part = id_part,
        //        NbPart = nbPart,
        //        NomPart = nom
        //    };
        //}
        #endregion

        //public static IEnumerable<PartApiModel> GetAll()
        //{
        //    return _services.ProduitServices.GetAllPart().data.Select(x => PartApiModel._FromBase(x));
        //}

        //public static PartApiModel Get(int id)
        //{
        //    return PartApiModel._FromBase(_services.ProduitServices.GetPart(id).data);
        //}
    }
}
