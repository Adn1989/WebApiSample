using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Web.Api.Core.Models;
using Web.Api.Core.ViewModels;

namespace Web.Api.Infrastructure.Interfaces
{
    public interface IDistanceCalculateService
    {
        double Calculate(int appUserId, DistanceCalculateVM distanceCalculate, char unit = 'k');

        Task<IList<AppUserCalculateLog>> GetCalculateLog(int appUserId);
    }
}
