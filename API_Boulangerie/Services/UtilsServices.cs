using API_Orders.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace API_Orders.Services
{
    public class UtilsServices : IUtilsServices
    {
        private static UtilsServices instance = null;

        public static UtilsServices Instance
        {
            get
            {
                if (instance == null) instance = new UtilsServices();
                return instance;
            }
        }

        public DbResponse<string> ExecSQL(string req)
        {
            Data.DbState _status = Data.DbState.OK;
            string res = null;
            try
            {
                using (var db = new BakeryContext())
                {
                    if (req?.ToLower() == "migration")
                        db.Database.Migrate();
                    else
                        res = db.Database.ExecuteSqlRaw(req).ToString();
                }
            }
            catch (Exception ex)
            {
                _status = Data.DbState.ERROR;
                Log.logger.Error($"[ClientServices - Search()] Erreur lors de la récupération des données : {ex}");
            }

            return new Data.DbResponse<string>(res, _status);
        }
    }
}
