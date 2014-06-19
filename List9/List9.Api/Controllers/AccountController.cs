using Data;
using List9.Api.Filters;
using List9.Api.Models;
using List9.Api.Providers;
using List9.Api.Results;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Model.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.OData.Query;
using System.Net.Http;
using System.Web.Http.ModelBinding;


namespace List9.Api.Controllers
{
    [Authorize]
    [RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        private const string LocalLoginProvider = "Local";
        private UnitOfWork Uow;

        public AccountController()
            : this(Startup.UserManagerFactory(), Startup.OAuthOptions.AccessTokenFormat, UnitOfWork.Instantiate())
        {
        }

        public AccountController(UserManager<List9User> userManager, ISecureDataFormat<AuthenticationTicket> accessTokenFormat, UnitOfWork uow)
        {
            UserManager = userManager;
            AccessTokenFormat = accessTokenFormat;
            Uow = uow;

            //List9User liz = new List9User("greeter", Uow.Greeters.Find(8));


            //var se = userManager.Create(liz, "greeter1");

            //var a = Uow.Greeters.Find(8);
        }

        public UserManager<List9User> UserManager { get; private set; }
        public ISecureDataFormat<AuthenticationTicket> AccessTokenFormat { get; private set; }

        #region Built In

        // GET api/Account/UserInfo
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("UserInfo")]
        public UserInfoViewModel GetUserInfo()
        {
            ExternalLoginData externalLogin = ExternalLoginData.FromIdentity(User.Identity as ClaimsIdentity);

            return new UserInfoViewModel
            {
                UserName = User.Identity.GetUserName(),
                HasRegistered = externalLogin == null,
                LoginProvider = externalLogin != null ? externalLogin.LoginProvider : null
            };
        }

        // POST api/Account/Logout
        [Route("Logout")]
        public IHttpActionResult Logout()
        {
            Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return Ok();
        }

