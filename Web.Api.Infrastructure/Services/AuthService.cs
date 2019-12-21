using System;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Web.Api.Core.Dto;
using Web.Api.Core.Models;
using Web.Api.Infrastructure.AppDB;
using Web.Api.Infrastructure.Interfaces;

namespace Web.Api.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        const string passphrase = "@DnCwPp1@2";

        protected readonly AppDbContext _appDbContext;

        public AuthService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        /// <summary>
        /// ورود به سیستم
        /// </summary>
        /// <param name="loginRequest">اطلاعات اولیه</param>
        /// <returns>اطلاعات کاربر</returns>
        public async Task<LoginResponse> Login(LoginRequest loginRequest)
        {
            try
            {
                if (!string.IsNullOrEmpty(loginRequest.Email) && !string.IsNullOrEmpty(loginRequest.Password))
                {

                    string passwordHash = EncryptPassword(loginRequest.Password);

                    var user = await _appDbContext.AppUsers.Where(x => x.Email == loginRequest.Email && x.Password == passwordHash).ToAsyncEnumerable().SingleOrDefault();

                    if (user != null)
                    {
                        user.Password = null;
                        return new LoginResponse(user, true);
                    }
                }

                return new LoginResponse(new Error("login_failure", "نام کاربری یا کلمه عبور اشتباه است"), false);

            }
            catch (Exception ex)
            {
                //ex Logger is here
                throw ex;
            }
        }

        /// <summary>
        /// ثبت نام در سایت
        /// </summary>
        /// <param name="appUser">اطلاعات کاربر</param>
        public async Task Register(AppUser appUser)
        {
            try
            {
                var user = await _appDbContext.AppUsers.Where(x => x.Email == appUser.Email).ToAsyncEnumerable().SingleOrDefault();

                if (user != null)
                {
                    throw new DuplicateNameException("این ایمیل قبلا ثبت شده است");
                }

                appUser.Password = EncryptPassword(appUser.Password);

                _appDbContext.AppUsers.Add(appUser);
                await _appDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                //ex Logger is here
                throw ex;
            }
        }

        /// <summary>
        /// رمزینه سازی کلمه عبور
        /// </summary>
        /// <param name="Message"></param>
        private static string EncryptPassword(string Message)
        {
            byte[] encryptResults;

            UTF8Encoding UTF8 = new UTF8Encoding();

            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();

            byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(passphrase));

            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider();

            TDESAlgorithm.Key = TDESKey;

            TDESAlgorithm.Mode = CipherMode.ECB;

            TDESAlgorithm.Padding = PaddingMode.PKCS7;

            byte[] DataToEncrypt = UTF8.GetBytes(Message); /// it will encrypt your message

            try
            {
                ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();

                encryptResults = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
            }
            finally
            {
                TDESAlgorithm.Clear();

                HashProvider.Clear();
            }

            return Convert.ToBase64String(encryptResults);

        }
    }
}
