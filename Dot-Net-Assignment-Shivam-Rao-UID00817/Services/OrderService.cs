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
using Microsoft.Owin.Security;

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

        private readonly Restaurant_ManagementContext _db;

        public OrderService(IUserRepository userRepository, IItemsRepository itemsRepository, IAddressRepository addressRepository, IRestaurantRepository restaurantRepository, IOrderRepository orderRepository, IUnitOfWork unitOfWork, Restaurant_ManagementContext db)
        {
            _addressRepository = addressRepository;
            _db = db;
            _itemRepository = itemsRepository;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _restaurantRepository = restaurantRepository;
            _orderRepository = orderRepository;
        }

        public async Task<OrderResponseDto> PlaceOrderAsync(long userId , OrderRequestDto order)
        {
            using (var transaction = _db.Database.BeginTransaction(System.Data.IsolationLevel.ReadCommitted))
            {
                try
                {
                    var restaurant = await _restaurantRepository.GetRestaurantByIdAsync(order.RestaurantId , false) ?? throw new ValidationException(ErrorMessages.RESTAURANT_DOESNOT_EXIST);

                    var address = await _addressRepository.GetAddressAsync(order.AddressId , false) ?? throw new ValidationException(ErrorMessages.ADDRESS_DOESNOT_EXIST);

                    if (order.Items is null || order.Items.Count == 0) throw new ValidationException(ErrorMessages.NO_ITEMS_FOUND);

                    var requestedItems = order.Items.OrderBy(x => x.ItemId).ToList();

                    var itemIds = requestedItems.Select(x => x.ItemId).ToList();

                    var items = await _itemRepository.GetItemsAsync(itemIds , order.RestaurantId);

                    if (items.Count != requestedItems.Count) throw new ValidationException(ErrorMessages.INVALID_ITEMS);

                    var itemDictionary = items.ToDictionary(x => x.ItemId);

                    decimal total = 0;

                    foreach (var requestedItem in requestedItems)
                    {
                        var item = itemDictionary[requestedItem.ItemId];

                        if (requestedItem.Quantity <= 0)
                        {
                            throw new ValidationException(ErrorMessages.INVALID_ITEM_QUANTITY);
                        }

                        if (item.AvailableQuantity < requestedItem.Quantity)
                        {
                            throw new ValidationException($"Insufficient quantity for item {item.Name}");
                        }

                        total += item.Price * requestedItem.Quantity;
                    }

                    var user = await _userRepository.GetUserByUserIdAsync(userId , true) ?? throw new ValidationException(ErrorMessages.USER_DOESNOT_EXIST);

                    foreach (var requestedItem in requestedItems)
                    {
                        var affectedRows = await _itemRepository.DecreaseQuantityIfAvailableAsync(requestedItem.ItemId , order.RestaurantId , requestedItem.Quantity);

                        if (affectedRows != 1)
                        {
                            throw new ValidationException(ErrorMessages.REQUIRED_QUANTITY_NOT_AVAILABLE);
                        }
                    }

                    var walletUpdated = await _userRepository.DeductWalletBalanceIfSufficientAsync(userId , total);

                    if (walletUpdated != 1) throw new ValidationException(ErrorMessages.INSUFFICIENT_WALLET_BALANCE);

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
                catch
                {
                    transaction.Rollback();
                }
            }
            return null;
        }
    }
}
