using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySqlConnector;
namespace DocumentControl.Models
{
    public static class AppManager
    {
        public static string MainConnectMY => Properties.Settings.Default.MainConnection;
        public static string MainConnectMS => Properties.Settings.Default.MSSQLConnection;
        public static string AppDataFolder => HttpContext.Current.Server.MapPath("~/App_Data");
        public static System.Data.SqlClient.SqlConnection GetConnection()
        {
            return new System.Data.SqlClient.SqlConnection(MainConnectMS);
        }
        public static ExecuteResult ExecuteDataTable(string sql)
        {
            ExecuteResult cmd = new ExecuteResult();
            cmd.Message = MainConnectMS;
            try
            {
                System.Data.DataTable dt = new System.Data.DataTable();
                using (var cn = GetConnection())
                {
                    cn.Open();
                    using (System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(sql, cn))
                    {
                        da.Fill(dt);
                    }
                    cn.Close();
                }
                cmd.isError = false;
                cmd.Result = dt;
            }
            catch (Exception e)
            {
                cmd.isError = true;
                cmd.Result = e.Message;
            }
            return cmd;
        }
        public static ExecuteResult ExecuteSQL(string sql)
        {
            ExecuteResult cmd = new ExecuteResult();
            cmd.Message = MainConnectMS;
            try
            {
                using (var cn=GetConnection())
                {
                    cn.Open();
                    using (System.Data.SqlClient.SqlCommand cm = new System.Data.SqlClient.SqlCommand(sql, cn))
                    {
                        cm.ExecuteNonQuery();
                    }
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
        public static ExecuteResult TestConnect()
        {
            ExecuteResult cmd = new ExecuteResult();
            cmd.Message = MainConnectMY;
            try
            {
                using (MySqlConnection cn = new MySqlConnection(MainConnectMY))
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