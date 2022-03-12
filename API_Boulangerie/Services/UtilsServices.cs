using API_Boulangerie.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace API_Boulangerie.Services
{
    //public class UtilsServices : IUtilsServices
    //{
    //    public DbResponse<string> ExecSQL(string req)
    //    {
    //        Data.DbState _status = Data.DbState.OK;
    //        string res = null;
    //        try
    //        {
    //            using (var db = new Data.BoulangerieContext())
    //            {
    //                if(req?.ToLower() == "migration")
    //                    db.Database.Migrate();
    //                else
    //                    res = db.Database.ExecuteSqlCommand(req).ToString();
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            _status = Data.DbState.ERROR;
    //            Log.logger.Error($"[ClientServices - Search()] Erreur lors de la récupération des données : {ex}");
    //        }

    //        return new Data.DbResponse<string>(res, _status);
    //    }
    //}
}
