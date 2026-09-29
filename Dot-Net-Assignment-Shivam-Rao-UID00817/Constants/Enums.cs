namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Constants
{
    public static class Enums
    {
        public enum Roles
        {
            Customer = 1,
            Owner,
            Admin
        };

        public enum OrderStatus
        {
            Placed = 1,
            Accepted,
            Rejected,
            Dispatched,
            Delivered,
            Cancelled
        }

        public enum SortBy
        {
            OrderId = 1,
            OrderIdDesc,
            Amount,
            AmountDesc,
            OrderDateLatest,
            OrderDateEarliest,
            LastUpdatedLatest,
            LastUpdatedEarliest,
            ItemCount,
            ItemCountDesc
        }

        public enum FilterBy
        {
            StatusPlaced = 1,
            StatusAcceptd,
            StatudRejected,
            StatusDispatched,
            StatusDelivered,
            StatusCancelled,
            Default
        }
    }
}
