using Dot_Net_Assignment_Shivam_Rao_UID00817.Constants;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Exceptions;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Models.DTOs;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Repositories.Interfaces;
using Dot_Net_Assignment_Shivam_Rao_UID00817.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dot_Net_Assignment_Shivam_Rao_UID00817.Services
{
    public class AdminRestaurantService : IAdminRestaurantService
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IUserRepository _userRepository;

        private readonly IRestaurantRepository _restaurantRepository;

        private readonly IOwnerManagesRestaurantsRepository _ownerManagesRestaurantsRepository;

        public AdminRestaurantService(IUserRepository userRepository ,
            IUnitOfWork unitOfWork ,
            IRestaurantRepository restaurantRepository ,
            IOwnerManagesRestaurantsRepository ownerManagesRestaurantsRepository)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _restaurantRepository = restaurantRepository;
            _ownerManagesRestaurantsRepository = ownerManagesRestaurantsRepository;
        }

        bool CheckEmailFormat(string email)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(email.Trim() , Constants.Regex.EMAIL_REGEX);
        }

        public async Task<List<string>> GetValidEmailsAsync(List<string> Emails , List<EmailAndStatus> status)
        {
            List<string> validEmails = new List<string>();

            foreach (string email in Emails)
            {
                if (CheckEmailFormat(email))
                {
                    validEmails.Add(email.Trim().ToLower());
                }
                else
                {
                    status.Add(new EmailAndStatus(email.Trim().ToLower() , false , ErrorMessages.INVALID_EMAIL_FORMAT));
                }
            }
            return validEmails;
        }

        public async Task<OwnerOnboardResponseDto> OnboardRestaurantAsync(RestaurantOnboardDto restaurant)
        {
            if (!(await _restaurantRepository.GetRestaurantAsync(restaurant.Name.Trim() , false) is null))
            {
                throw new ConflictException(ErrorMessages.RESTAURANT_ALREADY_EXISTS);
            }

            List<EmailAndStatus> Status = new List<EmailAndStatus>();


            List<string> ValidEmails = await GetValidEmailsAsync(restaurant.Emails , Status);

            List<Users> ValidUsers = await _userRepository.GetUsersByEmails(ValidEmails);

            if (ValidUsers.Count == 0)
            {
                throw new ValidationException(ErrorMessages.NO_VALID_EMAILS);
            }

            Restaurants NewRestaurant = new Restaurants(
                restaurant.Name ,
                restaurant.AddressLine1 ,
                restaurant.City ,
                restaurant.State ,
                restaurant.Pincode ,
                restaurant.Country ,
                restaurant.AddressLine2
            );

            _restaurantRepository.Add(NewRestaurant);

            await _unitOfWork.SaveChangesAsync();

            await AssignOwnerToRestaurantAsync(NewRestaurant.Name , ValidUsers , Status);

            foreach (string email in ValidEmails)
            {
                if (!Status.Exists(x => x.Email == email))
                {
                    Status.Add(new EmailAndStatus(email , false , ErrorMessages.USER_DOES_NOT_EXIST));
                }
            }

            return new OwnerOnboardResponseDto { EmailStatus = Status };
        }

        public async Task AssignOwnerToRestaurantAsync(string Name , List<Users> ValidUsers , List<EmailAndStatus> Status)
        {
            Restaurants restaurant = await _restaurantRepository.GetRestaurantAsync(Name , false) ?? throw new ValidationException(ErrorMessages.RESTAURANT_DOES_NOT_EXIST);
            if (restaurant.IsActive == false)
            {
                throw new ValidationException(ErrorMessages.CANNOT_ASSIGN_OWNER_TO_RESTAURANT_THAT_IS_NOT_ACTIVE);
            }

            long RestaurantId = restaurant.RestaurantId;

            List<Owner_Manages_Restaurants> ToOnboard = new List<Owner_Manages_Restaurants>();

            foreach (Users user in ValidUsers)
            {
                ToOnboard.Add(new Owner_Manages_Restaurants(user.UserId , RestaurantId));
                Status.Add(new EmailAndStatus(user.Email , true, null));
                user.Role = Enums.Roles.Owner;
            }
            _ownerManagesRestaurantsRepository.Add(ToOnboard);

            await _unitOfWork.SaveChangesAsync();
        }


        public async Task<OwnerOnboardResponseDto> AssignOwnerToRestaurantAsync(OwnerOnboardRequestDto model)
        {
            List<EmailAndStatus> Status = new List<EmailAndStatus>();


            List<string> ValidEmails = await GetValidEmailsAsync(model.Emails , Status);

            List<Users> ValidUsers = await _userRepository.GetUsersByEmails(ValidEmails);

            if (ValidUsers.Count == 0)
            {
                throw new ValidationException(ErrorMessages.NO_VALID_EMAILS);
            }

            await AssignOwnerToRestaurantAsync(model.Name , ValidUsers , Status);

            foreach (string email in ValidEmails)
            {
                if (!Status.Exists(x => x.Email == email))
                {
                    Status.Add(new EmailAndStatus(email , false , ErrorMessages.USER_DOES_NOT_EXIST));
                }
            }

            return new OwnerOnboardResponseDto { EmailStatus = Status };
        }

        public async Task DeactivateRestaurant(string name)
        {
            Restaurants restaurant = await _restaurantRepository.GetRestaurantAsync(name.Trim() , true) ?? throw new ValidationException(ErrorMessages.RESTAURANT_DOES_NOT_EXIST);
            if (restaurant.IsActive == false)
            {
                return;
            }
            restaurant.IsActive = false;
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task ActivateRestaurant(string name)
        {
            Restaurants restaurant = await _restaurantRepository.GetRestaurantAsync(name.Trim() , true) ?? throw new ValidationException(ErrorMessages.RESTAURANT_DOES_NOT_EXIST);
            if (restaurant.IsActive == true)
            {
                return;
            }
            restaurant.IsActive = true;
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
