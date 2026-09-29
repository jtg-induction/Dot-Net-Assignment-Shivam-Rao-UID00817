using System.Web.Http.Results;

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

        public enum SortDirection
        {
            ASC,
            DESC,
        }
    }

    public static class SortBy
    {
        public const string ORDER_DATE = "order-date";
        public const string ORDER_ID = "order-id";
        public const string AMOUNT = "amount";
        public const string UPDATE_DATE = "update-date";
        public const string ITEM_COUNT = "item-count";
    }

    public static class Status
    {
        public const string PLACED = "placed";
        public const string ACCEPTED = "accepted";
        public const string CANCELLED = "Cancelled";
        public const string REJECTED = "rejected";
        public const string DISPATCHED = "dispatched";
        public const string DELIVERED = "delivered";
    }
}
