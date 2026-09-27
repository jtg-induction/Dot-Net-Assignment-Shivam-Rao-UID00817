using Dot_Net_Assignment_Shivam_Rao_UID00817.Constants;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Exceptions;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Services
{
    public class AdminRestaurantService : IAdminRestaurantService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IUserRepository _userRepository;

        private readonly IRestaurantRepository _restaurantRepository;

        private readonly IOwnerManagesRestaurantsRepository _ownerManagesRestaurantsRepository;

        public AdminRestaurantService(IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IRestaurantRepository restaurantRepository,
            IOwnerManagesRestaurantsRepository ownerManagesRestaurantsRepository)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _restaurantRepository = restaurantRepository;
            _ownerManagesRestaurantsRepository = ownerManagesRestaurantsRepository;
        }

        private bool CheckEmailFormat(string email)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(email, Constants.Regex.EMAIL_REGEX);
        }

        public async Task<List<string>> GetValidEmailsAsync(List<string> emails, List<EmailAndStatus> status)
        {
            var validEmails = new List<string>();

            foreach (string rawEmail in emails)
            {
                string email = rawEmail.Trim().ToLowerInvariant();

                if (string.Equals(email, ErrorMessages.ADMIN_EMAIL, StringComparison.OrdinalIgnoreCase))
                {
                    status.Add(new EmailAndStatus(email, ErrorMessages.CANNNOT_ONBOARD_ADMIN_TO_RESTAURANT));
                    continue;
                }
                if (!CheckEmailFormat(email))
                {
                    status.Add(new EmailAndStatus(email, ErrorMessages.INVALID_EMAIL_FORMAT));
                    continue;
                }
                validEmails.Add(email);
            }
            return validEmails;
        }


        public async Task<OwnerOnboardResponseDto> OnboardRestaurantAsync(RestaurantOnboardDto restaurant)
        {
            Restaurants existingRestaurant = await _restaurantRepository.GetRestaurantAsync(
                    restaurant.Name.Trim(),
                    false);

            if (existingRestaurant != null)
            {
                throw new ConflictException(
                    ErrorMessages.RESTAURANT_ALREADY_EXISTS);
            }

            var status = new List<EmailAndStatus>();

            List<string> validEmails = await GetValidEmailsAsync(restaurant.Emails, status);

            List<Users> validUsers = await _userRepository.GetUsersByEmails(validEmails);

            foreach (string email in validEmails)
            {
                bool userExists = validUsers.Any(x => string.Equals(x.Email.Trim(), email, StringComparison.OrdinalIgnoreCase));

                if (!userExists)
                {
                    status.Add(new EmailAndStatus(email, ErrorMessages.USER_DOES_NOT_EXIST));
                }
            }

            if (validUsers.Count == 0)
            {
                return new OwnerOnboardResponseDto
                {
                    EmailErrors = status
                };
            }

            Restaurants newRestaurant = new Restaurants(
                restaurant.Name,
                restaurant.AddressLine1,
                restaurant.City,
                restaurant.State,
                restaurant.Pincode,
                restaurant.Country,
                restaurant.AddressLine2
            );

            _restaurantRepository.Add(newRestaurant);

            await _unitOfWork.SaveChangesAsync();

            await AssignOwnerToRestaurantAsync(newRestaurant.Name, validUsers, status);

            return new OwnerOnboardResponseDto
            {
                EmailErrors = status
            };
        }


        public async Task AssignOwnerToRestaurantAsync(string name, List<Users> validUsers, List<EmailAndStatus> status)
        {
            Restaurants restaurant = await _restaurantRepository.GetRestaurantAsync(name, false);

            if (restaurant == null)
            {
                throw new ValidationException(ErrorMessages.RESTAURANT_DOES_NOT_EXIST);
            }

            if (!restaurant.IsActive)
            {
                throw new ValidationException(ErrorMessages.CANNOT_ASSIGN_OWNER_TO_RESTAURANT_THAT_IS_NOT_ACTIVE);
            }

            long restaurantId = restaurant.RestaurantId;

            var toOnboard = new List<Owner_Manages_Restaurants>();

            foreach (Users user in validUsers)
            {
                Owner_Manages_Restaurants existingOwner = await _ownerManagesRestaurantsRepository.GetOwnerIfExistsAsync(user.UserId, restaurantId, false);

                if (existingOwner == null)
                {
                    toOnboard.Add(new Owner_Manages_Restaurants(user.UserId, restaurantId));
                    user.Role = Enums.Roles.Owner;
                }
            }

            if (toOnboard.Count > 0)
            {
                _ownerManagesRestaurantsRepository.Add(toOnboard);
                await _unitOfWork.SaveChangesAsync();
            }
        }


        public async Task<OwnerOnboardResponseDto> AssignOwnerToRestaurantAsync(OwnerOnboardRequestDto model)
        {
            var status = new List<EmailAndStatus>();

            List<string> validEmails = await GetValidEmailsAsync(model.Emails, status);

            List<Users> validUsers = await _userRepository.GetUsersByEmails(validEmails);

            foreach (string email in validEmails)
            {
                bool userExists = validUsers.Any(x => string.Equals(x.Email.Trim(), email, StringComparison.OrdinalIgnoreCase));

                if (!userExists)
                {
                    status.Add(new EmailAndStatus(email, ErrorMessages.USER_DOES_NOT_EXIST));
                }
            }

            if (validUsers.Count == 0)
            {
                return new OwnerOnboardResponseDto
                {
                    EmailErrors = status
                };
            }

            await AssignOwnerToRestaurantAsync(
                model.Name,
                validUsers,
                status
            );

            return new OwnerOnboardResponseDto
            {
                EmailErrors = status
            };
        }

        public async Task DeactivateRestaurant(string name)
        {
            Restaurants restaurant = await _restaurantRepository.GetRestaurantAsync(name.Trim(), true) ?? throw new ValidationException(ErrorMessages.RESTAURANT_DOES_NOT_EXIST);
            if (restaurant.IsActive == false)
            {
                return;
            }
            restaurant.IsActive = false;
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task ActivateRestaurant(string name)
        {
            Restaurants restaurant = await _restaurantRepository.GetRestaurantAsync(name.Trim(), true) ?? throw new ValidationException(ErrorMessages.RESTAURANT_DOES_NOT_EXIST);
            if (restaurant.IsActive == true)
            {
                return;
            }
            restaurant.IsActive = true;
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
