using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace API_Boulangerie
{
    public class Part
    {
        [Key]
        public int ID_Part { get; set; }
        [Required]
        public int NbPart { get; set; }
        [Required]
        public string NomPart { get; set; }

        public void Copy(Part other)
        {
            this.NbPart = other.NbPart;
            this.NomPart = other.NomPart;
        }

        public bool Exist()
        {
            return this.ID_Part > 0;
        }
    }
}
