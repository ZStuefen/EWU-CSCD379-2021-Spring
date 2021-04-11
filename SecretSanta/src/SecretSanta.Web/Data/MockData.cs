using System;
using System.Collections.Generic;
using SecretSanta.Web.ViewModels;

namespace SecretSanta.Web.Data
{
    public static class MockData
    {        
        public static List<UserViewModel> Users = new List<UserViewModel>{
            new UserViewModel {Id = 0, FirstName = "First", LastName = "Last"}
        };
        public static List<GroupViewModel> Groups = new List<GroupViewModel>{
            new GroupViewModel {Id = 0, GroupName = "FirstGroup"}
        };
        public static List<GiftViewModel> Gifts = new List<GiftViewModel>{
            new GiftViewModel {Id = 0, Title = "FirstGift", Description = "The First", URL = "website.com", Priority = 1, UserId = 0}
        };
    }
}