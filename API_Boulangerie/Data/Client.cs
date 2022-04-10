using API_Orders.Services;
using Shared_Orders.DTO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_Orders
{
    public class Client : Data.ICopy<Client>
    {
        private static ServicesFactory _services => ServicesFactory.Instance;

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ID_Client { get; set; }
        [Required, MaxLength(500)]
        public string FullName { get; set; }
        [Phone]
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public bool Disabled { get; set; }
        [NotMapped]
        public bool IsActive => !this.Disabled;
        public void Copy(Client other)
        {
            this.FullName = other.FullName;
            this.PhoneNumber = other.PhoneNumber;
            this.Email = other.Email;
            this.Address = other.Address;
        }

        public bool Exist()
        {
            return this.ID_Client > 0;
        }

        

        #region Database mapping
        public static ClientDTO FromBusinessObject(Client client)
        {
            if (client == null) return null;

            return new ClientDTO()
            {
                id_client = client.ID_Client,
                fullName = client.FullName,
                phoneNumber = client.PhoneNumber,
                email = client.Email
            };
        }

        public static Client ToBusinessObject(ClientDTO clientDTO)
        {
            return new Client()
            {
                ID_Client = clientDTO.id_client,
                FullName = clientDTO.fullName,
                PhoneNumber = clientDTO.phoneNumber,
                Email = clientDTO.email
            };
        }

        public static ClientDTO Get(int id)
        {
            return FromBusinessObject(_services.Clients.Get(id).data);
        }

        public static List<ClientDTO> GetAll()
        {
            return _services.Clients.GetAll().data.Select(x => FromBusinessObject(x)).ToList();
        }

        public static List<ClientDTO> Search(ClientServices.SearchArgument criteria)
        {
            return _services.Clients.Search(criteria).data.Select(x => FromBusinessObject(x)).ToList();
        }
        #endregion
    }
}
