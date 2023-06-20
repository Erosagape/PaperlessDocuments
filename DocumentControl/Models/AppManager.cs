using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySqlConnector;
namespace DocumentControl.Models
{
    public static class AppManager
    {
        public static string MainConnect => Properties.Settings.Default.MainConnection;
        public static string AppDataFolder => HttpContext.Current.Server.MapPath("~/App_Data");
        public static ExecuteResult TestConnect()
        {
            ExecuteResult cmd = new ExecuteResult();
            cmd.Message = MainConnect;
            try
            {
                using (MySqlConnection cn = new MySqlConnection(MainConnect))
                {
                    cn.Open();
                    cn.Close();
                }
                cmd.isError = false;                
                cmd.Result = "OK";
            }
            catch (Exception e)
            {
                cmd.isError = true;
                cmd.Result = e.Message;
            }
            return cmd;
        }
    }
}