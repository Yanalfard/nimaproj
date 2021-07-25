using DataLayer.Models;
using Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NimaProj.Utilities
{
    public static class Main
    {
        public static TblClient SelectUser(int userId)
        {
            Core _core = new Core();
            TblClient selectUser = _core.Client.GetById(userId);
            return selectUser;
        }
    }
}
