using API_Orders.Services;
using Shared_Orders.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace API_Orders
{
    public class Portion
    {
        private static ServicesFactory _services => ServicesFactory.Instance;

        [Key]
        public int ID_Portion { get; set; }
        [Required]
        public int NbPortion { get; set; }
        [Required]
        public string? Name { get; set; }

        public void Copy(Portion other)
        {
            this.NbPortion = other.NbPortion;
            this.Name = other.Name;
        }

        public bool Exist()
        {
            return this.ID_Portion > 0;
        }

        #region Database mapping

        public static PortionDTO? FromBusinessObject(Portion part)
        {
            if (part == null) return null;
            return new PortionDTO()
            {
                id_portion = part.ID_Portion,
                nbPortion = part.NbPortion,
                name = part.Name
            };
        }

        public static Portion ToBusinessObject(PortionDTO partDTO)
        {
            return new Portion()
            {
                ID_Portion = partDTO.id_portion,
                NbPortion = partDTO.nbPortion,
                Name = partDTO.name
            };
        }
        #endregion

        public static IEnumerable<PortionDTO> GetAll()
        {
            return _services.Produits.GetAllPart().data.Select(x => FromBusinessObject(x));
        }

        public static PortionDTO Get(int id)
        {
            return FromBusinessObject(_services.Produits.GetPart(id).data);
        }
    }
}
