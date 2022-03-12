using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API_Boulangerie
{
    public class Client : Data.ICopy<Client>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ID_Client { get; set; }
        [Required, MaxLength(500)]
        public string nomComplet { get; set; }
        [Phone]
        public string phoneNumber { get; set; }
        public string email { get; set; }
        public string adresse { get; set; }
        public bool disabled { get; set; }
        public void Copy(Client other)
        {
            this.nomComplet = other.nomComplet;
            this.phoneNumber = other.phoneNumber;
            this.email = other.email;
            this.adresse = other.adresse;
        }

        public bool Exist()
        {
            return this.ID_Client > 0;
        }

        public bool Active()
        {
            return !this.disabled;
        }
    }
}
