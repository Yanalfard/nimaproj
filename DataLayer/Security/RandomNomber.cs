using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer.Security
{
    public static class RandomNomber
    {
        public static int RandomNo()
        {
            #region random
            int min = 100000;
            int max = 999999;
            Random r = new Random();
            int randomNumber = r.Next(max - min + 1) + min;
            return randomNumber;
        }
        #endregion
    }
}
