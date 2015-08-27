using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;
using ClubMap.DbModels;
using ClubMap.Models;

namespace ClubMap.Logic
{
    public class WebApiLogic : IDisposable
    {
        public IEnumerable<UserViewModel> GetUsersWhoseCheckedLocal(string localCode)
        {
            if (string.IsNullOrEmpty(localCode)) return null;
            var users = _db.Checks.Where(x => x.HotelCode.Equals(localCode) || x.ClubCode.Equals(localCode) 
                || x.RestaurantCode.Equals(localCode)
                || x.PubCode.Equals(localCode) || x.CafeCode.Equals(localCode))
                .Select(x => x.ApplicationUser);
            return from user in users
                   select new UserViewModel
                   {
                       Icon = user.Icon,
                       UserName = user.UserName,
                       UserId = user.Id,
                   };
        }
        public async Task<bool> LikeUserAsync(string likedUserId, string myUserId)
        {
            var user = _db.Users.Find(likedUserId);
            if (user == null)
                return false;
            _db.Likes.Add(new Like
            {
                ApplicationUserId = myUserId,
                LikedUserId = likedUserId,
            });
            await _db.SaveChangesAsync();
            return true;
        }
        public async Task<bool> CheckLocalAsync(string localCode, string myUserId)
        {
            if (string.IsNullOrEmpty(localCode)) return false;
            var check = new Check
            {
                ApplicationUserId = myUserId,
            };
            switch (localCode[0])
            {
                case 'K':
                    check.ClubCode = localCode;
                    break;
                case 'P':
                    check.PubCode = localCode;
                    break;
                case 'R':
                    check.RestaurantCode = localCode;
                    break;
                case 'A':
                    check.CafeCode = localCode;
                    break;
                case 'H':
                    check.HotelCode = localCode;
                    break;
            }
            _db.Checks.Add(check);
            await _db.SaveChangesAsync();
            return true;
        }

        public MutualLikeViewModel GetUsersWhoseLikesYouAndYouLikeThem(string myUserId)
        {
            var results = new MutualLikeViewModel();
            var likedUserIds = _db.Likes.Where(x => x.ApplicationUserId.Equals(myUserId)).Select(x => x.LikedUserId);
            foreach (var likedUserId in likedUserIds)
            {
                var like = _db.Likes.FirstOrDefault(x => x.ApplicationUserId.Equals(likedUserId) && x.LikedUserId.Equals(myUserId));
                if (like != null)
                {
                    results.UsersDictionary.Add(like.ApplicationUserId, like.ApplicationUser.UserName);
                }
            }
            return results;
        }
        public IEnumerable<Message> GetConversationWith(string userId, string myUserId)
        {
            var results = new List<Message>();
            results.AddRange(_db.Messages.Where(x => (x.SenderId.Equals(userId) && x.ReceiverId.Equals(myUserId))
                || (x.SenderId.Equals(myUserId) && x.ReceiverId.Equals(userId))));
            return results;
        }
        public async Task<bool> SaveMessage(Message message)
        {
            if (!message.IsValid()) return false;
            message.Created = DateTime.Now;
            message.Id = Guid.NewGuid().ToString();
            _db.Messages.Add(message);
            await _db.SaveChangesAsync();
            return true;
        }

        public void Dispose()
        {
            // Dispose of unmanaged resources.
            Dispose(true);
            // Suppress finalization.
            GC.SuppressFinalize(this);
        }
        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            if (disposing)
            {
                // Free any other managed objects here.
            }
            // Free any unmanaged objects here.
            _db.Dispose();
            _disposed = true;
        }

        private readonly ApplicationDbContext _db = new ApplicationDbContext();
        private bool _disposed;
    }

    public class LikesComparer : IEqualityComparer<Like>
    {
        public bool Equals(Like x, Like y)
        {
            return x.Id.Equals(y.Id);
        }

        public int GetHashCode(Like obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}