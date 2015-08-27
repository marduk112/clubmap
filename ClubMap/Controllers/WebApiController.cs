using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;
using ClubMap.DbModels;
using ClubMap.Logic;
using ClubMap.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using PushSharp;
using PushSharp.Android;

namespace ClubMap.Controllers
{
    [Authorize]
    public class WebApiController : ApiController
    {
        // GET /Api/GetDatabaseZipArchiveFile
        /// <summary>
        /// Gets the database zip archive file.
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage GetDatabaseZipArchiveFile()
        {
            var zipFile = System.Web.Hosting.HostingEnvironment.MapPath(@"~\App_Data\database.zip");
            if (zipFile != null) File.Delete(zipFile);
            var zipPath = System.Web.Hosting.HostingEnvironment.MapPath(@"~\App_Data\sqlite\");
            if (!File.Exists(zipPath + "ClubMap.sqlite"))
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            ZipFile.CreateFromDirectory(zipPath, zipFile);
            var archiveFileByteArray = File.ReadAllBytes(zipFile);
            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(archiveFileByteArray)
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/x-zip-compressed");
            return response;
        }

        // GET /Api/GetUsersWhoseCheckedLocal?localCode=KKK1
        /// <summary>
        /// Gets the users whose checked local.
        /// </summary>
        /// <param name="localCode">The local code.</param>
        /// <returns></returns>
        public IEnumerable<UserViewModel> GetUsersWhoseCheckedLocal(string localCode)
        {
            return new WebApiLogic().GetUsersWhoseCheckedLocal(localCode);
        }

        // POST /Api/CheckLocal
        /// <summary>
        /// Checks the local.
        /// </summary>
        /// <param name="localCode">The local code.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IHttpActionResult> CheckLocal(string localCode)
        {
            try
            {
                var result = await new WebApiLogic().CheckLocalAsync(localCode, User.Identity.GetUserId());
                if (result)
                    return Ok();
                return BadRequest();
            }
            catch
            {
                return InternalServerError();
            }
        }

        // POST /Api/LikeUser
        /// <summary>
        /// Likes the user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IHttpActionResult> LikeUser(string userId)
        {
            try
            {
                var result = await new WebApiLogic().LikeUserAsync(userId, User.Identity.GetUserId());
                if (result)
                    return Ok();
                return BadRequest();
            }
            catch
            {
                return InternalServerError();
            }
        }

        // GET /Api/GetUsersWhoseLikesYouAndYouLikeThem
        /// <summary>
        /// Gets the users whose likes you and you like them.
        /// </summary>
        /// <returns></returns>
        public MutualLikeViewModel GetUsersWhoseLikesYouAndYouLikeThem()
        {
            return new WebApiLogic().GetUsersWhoseLikesYouAndYouLikeThem(User.Identity.GetUserId());
        }

        // GET /Api/GetConversationWith?userId=dfgfdgfdg
        /// <summary>
        /// Gets the conversation with.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public IEnumerable<Message> GetConversationWith(string userId)
        {
            return new WebApiLogic().GetConversationWith(userId, User.Identity.GetUserId());
        }

        // POST /Api/SendMessage
        /// <summary>
        /// Sends the message. (set receiverId and News)
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IHttpActionResult> SendMessage(Message message)
        {
            try
            {
                message.SenderId = User.Identity.GetUserId();
                var result = await new WebApiLogic().SaveMessage(message);
                if (!result) return BadRequest();
                string registrationId;
                using (var db = new ApplicationDbContext())
                {
                    var user = db.Users.Find(message.ReceiverId);
                    registrationId = user.DeviceId;
                }
                var push = new PushBroker();
                //---------------------------
                // ANDROID GCM NOTIFICATIONS
                //---------------------------
                //Configure and start Android GCM
                //IMPORTANT: The API KEY comes from your Google APIs Console App, under the API Access section, 
                //  by choosing 'Create new Server key...'
                //  You must ensure the 'Google Cloud Messaging for Android' service is enabled in your APIs Console
                push.RegisterGcmService(new GcmPushChannelSettings("AIzaSyDOOAhH2o5IP00lL9aJt5NbYfRNfGWDJ94"));
                //Fluent construction of an Android GCM Notification
                //IMPORTANT: For Android you MUST use your own RegistrationId here that gets generated within your Android app itself!
                push.QueueNotification(new GcmNotification().ForDeviceRegistrationId(registrationId)
                    .WithJson(JsonConvert.SerializeObject(message)));
                //Stop and wait for the queues to drains
                push.StopAllServices();
                return Ok();
            }
            catch
            {
                return InternalServerError();
            }
        }
    }
}