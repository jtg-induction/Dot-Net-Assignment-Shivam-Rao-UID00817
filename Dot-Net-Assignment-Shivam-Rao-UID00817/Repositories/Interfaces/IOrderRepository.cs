using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Orders CreateOrder(Orders order);
        void AddOrderItems(Order_Items orderItem);
    }
}
