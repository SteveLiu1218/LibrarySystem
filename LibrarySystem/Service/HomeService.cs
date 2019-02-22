using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LibrarySystem.Models;

namespace LibrarySystem.Service
{
    public class HomeService
    {        
        private string GetDBConnectionString()
        {
            return
                System.Configuration.ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString.ToString();
        }
        public List<HomeBookData> GetAll()
        {
            DataTable dt = new DataTable();
            string sql = @"select BDATA.BOOK_ID as BookId,
                                  BCLASS.BOOK_CLASS_NAME as BookClass,
                                  BDATA.BOOK_NAME as BookName,
                                  BDATA.BOOK_BOUGHT_DATE as BookBoughtDate,
                                  BCODE.CODE_NAME as status,
                                  ISNULL(M.USER_CNAME,'') as BookKeeper
                          from BOOK_DATA BDATA
                          inner join BOOK_CLASS BCLASS 
	                          on BDATA.BOOK_CLASS_ID = BCLASS.BOOK_CLASS_ID
                          inner join BOOK_CODE BCODE 
	                          on BDATA.BOOK_STATUS = BCODE.CODE_ID
                          left join MEMBER_M M 
	                          on BDATA.BOOK_KEEPER = M.USER_ID";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);                
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);

                sqlAdapter.Fill(dt);
                conn.Close();
            }            
            return this.MapHomeBookDataToList(dt);
        }
        public List<HomeBookData> QuerySearchBookData(HomeBookData homeBookData)
        {
            DataTable dt = new DataTable();
            string sql = @"Select BDATA.BOOK_ID as BookId,
                                  BCLASS.BOOK_CLASS_NAME as BookClass,
                                  BDATA.BOOK_NAME as BookName,
                                  BDATA.BOOK_BOUGHT_DATE as BookBoughtDate,
                                  BCODE.CODE_NAME as status,
                                  ISNULL(M.USER_ENAME,'') as BookKeeper
                          from  BOOK_DATA BDATA
                          inner join BOOK_CLASS BCLASS 
	                          on BDATA.BOOK_CLASS_ID = BCLASS.BOOK_CLASS_ID
                          inner join BOOK_CODE BCODE 
	                          on BDATA.BOOK_STATUS = BCODE.CODE_ID
                          left join MEMBER_M M 
	                          on BDATA.BOOK_KEEPER = M.USER_ID
                          where BDATA.BOOK_NAME like ('%' + @BookName + '%') and 
                                (BCLASS.BOOK_CLASS_ID = @BookClass or @BookClass='' )and
                                (M.USER_ENAME = @BookKeeper or @BookKeeper='') and
                                (BCODE.CODE_ID = @status or @status='')";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@BookName", homeBookData.BookName == null ? string.Empty : homeBookData.BookName));
                cmd.Parameters.Add(new SqlParameter("@BookClass", homeBookData.BookClass == null ? string.Empty : homeBookData.BookClass));
                cmd.Parameters.Add(new SqlParameter("@BookKeeper", homeBookData.BookKeeper == null ? string.Empty : homeBookData.BookKeeper));
                cmd.Parameters.Add(new SqlParameter("@status", homeBookData.status == null ? string.Empty : homeBookData.status));                
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);

                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapHomeBookDataToList(dt);
        }
        public void InsertBookData(BookData bookData)
        {
            DataTable dt = new DataTable();
            string sql = @" INSERT INTO BOOK_DATA
						 (
							 BOOK_NAME, BOOK_CLASS_ID, BOOK_AUTHOR, BOOK_BOUGHT_DATE, BOOK_PUBLISHER,BOOK_STATUS                      
						 )
						VALUES
						(
							 @BOOK_NAME,@BOOK_CLASS_ID, @BOOK_AUTHOR, @BOOK_BOUGHT_DATE, @BOOK_PUBLISHER,@BOOK_STATUS                  
						)";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@BOOK_NAME",bookData.BOOK_NAME ));
                cmd.Parameters.Add(new SqlParameter("@BOOK_CLASS_ID", bookData.BOOK_CLASS_ID));
                cmd.Parameters.Add(new SqlParameter("@BOOK_AUTHOR", bookData.BOOK_AUTHOR));
                cmd.Parameters.Add(new SqlParameter("@BOOK_BOUGHT_DATE", bookData.BOOK_BOUGHT_DATE));
                cmd.Parameters.Add(new SqlParameter("@BOOK_PUBLISHER", bookData.BOOK_PUBLISHER));
                cmd.Parameters.Add(new SqlParameter("@BOOK_STATUS", "A"));
                cmd.ExecuteNonQuery();
                conn.Close();
            }            
        }
        public EditBookDetails GetBookDetails(int id)
        {            
            DataTable dt = new DataTable();
            string sql = @"Select BDATA.BOOK_ID as BookId,
                                  BCLASS.BOOK_CLASS_NAME as BookClass,
                                  BDATA.BOOK_AUTHOR as BookAuthor,
                                  BDATA.BOOK_NAME as BookName,
                                  BDATA.BOOK_BOUGHT_DATE as BookBoughtDate,
                                  BCODE.CODE_NAME as status,
                                  ISNULL(M.USER_ENAME,'') as BookKeeper,
								  BDATA.BOOK_PUBLISHER as BookPublisher,
								  BDATA.BOOK_NOTE as BookNote
                          from  BOOK_DATA BDATA
                          inner join BOOK_CLASS BCLASS 
	                          on BDATA.BOOK_CLASS_ID = BCLASS.BOOK_CLASS_ID
                          inner join BOOK_CODE BCODE 
	                          on BDATA.BOOK_STATUS = BCODE.CODE_ID
                          left join MEMBER_M M 
	                          on BDATA.BOOK_KEEPER = M.USER_ID
                          where BDATA.BOOK_ID = @BookId ";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@BookId", id));
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapBookDetails(dt);
        }
        public void EditBookDeatails(EditBookDetails editBookDetails)
        {
            string sql = @"update BOOK_DATA 
                           set    BOOK_NAME = @BookName,
	                              BOOK_AUTHOR= @BookAuthor,
	                              BOOK_PUBLISHER= @BookPublisher,
	                              BOOK_NOTE= @BookNote,
	                              BOOK_BOUGHT_DATE= @BookBoughtDate,
	                              BOOK_CLASS_ID= @BookClassId,
	                              BOOK_STATUS= @BookStatus,
	                              BOOK_KEEPER= @BookKeeper
                           where  BOOK_ID = @BookId	";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.Add(new SqlParameter("@BookName", editBookDetails.BookName ?? string.Empty));
                cmd.Parameters.Add(new SqlParameter("@BookAuthor", editBookDetails.BookAuthor ?? string.Empty));
                cmd.Parameters.Add(new SqlParameter("@BookPublisher", editBookDetails.BookPublisher ?? string.Empty));
                cmd.Parameters.Add(new SqlParameter("@BookNote", editBookDetails.BookNote ?? string.Empty));
                cmd.Parameters.Add(new SqlParameter("@BookBoughtDate", editBookDetails.BookBoughtDate == null ? new DateTime() : editBookDetails.BookBoughtDate));
                cmd.Parameters.Add(new SqlParameter("@BookClassId", editBookDetails.BookClass ?? string.Empty));
                cmd.Parameters.Add(new SqlParameter("@BookStatus", editBookDetails.status ?? string.Empty));
                cmd.Parameters.Add(new SqlParameter("@BookKeeper", editBookDetails.BookKeeper ?? string.Empty));
                cmd.Parameters.Add(new SqlParameter("@BookId", editBookDetails.BookId));
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        public void DeleteBookData(int BookId)
        {
            try
            {
                string sql = "Delete FROM BOOK_DATA Where BOOK_ID = @BookId";
                using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.Add(new SqlParameter("@BookId", BookId));
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Get SelectList Method
        /// <summary>
        /// 找出BookClass SelectLists資料
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetBookClassSelectLists()
        {
            DataTable dt = new DataTable();
            string sql = @"select BOOK_CLASS_ID as BookId,BOOK_CLASS_NAME as BookClassName 
                           from BOOK_CLASS";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapBookClassToList(dt);
        }
        /// <summary>
        /// 找出BookKeeper SelectLists資料
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetBookKeeperSelectLists()
        {
            DataTable dt = new DataTable();
            string sql = "";

            sql = @"select USER_ID as UserId,USER_ENAME as UserEname
                    from MEMBER_M";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapBookKeeperToList(dt);
        }
        /// <summary>
        /// 找出BookStatus SelectLists資料
        /// </summary>
        /// <returns></returns>
        public List<SelectListItem> GetBookStatusSelectLists()
        {
            DataTable dt = new DataTable();
            string sql = "";

            sql = @"select CODE_ID as CodeId ,CODE_NAME as CodeName
                    from BOOK_CODE";
            using (SqlConnection conn = new SqlConnection(this.GetDBConnectionString()))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd);
                sqlAdapter.Fill(dt);
                conn.Close();
            }
            return this.MapBookStatusToList(dt);
        }
        #endregion

        #region Map Method
        /// <summary>
        /// 把 HomeBookData DataTable 轉 List 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private List<HomeBookData> MapHomeBookDataToList(DataTable dt)
        {
            List<HomeBookData> result = new List<HomeBookData>();
            foreach (DataRow row in dt.Rows)
            {
                result.Add(new HomeBookData()
                {
                    BookId = (int)row["BookId"],
                    BookClass = row["BookClass"].ToString(),
                    BookName = row["BookName"].ToString(),
                    BookBoughtDate = (DateTime)row["BookBoughtDate"],
                    status = row["status"].ToString(),
                    BookKeeper = row["BookKeeper"].ToString()
                });
            }
            return result;
        }
        /// <summary>
        /// 把 HomeBookClass DataTable 轉 List 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private List<SelectListItem> MapBookClassToList(DataTable dt)
        {
            List<SelectListItem> result = new List<SelectListItem>();
            foreach (DataRow row in dt.Rows)
            {
                result.Add(new SelectListItem()
                {
                    Text = row["BookClassName"].ToString(),
                    Value = row["BookId"].ToString()
                });
            }
            return result;
        }
        /// <summary>
        /// 把 HomeBookKeeper DataTable 轉 List 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private List<SelectListItem> MapBookKeeperToList(DataTable dt)
        {
            List<SelectListItem> result = new List<SelectListItem>();
            foreach (DataRow row in dt.Rows)
            {
                result.Add(new SelectListItem(){
                    Text = row["UserEname"].ToString(),
                    Value = row["UserId"].ToString()
                });
            }
            return result;
        }
        /// <summary>
        /// 把 HomeBookStatus DataTable 轉 List 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private List<SelectListItem> MapBookStatusToList(DataTable dt)
        {
            List<SelectListItem> result = new List<SelectListItem>();
            foreach (DataRow row in dt.Rows)
            {
                result.Add(new SelectListItem()
                {
                    Text = row["CodeName"].ToString(),
                    Value = row["CodeId"].ToString()
                });
            }
            return result;
        }
        private EditBookDetails MapBookDetails(DataTable dt)
        {
            EditBookDetails editBookDetails = new EditBookDetails();
            foreach (DataRow row in dt.Rows)
            {
                editBookDetails.BookId = (int)row["BookId"];
                editBookDetails.BookClass = row["BookClass"].ToString();
                editBookDetails.BookAuthor = row["BookAuthor"].ToString();
                editBookDetails.BookName = row["BookName"].ToString();
                editBookDetails.BookBoughtDate = (DateTime)row["BookBoughtDate"];
                editBookDetails.status = row["status"].ToString();
                editBookDetails.BookKeeper = row["BookKeeper"].ToString();
                editBookDetails.BookPublisher = row["BookPublisher"].ToString();
                editBookDetails.BookNote = row["BookNote"].ToString();
            }
            return editBookDetails;
        }
        #endregion

    }
}