        // POST api/Account/ChangePassword
        [Route("ChangePassword")]
        public async Task<IHttpActionResult> ChangePassword(ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword,
                model.NewPassword);
            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();
        }

        // POST api/Account/SetPassword
        [Route("SetPassword")]
        public async Task<IHttpActionResult> SetPassword(SetPasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();
        }

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(RegisterBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            List9User user = new List9User
            {
                UserName = model.UserName
            };

            IdentityResult result = await UserManager.CreateAsync(user, model.Password);
            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                UserManager.Dispose();
            }

            base.Dispose(disposing);
        }

        [HttpGet, Queryable]
        public IHttpActionResult Get(ODataQueryOptions<List9User> options)
        {
            return Ok(UserManager.Users);
        }

        /// <summary>
        /// Api endpoint for getting the current user.
        /// </summary>
        /// <returns>List9User: the current user</returns>
        [Route("CurrentUser")]
        public IHttpActionResult GetCurrentUser()
        {
            var userId = User.Identity.GetUserId();
            
            var result = Uow.Context.Set<List9User>().Include("Projects.Tasks.TaskCategories").Single(u => u.Id == userId);

            return Ok(result);
        }

        [Route("GetAllUsers")]
        public IHttpActionResult GetAllUsers()
        {
            var users = UserManager.Users.ToList();
           

            var result = new List<object>();

            foreach (var user in users)
            {
                result.Add(new
                {
                    Id = user.Id,
                    Name = user.Name,
                    Type = user.Type,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    UserName = user.UserName,
                    Roles = UserManager.GetRoles(user.Id),
                    LockedOut = user.LockoutEnabled,
                    EmailConfirmed = user.EmailConfirmed,
                    PhoneNumberConfirmed = user.PhoneNumberConfirmed
                });
            }

            return Ok(result);
        }

       
        [Route("PostNewUser"),
        HttpPost,
        ValidateModel]
        public IHttpActionResult PostNewUser(UserCreationModel newUser)
        {
            if (!newUser.Password.Equals(newUser.Repeat, StringComparison.InvariantCulture))
                return BadRequest("Passwords do not match");

            var user = new List9User
            {
                Name = newUser.Name,
                UserName = newUser.UserName,
                Email = newUser.Email,
                PhoneNumber = newUser.PhoneNumber,
                Type = newUser.Type,
              
            };

           

            var result = UserManager.Create(user, newUser.Password);
            if (result.Succeeded)
            {
                foreach (var role in newUser.Roles)
                {
                    UserManager.AddToRole(user.Id, role);
                }
                if (!string.IsNullOrWhiteSpace(newUser.Email))
                {
                    //Validate Email
                }
                if (!string.IsNullOrWhiteSpace(newUser.PhoneNumber))
                {
                    //Validate Email
                }
                

                return Ok(new
                {
                    Name = user.Name,
                    Type = user.Type,                   
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    UserName = user.UserName,
                    Roles = newUser.Roles,
                    LockedOut = user.LockoutEnabled,
                    EmailConfirmed = user.EmailConfirmed,
                    PhoneNumberConfirmed = user.PhoneNumberConfirmed
                });
            }
            else
            {
                return BadRequest(String.Join(", ", result.Errors));
            }
        }

        [Route("PutUser"),
        HttpPut,
        ValidateModel]
        public IHttpActionResult PutUser(UserCreationModel newUser)
        {
            if (!newUser.Password.Equals(newUser.Repeat, StringComparison.InvariantCulture))
                return BadRequest("Passwords do not match");

            var user = UserManager.FindByName(newUser.UserName);
            bool emailChanged = false, phoneChanged = false;
            if (!user.Email.Equals(newUser.Email, StringComparison.InvariantCultureIgnoreCase))
            {
                emailChanged = true;
                user.EmailConfirmed = false;
            }
            if (!user.PhoneNumber.Equals(newUser.PhoneNumber, StringComparison.InvariantCultureIgnoreCase))
            {
                phoneChanged = true;
                user.PhoneNumberConfirmed = false;
            }
            user.Name = newUser.Name;
            user.UserName = newUser.UserName;
            user.Email = newUser.Email;
            user.PhoneNumber = newUser.PhoneNumber;
            user.Type = newUser.Type;

           
            var result = UserManager.Update(user);
            if (result.Succeeded)
            {
                var roles = UserManager.GetRoles(user.Id);
                foreach (var role in roles)
                {
                    if (!newUser.Roles.Contains(role))
                        UserManager.RemoveFromRole(user.Id, role);
                }

                foreach (var role in newUser.Roles)
                {
                    if (!roles.Contains(role))
                        UserManager.AddToRole(user.Id, role);
                }

                if (emailChanged && !string.IsNullOrWhiteSpace(user.Email))
                {
                    //TODO: validate Email
                }
                if (phoneChanged && !string.IsNullOrWhiteSpace(user.PhoneNumber))
                {
                    //TODO: validate Email
                }
                return Ok(new
                {
                    Name = user.Name,
                    Type = user.Type,                    
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    UserName = user.UserName,
                    Roles = newUser.Roles,
                    LockedOut = user.LockoutEnabled,
                    EmailConfirmed = user.EmailConfirmed,
                    PhoneNumberConfirmed = user.PhoneNumberConfirmed
                });
            }
            else
            {
                return BadRequest("Errors Present");
            }
        }

        [Route("SetPassword"),
        HttpPut,
        ValidateModel]
        public IHttpActionResult SetPassword(UserPasswordChangeModel model)
        {
            if (!model.NewPassword.Equals(model.NewPasswordRepeat, StringComparison.InvariantCulture))
                return BadRequest("Passwords do not match");

            var user = UserManager.FindByName(model.UserName);

            var result = UserManager.ChangePassword(user.Id, model.OldPassword, model.NewPassword);

            if (result.Succeeded)
            {
               

                return Ok(new
                {
                    Name = user.Name,
                    Type = user.Type,                    
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    UserName = user.UserName,
                    Roles = user.Roles,
                    LockedOut = user.LockoutEnabled,
                    EmailConfirmed = user.EmailConfirmed,
                    PhoneNumberConfirmed = user.PhoneNumberConfirmed
                });
            }
            else
            {
                return BadRequest(String.Join(", ", result.Errors));
            }
        }

        [Route("Email"),
        HttpGet]
        public IHttpActionResult Email(string id, string email)
        {
            var user = UserManager.FindById(id);
            user.Email = email;
            if (user == null)
                return NotFound();

            //EmailBroker.Instance.SendUserDetailsEmail(user.UserName, );
            return Ok();
        }

        #region Helpers

        private IAuthenticationManager Authentication
        {
            get { return Request.GetOwinContext().Authentication; }
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

        private class ExternalLoginData
        {
            public string LoginProvider { get; set; }
            public string ProviderKey { get; set; }
            public string UserName { get; set; }

            public IList<Claim> GetClaims()
            {
                IList<Claim> claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.NameIdentifier, ProviderKey, null, LoginProvider));

                if (UserName != null)
                {
                    claims.Add(new Claim(ClaimTypes.Name, UserName, null, LoginProvider));
                }

                return claims;
            }

            public static ExternalLoginData FromIdentity(ClaimsIdentity identity)
            {
                if (identity == null)
                {
                    return null;
                }

                Claim providerKeyClaim = identity.FindFirst(ClaimTypes.NameIdentifier);

                if (providerKeyClaim == null || String.IsNullOrEmpty(providerKeyClaim.Issuer)
                    || String.IsNullOrEmpty(providerKeyClaim.Value))
                {
                    return null;
                }

                if (providerKeyClaim.Issuer == ClaimsIdentity.DefaultIssuer)
                {
                    return null;
                }

                return new ExternalLoginData
                {
                    LoginProvider = providerKeyClaim.Issuer,
                    ProviderKey = providerKeyClaim.Value,
                    UserName = identity.FindFirstValue(ClaimTypes.Name)
                };
            }
        }

        private static class RandomOAuthStateGenerator
        {
            private static RandomNumberGenerator _random = new RNGCryptoServiceProvider();

            public static string Generate(int strengthInBits)
            {
                const int bitsPerByte = 8;

                if (strengthInBits % bitsPerByte != 0)
                {
                    throw new ArgumentException("strengthInBits must be evenly divisible by 8.", "strengthInBits");
                }

                int strengthInBytes = strengthInBits / bitsPerByte;

                byte[] data = new byte[strengthInBytes];
                _random.GetBytes(data);
                return HttpServerUtility.UrlTokenEncode(data);
            }
        }

        #endregion
    }

    public class UserCreationModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Type { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Repeat { get; set; }
        public string[] Roles { get; set; }
    }

    public class UserPasswordChangeModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string OldPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
        [Required]
        public string NewPasswordRepeat { get; set; }
    }
}
