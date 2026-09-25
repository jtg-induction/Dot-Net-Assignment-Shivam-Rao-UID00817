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

        /// <summary>
        /// Checks the email against the regex.
        /// </summary>
        /// <param name="email">The email that is needed to be checked.</param>
        /// <returns>True if email is valid; otherwise, false.</returns>
        bool CheckEmailFormat(string email)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(email, Constants.Regex.EMAIL_REGEX);
        }

        /// <summary>
        /// Filters out all the invalid emails.
        /// </summary>
        /// <param name="Emails">List of emails that are needed to be checked.</param>
        /// <param name="status">List of emails and their status (valid / invalid)</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>List of all the valid emails.</returns>
        public async Task<List<string>> GetValidEmailsAsync(List<string> emails , List<EmailAndStatus> status, CancellationToken cancellationToken = default)
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


        /// <summary>
        /// Adds a restaurant into the restaurants table. Needs atleast one valid email in order to create a new restaurant.
        /// </summary>
        /// <param name="restaurant">Details of the restaurant to be added.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>A list of all the emails and thir status, if they have been added or not, and if not added then the reason.</returns>
        public async Task<OwnerOnboardResponseDto> OnboardRestaurantAsync(RestaurantOnboardDto restaurant, CancellationToken cancellationToken = default)
        {
            if (!(await _restaurantRepository.GetRestaurantAsync(restaurant.Name.Trim(), false) is null))
            {
                throw new ConflictException(
                    ErrorMessages.RESTAURANT_ALREADY_EXISTS);
            }

            var status = new List<EmailAndStatus>();

            List<string> validEmails = await GetValidEmailsAsync(restaurant.Emails, status);


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

        /// <summary>
        /// Onboard Customers / Owners to the current restaurant.
        /// </summary>
        /// <param name="Name">The name of the restaurant in which users are to be added.</param>
        /// <param name="ValidUsers">List of all the Valid Users after it has been filtered.</param>
        /// <param name="Status">List of emails requested to be added and their current status.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        public async Task AssignOwnerToRestaurantAsync(string Name, List<Users> ValidUsers, List<EmailAndStatus> Status, CancellationToken cancellationToken = default)
        {
            Restaurants restaurant = await _restaurantRepository.GetRestaurantAsync(Name, false) ?? throw new ValidationException(ErrorMessages.RESTAURANT_DOES_NOT_EXIST);
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

        /// <summary>
        /// Onboard Users / Customers to the restaurant.
        /// </summary>
        /// <param name="model">Contains name of restaurant and a list of emails.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        /// <returns>List of emails and their status if they have been added or not.</returns>
        public async Task<OwnerOnboardResponseDto> AssignOwnerToRestaurantAsync(OwnerOnboardRequestDto model, CancellationToken cancellationToken = default)
        {
            var status = new List<EmailAndStatus>();

            List<string> validEmails = await GetValidEmailsAsync(model.Emails, status);


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

        /// <summary>
        /// Deactivates the restaurant.
        /// </summary>
        /// <param name="name">Name of the restaurant to be deactivated</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        public async Task DeactivateRestaurant(string name, CancellationToken cancellationToken = default)
        {
            Restaurants restaurant = await _restaurantRepository.GetRestaurantAsync(name.Trim(), true) ?? throw new ValidationException(ErrorMessages.RESTAURANT_DOES_NOT_EXIST);
            if (!restaurant.IsActive)
            {
                return;
            }
            restaurant.IsActive = false;
            await _unitOfWork.SaveChangesAsync();
        }


        /// <summary>
        /// Activates the restaurant.
        /// </summary>
        /// <param name="name">Name of the restaurant to be activated.</param>
        /// <param name="cancellationToken">Token used to cancel the operation.</param>
        public async Task ActivateRestaurant(string name, CancellationToken cancellationToken = default)
        {
            Restaurants restaurant = await _restaurantRepository.GetRestaurantAsync(name.Trim(), true) ?? throw new ValidationException(ErrorMessages.RESTAURANT_DOES_NOT_EXIST);
            if (restaurant.IsActive)
            {
                return;
            }
            restaurant.IsActive = true;
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
