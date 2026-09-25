using Dot_Net_Assignment_Shivam_Rao_UID00817.Constants;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ValidationException = Dot_Net_Assignment_Shivam_Rao_UID00817.Exceptions.ValidationException;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly Restaurant_ManagementContext _db;

        public OrderRepository(Restaurant_ManagementContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Creates and adds a new order to the database context.
        /// </summary>
        /// <param name="order">The order to create.</param>
        /// <returns>The created order.</returns>
        public Orders CreateOrder(Orders order)
        {
            _db.Orders.Add(order);

            return order;
        }

        /// <summary>
        /// Adds an order item to the database context.
        /// </summary>
        /// <param name="orderItem">The order item to add.</param>        
        public void AddOrderItems(Order_Items orderItem)
        {
            _db.Order_Items.Add(orderItem);
        }


        /// <summary>
        /// Retrieves an order by its ID, with optional change tracking.
        /// </summary>
        /// <param name="orderId">The ID of the order to retrieve.</param>
        /// <param name="enableTracking">Whether to enable entity tracking.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>The order if found; otherwise, null.</returns>
        public async Task<Orders> GetOrderById(long orderId, bool enableTracking, CancellationToken cancellationToken = default)
        {
            if (enableTracking)
            {
                return await _db.Orders.FindAsync(orderId);
            }
            else
            {
                return await _db.Orders.AsNoTracking().FirstOrDefaultAsync(x => x.OrderId == orderId);
            }
        }

        /// <summary>
        /// Retrieves orders belonging to a user using pagination.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>A paginated list of the user's orders.</returns>
        public async Task<List<Orders>> GetOrdersByUserId(long userId, int pageNumber = 1, CancellationToken cancellationToken = default)
        {
            return await _db.Orders.Where(x => x.UserId == userId).OrderBy(x => x.OrderId).Skip((pageNumber - 1) * NumberConstants.PAGE_SIZE)
                                                                                        .Take(NumberConstants.PAGE_SIZE)
                                                                                        .ToListAsync();
        }


        /// <summary>
        /// Retrieves all items belonging to an order.
        /// </summary>
        /// <param name="orderId">The ID of the order.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>A list of order items.</returns>
        public async Task<List<Order_Items>> GetOrderItems(long orderId, CancellationToken cancellationToken = default)
        {
            return await _db.Order_Items.Where(x => x.OrderId == orderId).ToListAsync();
        }


        /// Retrieves a paginated list of restaurant orders with search, sorting, and filtering options.
        /// </summary>
        /// <param name="restaurantId">The ID of the restaurant.</param>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="search">The search text used to filter orders by address.</param>
        /// <param name="sortBy">The sorting option.</param>
        /// <param name="filterBy">The order status filter.</param>
        /// <param name="filterByCity">The city used to filter orders.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>A filtered and paginated list of restaurant orders.</returns>
        public async Task<List<Orders>> GetOrdersByRestaurantId(long restaurantId, int pageNumber, string search, Enums.SortBy sortBy, Enums.FilterBy filterBy, string filterByCity = "", CancellationToken cancellationToken = default)
        {
            IQueryable<Orders> query = _db.Orders.Where(x => x.RestaurantId == restaurantId);

            // Searching

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(x => x.AddressLine1.ToLower().Contains(search.Trim().ToLower()));
            }

            // Sorting
            switch (sortBy)
            {
                case Enums.SortBy.OrderId:
                    query = query.OrderBy(x => x.OrderId);
                    break;
                case Enums.SortBy.OrderIdDesc:
                    query = query.OrderByDescending(x => x.OrderId);
                    break;
                case Enums.SortBy.Amount:
                    query = query.OrderBy(x => x.TotalAmount);
                    break;
                case Enums.SortBy.AmountDesc:
                    query = query.OrderByDescending(x => x.TotalAmount);
                    break;
                case Enums.SortBy.OrderDateLatest:
                    query = query.OrderByDescending(x => x.CreatedAt);
                    break;
                case Enums.SortBy.OrderDateEarliest:
                    query = query.OrderBy(x => x.CreatedAt);
                    break;
                default:
                    query = query.OrderByDescending(x => x.UpdatedAt);
                    break;
            }
            //filter
            switch (filterBy)
            {
                case Enums.FilterBy.StatusPlaced:
                    query = query.Where(x => x.Status == Enums.OrderStatus.Placed);
                    break;
                case Enums.FilterBy.StatusAcceptd:
                    query = query.Where(x => x.Status == Enums.OrderStatus.Accepted);
                    break;
                case Enums.FilterBy.StatudRejected:
                    query = query.Where(x => x.Status == Enums.OrderStatus.Rejected);
                    break;
                case Enums.FilterBy.StatusCancelled:
                    query = query.Where(x => x.Status == Enums.OrderStatus.Cancelled);
                    break;
                case Enums.FilterBy.StatusDispatched:
                    query = query.Where(x => x.Status == Enums.OrderStatus.Dispatched);
                    break;
                case Enums.FilterBy.StatusDelivered:
                    query = query.Where(x => x.Status == Enums.OrderStatus.Delivered);
                    break;
            }
            if (!string.IsNullOrWhiteSpace(filterByCity)) query = query.Where(x => x.City.ToLower().Contains(filterByCity.Trim().ToLower()));

            return await query.OrderByDescending(x => x.UpdatedAt).Skip((pageNumber - 1) * NumberConstants.PAGE_SIZE)
                                                                                        .Take(NumberConstants.PAGE_SIZE)
                                                                                        .ToListAsync();
        }

        /// <summary>
        /// Retrieves an order by ID while applying an update and row-level lock.
        /// </summary>
        /// <param name="orderId">The ID of the order to retrieve and lock.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>The locked order.</returns>
        /// <exception cref="ValidationException">
        /// Thrown when the specified order does not exist.
        /// </exception>
        public async Task<Orders> GetOrderWithUpdateLockAsync(long orderId, CancellationToken cancellationToken = default)
        {
            var OrderId = new SqlParameter("@p0", orderId);
            return await _db.Orders.SqlQuery("SELECT order_id AS OrderId," +
                                                    "instructions AS Instructions," +
                                                    "status AS Status," +
                                                    "address_line1 AS AddressLine1," +
                                                    "address_line2 AS AddressLine2," +
                                                    "city AS City," +
                                                    "state AS State," +
                                                    "pincode AS Pincode," +
                                                    "country AS Country," +
                                                    "created_at AS CreatedAt," +
                                                    "updated_at AS UpdatedAt," +
                                                    "user_id AS UserId," +
                                                    "restaurant_id AS RestaurantId," +
                                                    "TotalAmount AS TotalAmount" +
                                                    " FROM Orders WITH(UPDLOCK, ROWLOCK) WHERE order_id = @p0;", OrderId).FirstOrDefaultAsync() ?? throw new ValidationException(ErrorMessages.ORDER_DOES_NOT_EXIST);
        }
    }
}
