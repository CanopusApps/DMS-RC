using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TEPL.QDMS.WindowsService.Constants;
using TEPL.QDMS.WindowsService.Log;

namespace TEPL.QDMS.WindowsService.DAL
{
    public class WSAdminDAL
    {
        public DataTable GetApprovalPendingDocuments()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(WSConstants.DBCon))
                {
                    using (SqlCommand cmd = new SqlCommand(WSConstants.spGetApprovalPendingDocuments, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            sda.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerBlock.WriteTraceLog(ex);
            }
            return dt;
        }
        public DataTable GetPublishedDocumentsForDigest(DateTime startDate,DateTime endDate)
        {
            DataTable dt= new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(WSConstants.DBCon))
                {
                    using (SqlCommand cmd = new SqlCommand(WSConstants.spGetPublishedDocumentsForDigest, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@startDate", SqlDbType.DateTime).Value = startDate;
                        cmd.Parameters.Add("@endDate", SqlDbType.DateTime).Value = endDate;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            sda.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerBlock.WriteTraceLog(ex);
            }
            return dt;
        }

        public DataTable GetRevalidationDocuments(DateTime cutOffDate)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(WSConstants.DBCon))
                {
                    using (SqlCommand cmd = new SqlCommand(WSConstants.spGetRevalidationDocuments, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@cutOffDate", SqlDbType.DateTime).Value = cutOffDate;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            sda.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerBlock.WriteTraceLog(ex);
            }
            return dt;
        }

        public DataTable GetApprovalPendingDocumentsHOD()
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(WSConstants.DBCon))
                {
                    using (SqlCommand cmd = new SqlCommand(WSConstants.spGetApprovalPendingDocumentsWITH_HOD, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            sda.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerBlock.WriteTraceLog(ex);
            }
            return dt;
        }

        public DataTable SetIsArchivedForDocuments(string DocumentNo)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection con = new SqlConnection(WSConstants.DBCon))
                {
                    using (SqlCommand cmd = new SqlCommand(WSConstants.spSetArchivedLatePendingDocuments, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@DocumentNo", SqlDbType.NVarChar).Value = DocumentNo;
                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            sda.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerBlock.WriteTraceLog(ex);
            }
            return dt;
        }
    }
}
