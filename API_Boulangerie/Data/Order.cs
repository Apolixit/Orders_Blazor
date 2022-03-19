using API_Orders.Services;
using Shared_Orders.DTO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Shared_Orders.DTO.OrderDTO;

namespace API_Orders
{
    public class Order : Data.ICopy<Order>, IEntitySaveChange
    {
        private static ServicesFactory _services => ServicesFactory.Instance;

        [Key]
        public int ID_Order { get; set; }
        public int? ID_Client { get; set; }
        [ForeignKey("ID_Client")]
        public Client Client { get; set; }
        //Date à laquelle la commande a été passée
        public DateTime CreationDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        //Date à laquelle la commande doit être effectuée
        public DateTime OrderDate { get; set; }
        public StatusOrder Status { get; set; }
        public PaymentOrderType PaymentType { get; set; }
        public bool IsAlreadyPaid { get; set; }
        public string? Commentary { get; set; }
        public IEnumerable<OrderDetails> Details { get; set; }
        public string? AdditionnalDetails { get; set; }

        public enum StatusOrder
        {
            RUNNING,
            FINISHED,
            CANCEL,
            ERROR
        }

        public enum PaymentOrderType
        {
            UNSET,
            CASH,
            CREDIT_CARD,
            CHEQUE,
            CREDIT_NOTE,
            OTHER
        }

        public bool Exist()
        {
            return this.ID_Order > 0;
        }

        public void Copy(Order other)
        {
            this.Client = other.Client;
            this.OrderDate = other.OrderDate;
            this.Status = other.Status;
            this.IsAlreadyPaid = other.IsAlreadyPaid;
            this.PaymentType = other.PaymentType;
            this.Commentary = other.Commentary;
            this.AdditionnalDetails = other.AdditionnalDetails;
            if(this.Details != null && other.Details != null)
            {
                this.Details = other.Details.Select(x => x).ToList();
            }
        }

        public void onInsert()
        {
            this.CreationDate = DateTime.Now;
            this.LastUpdatedDate = DateTime.Now;
        }

        public void onUpdate()
        {
            this.LastUpdatedDate = DateTime.Now;
        }

        #region DTO
        public static IEnumerable<SummaryOrderDTO> LoadSummaryOrder(DateTime dt)
        {
            IList<SummaryOrderDTO> commandesModel = null;
            IEnumerable<API_Orders.Order> commandes = _services.Commandes.Search(new OrderServices.SearchArgument()
            {
                dtCommande = new Utils.DateRange(dt)
            }).data;

            if (commandes != null)
            {
                commandesModel = new List<SummaryOrderDTO>();
                commandesModel.Add(new SummaryOrderDTO()
                {
                    color = System.Drawing.Color.Red,
                    eOrderType = OrderType.UNITARY,
                    nbOrders = commandes.Count()
                });

            }
            return commandesModel;
        }

        public static OrderDTO FromBusinessObject(Order commande)
        {
            if (commande == null) return null;

            return new OrderDTO()
            {
                commentary = commande.Commentary,
                deliveryDate = commande.OrderDate,
                isAlreadyPaid = commande.IsAlreadyPaid,
                status = ToStatutDTO(commande.Status),
                id_order = commande.ID_Order,
                client = Client.FromBusinessObject(commande.Client),
                customDetails = commande.AdditionnalDetails,
                products = null //commande.details?.Select(x => QuantityItem._FromBase(x)).ToList()
            };
        }

        public static StatusCommandeDTO ToStatutDTO(StatusOrder statut)
        {
            switch(statut)
            {
                case StatusOrder.FINISHED:
                    return StatusCommandeDTO.FINISHED;
                case StatusOrder.CANCEL:
                    return StatusCommandeDTO.CANCEL;
                case StatusOrder.ERROR:
                    return StatusCommandeDTO.ERROR;
                default:
                    return StatusCommandeDTO.RUNNING;
            }
        }

        public static StatusOrder FromStatutDTO(StatusCommandeDTO status)
        {
            switch (status)
            {
                case StatusCommandeDTO.FINISHED:
                    return StatusOrder.FINISHED;
                case StatusCommandeDTO.CANCEL:
                    return StatusOrder.CANCEL;
                case StatusCommandeDTO.ERROR:
                    return StatusOrder.ERROR;
                default:
                    return StatusOrder.RUNNING;
            }
        }

        public static Order ToBusinessObject(OrderDTO dto)
        {
            if (dto == null) return null;
            var commande = new API_Orders.Order()
            {
                ID_Order = dto.id_order,
                ID_Client = dto.client?.id_client,
                Client = Client.ToBusinessObject(dto.client),
                OrderDate = dto.deliveryDate,
                IsAlreadyPaid = dto.isAlreadyPaid,
                Status = FromStatutDTO(dto.status),
                PaymentType = PaymentOrderType.CASH,
                Commentary = dto.commentary,
                AdditionnalDetails = dto.customDetails,
                Details = null
            };
            commande.Details = dto.products.Where(x => x.quantity > 0).Select(x => new OrderDetails() { product = Product.ToBusinessObject(x.product), quantity = x.quantity }).ToList();
            return commande;
        }

        public static OrderDTO Get(int id)
        {
            return FromBusinessObject(_services.Commandes.Get(id).data);
        }

        public static IEnumerable<OrderDTO> GetByDate(DateTime dtBegin, DateTime dtEnd)
        {
            IEnumerable<OrderDTO> orderDTO = null;
            IEnumerable<API_Orders.Order> commandesBase = _services.Commandes.Search(new OrderServices.SearchArgument()
            {
                dtCommande = new API_Orders.Utils.DateRange()
                {
                    dtDebut = dtBegin,
                    dtFin = dtEnd
                }
            }).data;
            if (commandesBase != null)
            {
                orderDTO = commandesBase.Select(x => FromBusinessObject(x));
            }
            return orderDTO;
        }
        #endregion
    }
}
