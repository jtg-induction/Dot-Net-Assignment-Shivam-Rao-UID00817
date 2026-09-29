using Dot_Net_Assignment_Shivam_Rao_UID00817.Constants;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces;
using System;
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
                return await _db.Orders.FindAsync(orderId, cancellationToken);
            }
            else
            {
                return await _db.Orders.AsNoTracking().FirstOrDefaultAsync(x => x.OrderId == orderId, cancellationToken);
            }
        }

        /// <summary>
        /// Retrieves orders belonging to a user using pagination.
        /// </summary>
        /// <param name="userId">The ID of the user.</param>
        /// <param name="meta">The Pagination MetaData Object.</param>
        /// <param name="pageNumber">The page number to retrieve.</param>
        /// <param name="pageSize">The size of the page.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>A paginated list of the user's orders.</returns>
        public async Task<List<Orders>> GetOrdersByUserId(long userId, PaginationMetadata meta, int pageNumber = 1, int pageSize = NumberConstants.PAGE_SIZE, CancellationToken cancellationToken = default)
        {
            var query = _db.Orders.Where(x => x.UserId == userId);
            meta.CurrentPage = pageNumber;
            meta.TotalCount = await query.CountAsync();
            meta.PageSize = pageSize;
            return await query.OrderBy(x => x.OrderId)
                                .Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync(cancellationToken);
        }


        /// <summary>
        /// Retrieves all items belonging to an order.
        /// </summary>
        /// <param name="orderId">The ID of the order.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>A list of order items.</returns>
        public async Task<List<Order_Items>> GetOrderItemsByOrderId(long orderId, CancellationToken cancellationToken = default)
        {
            return await _db.Order_Items.Where(x => x.OrderId == orderId).ToListAsync(cancellationToken);
        }


        /// <summary>
        /// Retrieves a paginated list of restaurant orders with search, sorting, and filtering options
        /// </summary>
        /// <param name="restaurantId">The ID of the restaurant.</param>
        /// <param name="meta">The Pagination MetaData Object.</param>
        /// <param name="status">The order status filter.</param>
        /// <param name="pageNumber">>The page number to retrieve.</param>
        /// <param name="pageSize">The page size to retrieve.</param>
        /// <param name="search">The search text used to filter orders by address.</param>
        /// <param name="sortBy">The sorting option.</param>
        /// <param name="sortDirection">The Direction of Sorting.</param>
        /// <param name="City">The city used to filter orders.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>A filtered and paginated list of restaurant orders.</returns>
        public async Task<List<RestaurantOrderHistoryItems>> GetOrdersByRestaurantId(long restaurantId, PaginationMetadata meta, string status = "", int pageNumber = 1, int pageSize = NumberConstants.PAGE_SIZE, string search = "", string sortBy = SortBy.UPDATE_DATE, Enums.SortDirection sortDirection = Enums.SortDirection.DESC, string City = "", CancellationToken cancellationToken = default)
        {
            IQueryable<Orders> query = _db.Orders.Where(x => x.RestaurantId == restaurantId);

            // Search

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                query = query.Where(x => x.AddressLine1.Contains(search));
            }

            //filter
            if (!string.IsNullOrWhiteSpace(status))
            {
                switch (status)
                {
                    case Status.PLACED:
                        query = query.Where(x => x.Status == Enums.OrderStatus.Placed);
                        break;
                    case Status.ACCEPTED:
                        query = query.Where(x => x.Status == Enums.OrderStatus.Accepted);
                        break;
                    case Status.REJECTED:
                        query = query.Where(x => x.Status == Enums.OrderStatus.Rejected);
                        break;
                    case Status.CANCELLED:
                        query = query.Where(x => x.Status == Enums.OrderStatus.Cancelled);
                        break;
                    case Status.DISPATCHED:
                        query = query.Where(x => x.Status == Enums.OrderStatus.Dispatched);
                        break;
                    case Status.DELIVERED:
                        query = query.Where(x => x.Status == Enums.OrderStatus.Delivered);
                        break;
                }
            }

            if (!string.IsNullOrWhiteSpace(City))
            {
                City = City.Trim();
                query = query.Where(x => x.City.Contains(City));
            }


            meta.TotalCount = await query.CountAsync(cancellationToken);
            meta.CurrentPage = pageNumber;
            meta.PageSize = pageSize;

            var projection = query.Select(x => new RestaurantOrderHistoryItems(
                x.OrderId,
                x.Restaurants.Name,
                x.TotalAmount,
                x.Status,
                x.CreatedAt,
                x.UpdatedAt,
                x.AddressLine1,
                x.City,
                x.OrderItems.Count
            ));

            // Sorting
            switch (sortBy)
            {
                case SortBy.ORDER_ID:
                    if (sortDirection == Enums.SortDirection.ASC)
                        projection = projection.OrderBy(x => x.OrderId);
                    else
                        projection = projection.OrderByDescending(x => x.OrderId);
                    break;
                case SortBy.AMOUNT:
                    if (sortDirection == Enums.SortDirection.ASC)
                        projection = projection.OrderBy(x => x.Amount);
                    else
                        projection = projection.OrderByDescending(x => x.Amount);
                    break;
                case SortBy.ORDER_DATE:
                    if (sortDirection == Enums.SortDirection.ASC)
                        projection = projection.OrderBy(x => x.OrderDate);
                    else
                        projection = projection.OrderByDescending(x => x.OrderDate);
                    break;
                default:
                    if (sortDirection == Enums.SortDirection.ASC)
                        projection = projection.OrderBy(x => x.LastUpdated);
                    else
                        projection = projection.OrderByDescending(x => x.LastUpdated);
                    break;
            }

            return await projection.Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .ToListAsync(cancellationToken);
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
        public async Task<Orders> GetOrderWithUpdateLock(long orderId, CancellationToken cancellationToken = default)
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
                                                    " FROM Orders WITH(UPDLOCK, ROWLOCK) WHERE order_id = @p0;", OrderId).FirstOrDefaultAsync(cancellationToken) ?? throw new ValidationException(ErrorMessages.ORDER_DOES_NOT_EXIST);
        }
    }
}
