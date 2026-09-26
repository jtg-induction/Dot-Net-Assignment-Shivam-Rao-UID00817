using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using System.Web;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories
{
    public class OrderRepository: IOrderRepository
    {
        private readonly Restaurant_ManagementContext _db;

        public OrderRepository(Restaurant_ManagementContext db)
        {
            _db = db;
        }

        public Orders CreateOrder(Orders order)
        {
            _db.Orders.Add(order);

            return order;
        }

        public void AddOrderItems(Order_Items orderItem)
        {
            _db.Order_Items.Add(orderItem);
        }
    }
}
