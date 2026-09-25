using Dot_Net_Assignment_Shivam_Rao_UID00817.Constants;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Exceptions;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using ValidationException = Dot_Net_Assignment_Shivam_Rao_UID00817.Exceptions.ValidationException;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUserRepository _userRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOwnerManagesRestaurantsRepository _ownerManagesRestaurantsRepository;
        private readonly Restaurant_ManagementContext _db;

        public OrderService(IUserRepository userRepository, IItemRepository itemsRepository, IAddressRepository addressRepository, IRestaurantRepository restaurantRepository, IOrderRepository orderRepository, IUnitOfWork unitOfWork, IOwnerManagesRestaurantsRepository ownerManagesRestaurantsRepository, Restaurant_ManagementContext db)
        {
            _addressRepository = addressRepository;
            _db = db;
            _itemRepository = itemsRepository;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _restaurantRepository = restaurantRepository;
            _orderRepository = orderRepository;
            _ownerManagesRestaurantsRepository = ownerManagesRestaurantsRepository;
        }

        /// <summary>
        /// Creates a new order into the orders table for the user. While Validating all the details.
        /// </summary>
        /// <param name="transaction">The transaction object of the current ongoing transaction.</param>
        /// <param name="user">Details of the user for whom the order is to be created.</param>
        /// <param name="address">Address Details of the user.</param>
        /// <param name="restaurant">Restaurant Details from where the order is to be placed.</param>
        /// <param name="order">The Order details (Items and quantity, address id, restaurant id, instructions(optional))</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>An object with the details of the placed order i.e. Order Id, address, items etc.</returns>
        public async Task<OrderResponseDto> PlaceOrderAsync(DbContextTransaction transaction,Users user, Addresses address, Restaurants restaurant, OrderRequestDto order, CancellationToken cancellationToken = default)
        {
            long userId = user.UserId;

            var requestedItems = order.Items.OrderBy(x => x.ItemId).ToList();

            var itemIds = requestedItems.Select(x => x.ItemId).ToList();

            var items = await _itemRepository.GetItemsWithUpdateLockAsync(itemIds , restaurant.RestaurantId);

            if (items.Count != requestedItems.Count) throw new ValidationException(ErrorMessages.INVALID_ITEMS);

            var itemDictionary = items.ToDictionary(x => x.ItemId);

            decimal total = 0;

            foreach (var requestedItem in requestedItems)
            {
                if (requestedItem.Quantity <= itemDictionary[requestedItem.ItemId].AvailableQuantity)
                {
                    itemDictionary[requestedItem.ItemId].AvailableQuantity -= requestedItem.Quantity;
                    total += itemDictionary[requestedItem.ItemId].Price * requestedItem.Quantity;
                }
                else
                {
                    throw new ValidationException($"Insufficient quantity for item {itemDictionary[requestedItem.ItemId].Name}");
                }
            }

            if (user.WalletBalance < total) throw new ValidationException(ErrorMessages.INSUFFICIENT_WALLET_BALANCE);
            else user.WalletBalance -= total;

            var newOrder = new Orders(
                userId ,
                order.RestaurantId ,
                total ,
                address.AddressLine1 ,
                address.City ,
                address.State ,
                address.Pincode ,
                address.Country ,
                address.AddressLine2 ,
                order.Instructions
            );

            _orderRepository.CreateOrder(newOrder);

            foreach (var requestedItem in requestedItems)
            {
                var item = itemDictionary[requestedItem.ItemId];

                var orderItem = new Order_Items
                {
                    Orders = newOrder ,
                    ItemId = item.ItemId ,
                    Name = item.Name ,
                    ItemPrice = item.Price ,
                    Quantity = requestedItem.Quantity
                };

                _orderRepository.AddOrderItems(orderItem);
            }

            await _unitOfWork.SaveChangesAsync();

            transaction.Commit();

            return new OrderResponseDto
            {
                OrderId = newOrder.OrderId ,
                UserId = newOrder.UserId ,
                RestaurantId = newOrder.RestaurantId ,
                TotalAmount = newOrder.TotalAmount ,
                Status = newOrder.Status.ToString() ,

                Items = newOrder.OrderItems.Select(x => new OrderItemResponseDto
                {
                    ItemId = x.ItemId ,
                    Name = x.Name ,
                    ItemPrice = x.ItemPrice ,
                    Quantity = x.Quantity ,
                    TotalPrice = x.ItemPrice * x.Quantity
                }).ToList()
            };
        }

        /// <summary>
        /// Places the order with given details and items.
        /// </summary>
        /// <param name="userId">The User Id of the requesting user.</param>
        /// <param name="order">Order details with which the order is to be placed (items, address, restaurant)</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>An object with the details of the placed order i.e. Order Id, address, items etc.</returns>
        public async Task<OrderResponseDto> PlaceOrderAsync(long userId , OrderRequestDto order, CancellationToken cancellationToken = default)
        {
            var restaurant = await _restaurantRepository.GetRestaurantByIdAsync(order.RestaurantId , false) ?? throw new ValidationException(ErrorMessages.RESTAURANT_DOES_NOT_EXIST);

            var address = await _addressRepository.GetAddressAsync(order.AddressId , false) ?? throw new ValidationException(ErrorMessages.ADDRESS_DOES_NOT_EXIST);

            var user = await _userRepository.GetUserWithUpdateLockAsync(userId) ?? throw new ValidationException(ErrorMessages.USER_DOES_NOT_EXIST);

            if (order.Items is null || order.Items.Count == 0) throw new ValidationException(ErrorMessages.ATLEAST_ONE_ITEM_REQUIRED);

            using (var transaction = _db.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
            {
                try
                {
                    return await PlaceOrderAsync(transaction, user, address, restaurant, order);
                }
                catch (ValidationException e)
                {
                    transaction.Rollback();
                    throw e;
                }catch (Exception e)
                {
                    transaction.Rollback();
                    throw e;
                }
            }
        }

        /// <summary>
        /// Retrieve list of all the orders for the requesting user.
        /// </summary>
        /// <param name="userId">The User Id of the user whose orders are to be retrieved.</param>
        /// <param name="pageNumber">The Page Number.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>A list of all the orders of the customer.</returns>
        public async Task<GetCustomerOrdersDto> GetAllOrdersAsync(long userId, int pageNumber, CancellationToken cancellationToken = default)
        {
            var orders = await _orderRepository.GetOrdersByUserId(userId , pageNumber);
            var response = new GetCustomerOrdersDto();
            foreach(var order in orders)
            {
                string restaurantName = (await _restaurantRepository.GetRestaurantByIdAsync(order.RestaurantId , false)).Name;
                response.Orders.Add(new OrderHistoryItems(order.OrderId, restaurantName, order.TotalAmount, order.Status, order.CreatedAt, order.UpdatedAt));
            }

            return response;
        }

        /// <summary>
        /// Retrieve the details of a specific order.
        /// </summary>
        /// <param name="userId">The user Id of the requesting user.</param>
        /// <param name="orderId">The order id whose details are to be retrieved.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>An object with all the details of a specific order.</returns>
        public async Task<GetCustomerOrderDetailsDto> GetOrderDetailsAsync(long userId , long orderId, CancellationToken cancellationToken = default)
        {
            var order = await _orderRepository.GetOrderById(orderId , false) ?? throw new ValidationException(ErrorMessages.ORDER_DOES_NOT_EXIST);
            if (order.UserId != userId) throw new ValidationException(ErrorMessages.ORDER_DOES_NOT_EXIST);

            string restaurantName = (await _restaurantRepository.GetRestaurantByIdAsync(order.RestaurantId , false)).Name;

            var response = new GetCustomerOrderDetailsDto(orderId, restaurantName, order.Status, order.Instructions, order.TotalAmount, order.AddressLine1, order.City, order.State, order.Pincode, order.Country, order.CreatedAt, order.UpdatedAt, order.AddressLine2);
            var Items = await _orderRepository.GetOrderItems(orderId);

            foreach(var item in Items)
            {
                response.Items.Add(new OrderItem(item.Name , item.ItemPrice , item.Quantity));
            }

            return response;
        }

        /// <summary>
        /// Retirieves all the orders that belong to the restaurant.
        /// </summary>
        /// <param name="userId">The user id of the requesting user.</param>
        /// <param name="restaurantId">The restaurant id of the restaurant whose orders are to be fetched.</param>
        /// <param name="pageNumber">The page number</param>
        /// <param name="search">The string which is needed to be searched.</param>
        /// <param name="sortBy">The parameter to sort the orders.</param>
        /// <param name="filterBy">The parameter to filter the orders.</param>
        /// <param name="filterByCity">The parameter to filter the orders by the city.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>A list of all the orders belonging to the restaurant based on the searching, sorting and filteing parameters.</returns>
        public async Task<GetRestaurantOrdersDto> GetAllOrdersAsync(long userId , long restaurantId , int pageNumber, string search, Enums.SortBy sortBy, Enums.FilterBy filterBy, string filterByCity = "", CancellationToken cancellationToken = default)
        {
            if(await _ownerManagesRestaurantsRepository.GetOwnerIfExistsAsync(userId, restaurantId, false)  == null)
            {
                throw new UnauthorizedException();
            }

            var orders = await _orderRepository.GetOrdersByRestaurantId(restaurantId , pageNumber, search, sortBy, filterBy, filterByCity);


            var response = new GetRestaurantOrdersDto();
            foreach (var order in orders)
            {
                string restaurantName = (await _restaurantRepository.GetRestaurantByIdAsync(order.RestaurantId , false)).Name;
                int itemCount = (await _orderRepository.GetOrderItems(order.OrderId)).Count;
                response.Orders.Add(new RestaurantOrderHistoryItems(order.OrderId , restaurantName , order.TotalAmount , order.Status , order.CreatedAt , order.UpdatedAt, order.AddressLine1, order.City, itemCount));
            }

            if(sortBy == Enums.SortBy.ItemCount)
            {
                response.Orders.OrderBy(x => x.ItemCount);
            }
            else if(sortBy == Enums.SortBy.ItemCountDesc)
            {
                response.Orders.OrderByDescending(x => x.ItemCount);
            }

            return response;
        }

        /// <summary>
        /// Retrieves the order details of a specific order for the restaursnt id.
        /// </summary>
        /// <param name="userId">The user id of the requesting user.</param>
        /// <param name="restaurantId">The restaurant id to which to order was placed.</param>
        /// <param name="orderId">The order id of the order of which the details are to be retrieved.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>An object with all the details of the requested order.</returns>
        public async Task<GetRestaurantOrderDetailsDto> GetOrderDetailsAsync(long userId , long restaurantId , long orderId, CancellationToken cancellationToken = default)
        {
            if (await _ownerManagesRestaurantsRepository.GetOwnerIfExistsAsync(userId , restaurantId , false) == null)
            {
                throw new UnauthorizedException();
            }
            var order = await _orderRepository.GetOrderById(orderId , false) ?? throw new ValidationException(ErrorMessages.ORDER_DOES_NOT_EXIST);
            if (order.RestaurantId != restaurantId) throw new ValidationException(ErrorMessages.ORDER_DOES_NOT_EXIST);

            string restaurantName = (await _restaurantRepository.GetRestaurantByIdAsync(order.RestaurantId , false)).Name;

            var user = await _userRepository.GetUserByUserIdAsync(userId , false);

            var response = new GetRestaurantOrderDetailsDto(orderId, user.Name, user.PhoneNumber , restaurantName , order.Status , order.Instructions , order.TotalAmount , order.AddressLine1 , order.City , order.State , order.Pincode , order.Country , order.CreatedAt , order.UpdatedAt , order.AddressLine2);
            var Items = await _orderRepository.GetOrderItems(orderId);

            foreach (var item in Items)
            {
                response.Items.Add(new RestaurantOrderItem(item.Name , item.ItemPrice , item.Quantity));
            }

            return response;
        }


        /// <summary>
        /// Cancels the orders given that its current status is placed and refunds the amount to the users wallet.
        /// </summary>
        /// <param name="userId">The user id of the requesting user.</param>
        /// <param name="orderId">The order id of the order to be cancelled.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        public async Task CancelOrderAsync(long userId , long orderId, CancellationToken cancellationToken = default)
        {
            var order = await _orderRepository.GetOrderWithUpdateLockAsync(orderId);
            if (order.UserId != userId) throw new ValidationException(ErrorMessages.ORDER_DOES_NOT_EXIST);
            var user = await _userRepository.GetUserWithUpdateLockAsync(userId);
            bool cancelled = false;
            if(order.Status == Enums.OrderStatus.Placed)
            {
                order.Status = Enums.OrderStatus.Cancelled;
                user.WalletBalance += order.TotalAmount;
                cancelled = true;
            }
            await _unitOfWork.SaveChangesAsync();
            if (cancelled) return;
            else throw new ValidationException(ErrorMessages.CANNOT_CANCEL_ORDER_AFTER_IT_HAS_BEEN_ACCEPTED);
        }

        /// <summary>
        /// Changes the order status of the order with the given order given that the new status is a valid one and refunds the amount to the users wallet.
        /// </summary>
        /// <param name="restaurantId">The restaurant id to which the order was placed.</param>
        /// <param name="ownerId">The owner id of the requesting owner.</param>
        /// <param name="orderId">The orderd id of which the status is to be changed/</param>
        /// <param name="model">The new status of the order.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        public async Task ManageOrderAsync(long restaurantId, long ownerId , long orderId , OrderManagementRequestDto model, CancellationToken cancellationToken = default)
        {
            if (await _ownerManagesRestaurantsRepository.GetOwnerIfExistsAsync(ownerId , restaurantId , false) == null)
            {
                throw new UnauthorizedException();
            }
            var order = await _orderRepository.GetOrderWithUpdateLockAsync(orderId);
            if (order.RestaurantId != restaurantId) throw new ValidationException(ErrorMessages.ORDER_DOES_NOT_EXIST);
            var user = await _userRepository.GetUserWithUpdateLockAsync(order.UserId);
            var newStatus = model.ChangeStatusTo;
            var currentStatus = order.Status;

            if(newStatus <= currentStatus)
            {
                throw new ValidationException(ErrorMessages.INVALID_OPERATION);
            }
            else if(currentStatus == Enums.OrderStatus.Placed)
            {
                switch (newStatus)
                {
                    case Enums.OrderStatus.Accepted:
                        order.Status = Enums.OrderStatus.Accepted;
                        break;
                    case Enums.OrderStatus.Rejected:
                        order.Status = Enums.OrderStatus.Rejected;
                        break;
                    default:
                        throw new ValidationException(ErrorMessages.INVALID_OPERATION);
                }
            }
            else if(currentStatus == Enums.OrderStatus.Accepted && newStatus == Enums.OrderStatus.Dispatched)
            {
                order.Status = Enums.OrderStatus.Dispatched;
            }
            else if(currentStatus == Enums.OrderStatus.Dispatched && newStatus == Enums.OrderStatus.Delivered)
            {
                order.Status = Enums.OrderStatus.Delivered;
            }
            else
            {
                throw new ValidationException(ErrorMessages.INVALID_OPERATION);
            }
            if(order.Status == Enums.OrderStatus.Rejected)
            {
                user.WalletBalance += order.TotalAmount;
            }
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
