using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Constants;
using ValidationException = Dot_Net_Assignment_Shivam_Rao_UID00817.Exceptions.ValidationException;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using System.Data.Entity;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Exceptions;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUserRepository _userRepository;
        private readonly IItemsRepository _itemRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOwnerManagesRestaurantsRepository _ownerManagesRestaurantsRepository;
        private readonly Restaurant_ManagementContext _db;

        public OrderService(IUserRepository userRepository, IItemsRepository itemsRepository, IAddressRepository addressRepository, IRestaurantRepository restaurantRepository, IOrderRepository orderRepository, IUnitOfWork unitOfWork, IOwnerManagesRestaurantsRepository ownerManagesRestaurantsRepository, Restaurant_ManagementContext db)
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

        public async Task<OrderResponseDto> PlaceOrderAsync(DbContextTransaction transaction,Users user, Addresses address, Restaurants restaurant, OrderRequestDto order)
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

        public async Task<OrderResponseDto> PlaceOrderAsync(long userId , OrderRequestDto order)
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


        public async Task<GetOrdersDto> GetAllOrdersAsync(long userId, int pageNumber)
        {
            var orders = await _orderRepository.GetOrdersByUserId(userId).OrderBy(x => x.OrderId).Skip((pageNumber - 1) * NumberConstants.PAGE_SIZE)
                                                                                        .Take(NumberConstants.PAGE_SIZE)
                                                                                        .ToListAsync();
            var response = new GetOrdersDto();
            foreach(var order in orders)
            {
                string restaurantName = (await _restaurantRepository.GetRestaurantByIdAsync(order.RestaurantId , false)).Name;
                response.Orders.Add(new OrderHistoryItems(order.OrderId, restaurantName, order.TotalAmount, order.Status, order.CreatedAt, order.UpdatedAt));
            }

            return response;
        }

        public async Task<GetOrderDetailsDto> GetOrderDetailsAsync(long userId , long orderId)
        {
            var order = await _orderRepository.GetOrderById(orderId , false) ?? throw new ValidationException(ErrorMessages.ORDER_DOES_NOT_EXIST);
            if (order.UserId != userId) throw new ValidationException(ErrorMessages.ORDER_DOES_NOT_EXIST);

            string restaurantName = (await _restaurantRepository.GetRestaurantByIdAsync(order.RestaurantId , false)).Name;

            var response = new GetOrderDetailsDto(orderId, restaurantName, order.Status, order.Instructions, order.TotalAmount, order.AddressLine1, order.City, order.State, order.Pincode, order.Country, order.CreatedAt, order.UpdatedAt, order.AddressLine2);
            var Items = await _orderRepository.GetOrderItems(orderId);

            foreach(var item in Items)
            {
                response.Items.Add(new OrderItem(item.Name , item.ItemPrice , item.Quantity));
            }

            return response;
        }

        public async Task<GetOrdersDto> GetAllOrdersAsync(long userId , long restaurantId , int pageNumber)
        {
            if(await _ownerManagesRestaurantsRepository.GetOwnerIfExistsAsync(userId, restaurantId, false)  == null)
            {
                throw new UnauthorizedException();
            }
            var orders = await _orderRepository.GetOrdersByRestaurantId(restaurantId).OrderBy(x => x.OrderId).Skip((pageNumber - 1) * NumberConstants.PAGE_SIZE)
                                                                                        .Take(NumberConstants.PAGE_SIZE)
                                                                                        .ToListAsync();
            var response = new GetOrdersDto();
            foreach (var order in orders)
            {
                string restaurantName = (await _restaurantRepository.GetRestaurantByIdAsync(order.RestaurantId , false)).Name;
                response.Orders.Add(new OrderHistoryItems(order.OrderId , restaurantName , order.TotalAmount , order.Status , order.CreatedAt , order.UpdatedAt));
            }

            return response;
        }

        public async Task<GetOrderDetailsDto> GetOrderDetailsAsync(long userId , long restaurantId , long orderId)
        {
            if (await _ownerManagesRestaurantsRepository.GetOwnerIfExistsAsync(userId , restaurantId , false) == null)
            {
                throw new UnauthorizedException();
            }
            var order = await _orderRepository.GetOrderById(orderId , false) ?? throw new ValidationException(ErrorMessages.ORDER_DOES_NOT_EXIST);
            if (order.RestaurantId != restaurantId) throw new ValidationException(ErrorMessages.ORDER_DOES_NOT_EXIST);

            string restaurantName = (await _restaurantRepository.GetRestaurantByIdAsync(order.RestaurantId , false)).Name;

            var response = new GetOrderDetailsDto(orderId , restaurantName , order.Status , order.Instructions , order.TotalAmount , order.AddressLine1 , order.City , order.State , order.Pincode , order.Country , order.CreatedAt , order.UpdatedAt , order.AddressLine2);
            var Items = await _orderRepository.GetOrderItems(orderId);

            foreach (var item in Items)
            {
                response.Items.Add(new OrderItem(item.Name , item.ItemPrice , item.Quantity));
            }

            return response;
        }
    }
}
