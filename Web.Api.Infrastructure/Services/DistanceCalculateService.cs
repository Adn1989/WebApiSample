using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Web.Api.Core.Dto;
using Web.Api.Core.Models;
using Web.Api.Core.ViewModels;
using Web.Api.Infrastructure.AppDB;
using Web.Api.Infrastructure.Interfaces;

namespace Web.Api.Infrastructure.Services
{
    public class DistanceCalculateService : IDistanceCalculateService
    {
        protected readonly AppDbContext _appDbContext;

        public DistanceCalculateService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        /// <summary>
        /// محاصبه فاصله بین دو مختصات جغرافیایی
        /// </summary>
        /// <param name="distanceCalculate">مختصات جغرافیایی</param>
        /// <param name="unit">واحد اندازه گیری</param>
        /// <returns>فاصله</returns>
        public double Calculate(int appUserId, DistanceCalculateVM distanceCalculate, char unit = 'k')
        {
            double distance;

            try
            {
                if ((distanceCalculate.Lat1 == distanceCalculate.Lat2) && (distanceCalculate.Lon1 == distanceCalculate.Lon2))
                {
                    distance = 0;
                }
                else
                {
                    double theta = distanceCalculate.Lon1 - distanceCalculate.Lon2;
                    double dist = Math.Sin(deg2rad(distanceCalculate.Lat1)) * Math.Sin(deg2rad(distanceCalculate.Lat2)) + Math.Cos(deg2rad(distanceCalculate.Lat1)) * Math.Cos(deg2rad(distanceCalculate.Lat2)) * Math.Cos(deg2rad(theta));
                    dist = Math.Acos(dist);
                    dist = rad2deg(dist);
                    dist = dist * 60 * 1.1515;
                    if (unit == 'K')
                    {
                        dist = dist * 1.609344;
                    }
                    else if (unit == 'N')
                    {
                        dist = dist * 0.8684;
                    }

                    distance = dist;
                }

                CreateCalculateLog(appUserId, distanceCalculate, distance);

                return distance;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// This function converts decimal degrees to radians 
        /// </summary>
        private double deg2rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        /// <summary>
        /// This function converts radians to decimal degrees   
        /// </summary>
        private double rad2deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }

        /// <summary>
        /// ایجاد لاگ درخواست کاربر
        /// </summary>
        /// <param name="appUserId">کد کاربر</param>
        /// <param name="distanceCalculate">مختصات وارد شده</param>
        /// <param name="distance">مسافت محاسبه شده</param>
        private void CreateCalculateLog(int appUserId, DistanceCalculateVM distanceCalculate, double distance)
        {
            try
            {
                AppUserCalculateLog appUserCalculateLog = new AppUserCalculateLog()
                {
                    AppUserID = appUserId,
                    Distance = distance,
                    Lat1 = distanceCalculate.Lat1,
                    Lat2 = distanceCalculate.Lat2,
                    Lon1 = distanceCalculate.Lon1,
                    Lon2 = distanceCalculate.Lon2
                };

                _appDbContext.AppUserCalculateLogs.Add(appUserCalculateLog);

                _appDbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// تاریخچه محاسبات کاربر
        /// </summary>
        /// <param name="appUserId">کد کاربر</param>
        /// <returns>لیست محاسبات</returns>
        public async Task<IList<AppUserCalculateLog>> GetCalculateLog(int appUserId)
        {
            try
            {
                return await _appDbContext.AppUserCalculateLogs.Where(x => x.AppUserID == appUserId).ToAsyncEnumerable().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
