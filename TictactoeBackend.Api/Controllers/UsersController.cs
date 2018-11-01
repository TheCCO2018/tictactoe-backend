using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using TictactoeBackend.Api.Models;
using TictactoeBackend.Api.Results;
using TictactoeBackend.DataAccess.Abstractions;
using TictactoeBackend.DataAccess.Concretes;
using TictactoeBackend.Entity.Abstractions;
using TictactoeBackend.Entity.Concretes;
using WebGrease.Css.Extensions;

namespace TictactoeBackend.Api.Controllers
{   
    /// <summary>
    /// User Controller for user actions
    /// </summary>
    [System.Web.Http.Authorize]
    [System.Web.Http.RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        private IUserRepository _iuserRepository = new UserRepository();
        private IUserStatRepository _iuserStatRepository = new UserStatRepository();

        // GET: api/Users
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("")]
        public IHttpActionResult Get()
        {
            IList<User> users = _iuserRepository.GetAll();
            var content = new JsonContent<User>
            {
                Result = users.Count() != 0 ? "1":"0",
                Data = users
            };
            return new StandartResult<User>(content,Request);
        }

        // GET: api/Users/5
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("{id}")]
        public IHttpActionResult Get(string id)
        {   
            IList<User> users = new List<User>();
            // Get UserStats and set up rankings
            IList<UserStat> stats = _iuserStatRepository.GetOrderedList(false);
            ListExtensions.ForEach(stats, x => x.Rank = stats.IndexOf(x) + 1);

            users.Add(_iuserRepository.GetSingle(x=>x.AspUserId == id, x=>x.Stats));
            if (users.First() != null && !users.First().Username.StartsWith("Guest"))
            {   
                ListExtensions.ForEach(users, x => x.Stats = stats.First(s => s.UserId.Equals(x.UserId)));
            }
            var content = new JsonContent<User>
            {
                Result = users.First() != null ? "1" : "0",
                Data = users
            };

            return new StandartResult<User>(content, Request);
        }

        // POST: api/Users
        //public void Post([FromBody]string value)
        //{
        //}

        // POST: api/Users/5/win
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("{id}/{state}")]
        public IHttpActionResult UpdateTotalWins(string id,string state)
        {
            if (id == "" || state.IsNullOrWhiteSpace())
            {
                return BadRequest("Invalid parameters.");
            }
            var user = _iuserRepository.GetSingle(x => x.AspUserId == id, x => x.Stats);
            JsonContent<User> content;

            // if it's a real id
            if (user != null && state.Length > 3 )
            {
                // Check for post info depent game result
                if (state.ToLower().Equals("win") || state.ToLower().Equals("won") || state.ToLower().Equals("winner"))
                {
                    user.Stats.TotalWins++;
                }

                if (state.ToLower().Equals("lose") || state.ToLower().Equals("lost") || state.ToLower().Equals("loser"))
                {
                    user.Stats.TotalLoses++;
                }

                user.Stats.EntityState = EntityState.Modified;
                _iuserStatRepository.Update(user.Stats);

                content = new JsonContent<User>
                {
                    Result = "1",
                    Data = null
                };

            }
            else
            {
                // If it's a bad request
                content = new JsonContent<User>
                {
                    Result = "0",
                    Data = null
                };
            }
            return new StandartResult<User>(content, Request);
        }

        // GET: api/users/leaderboard
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("LeaderBoard")]
        public IHttpActionResult GetLeaderBoard()
        {
            IList<User> users = _iuserRepository.GetAll();

            // Get UserStats and set up rankings
            IList<UserStat> stats = _iuserStatRepository.GetOrderedList(false);
            ListExtensions.ForEach(stats, x => x.Rank = stats.IndexOf(x) + 1);
            ListExtensions.ForEach(stats, x => x.User = users.Where(u => u.UserId.Equals(x.UserId)).FirstOrDefault());
            if (users.Count() != 0)
            {
                users.Clear();
                //stats.ToList().RemoveAll(x => x.User.Username.StartsWith("Guest"));
                ListExtensions.ForEach(stats, x => users.Add(x.User));
                ListExtensions.ForEach(users, x => x.Stats = stats.First(s => s.UserId.Equals(x.UserId)));
            }
            // Set content of result data
            var content = new JsonContent<User>
            {
                Result = users.Count() != 0 ? "1" : "0",
                Data = users.Take(users.Count() > 15 ? 15 : users.Count()).ToList()
            };

            return new StandartResult<User>(content, Request);
        }

        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("FullBoard")]
        public IHttpActionResult GetAllUsersWithRank()
        {
            IList<User> users = _iuserRepository.GetAll();

            // Get UserStats and set up rankings
            IList<UserStat> stats = _iuserStatRepository.GetOrderedList(false);
            ListExtensions.ForEach(stats, x => x.Rank = stats.IndexOf(x) + 1);
            ListExtensions.ForEach(stats, x => x.User = users.Where(u=>u.UserId.Equals(x.UserId)).FirstOrDefault());
            if (users.Count() != 0)
            {
                users.Clear();
                ListExtensions.ForEach(stats, x => users.Add(x.User));
                ListExtensions.ForEach(users, x => x.Stats = stats.First(s => s.UserId.Equals(x.UserId)));
            }
            // Set content of result data
            var content = new JsonContent<User>
            {
                Result = users.Count() != 0 ? "1" : "0",
                Data = users.ToList()
            };

            return new StandartResult<User>(content, Request);
        }

        // DELETE: api/Users/5
        [System.Web.Http.HttpDelete]
        [System.Web.Http.Route("{id}")]
        public IHttpActionResult Delete(string id)
        {
            var user = _iuserRepository.GetSingle(x => x.AspUserId == id, x => x.Stats);
            JsonContent<User> content;

            // if it's a real id
            if (user != null)
            {
                ApplicationUserManager.Delete(user.AspUserId, Request.GetOwinContext());

                user.Stats.EntityState = EntityState.Deleted;
                _iuserStatRepository.Remove(user.Stats);
                user.EntityState = EntityState.Deleted;
                _iuserRepository.Remove(user);

                content = new JsonContent<User>
                {
                    Result = "1",
                    Data = null
                };

            }
            else
            {
                // If it's a bad request
                content = new JsonContent<User>
                {
                    Result = "0",
                    Data = null
                };
            }

            return new StandartResult<User>(content, Request);
        }


    }
}
