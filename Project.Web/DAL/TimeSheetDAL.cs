using Project.Web.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Project.Web.DAL
{
    public class TimeSheetDAL : ITimeClockDAL
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["timetracker"].ConnectionString;
        const string SQL_GetTimeHistory = "Select * from timecard where user_name = @user_name";
        const string SQL_ClockIn = "Insert into timecard values (@user_name, @project, @start_datetime, @end_datetime, @notes)";
        const string SQL_ClockOut = "Update timecard set project = @project, end_datetime = getdate(), notes = @notes where user_name = @user_name and end_datetime is null";
        const string SQL_CanClockIn = "Select * from timecard where user_name = @user_name and end_datetime is null";
        

        public void ClockIn(TimeCard c)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_ClockIn, conn);
                    cmd.Parameters.AddWithValue("@user_name", c.Username);
                    cmd.Parameters.AddWithValue("@project", DBNull.Value);
                    cmd.Parameters.AddWithValue("@start_datetime", DateTime.Now);
                    cmd.Parameters.AddWithValue("@end_datetime", DBNull.Value);
                    cmd.Parameters.AddWithValue("@notes", DBNull.Value);

                    cmd.ExecuteNonQuery();

                    return;
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
        }

        public void ClockOut(TimeCard tc)
        {
            
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_ClockOut, conn);
                    cmd.Parameters.AddWithValue("@user_name", tc.Username);
                    cmd.Parameters.AddWithValue("@project", tc.Project);
                    cmd.Parameters.AddWithValue("@end_datetime", DateTime.Now);
                    cmd.Parameters.AddWithValue("@notes", tc.Notes);
                   

                    cmd.ExecuteNonQuery();

                    return;
                }
            }
            catch (SqlException ex)
            {
                throw;
            }
        }

        public bool CanClockIn(string username)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_CanClockIn, conn);
                    cmd.Parameters.AddWithValue("@user_name", username);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if(reader.Read() == true)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            catch(SqlException ex)
            {
                throw;
            }
        }


        public List<TimeCard> GetTimeCardHistory(string username)
        {
            List<TimeCard> output = new List<TimeCard>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    SqlCommand cmd = new SqlCommand(SQL_GetTimeHistory, conn);
                    cmd.Parameters.AddWithValue("@user_name", username);

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        TimeCard tc = new TimeCard();
                        tc.Username = Convert.ToString(reader["user_name"]);
                        tc.Project = Convert.ToString(reader["project"]);
                        tc.StartDate = Convert.ToDateTime(reader["start_datetime"]);
                        if(reader["end_datetime"] == DBNull.Value)
                        {
                            tc.EndDate = null;
                        }
                        else
                        {
                            tc.EndDate = Convert.ToDateTime(reader["end_datetime"]);
                        }
                        tc.Notes = Convert.ToString(reader["notes"]);

                        output.Add(tc);
                    }
                }

                return output;
            }
            catch (SqlException ex)
            {
                throw;
            }
        }



    }
}