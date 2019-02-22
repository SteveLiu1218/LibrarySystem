﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LibrarySystem.Models
{
    public class EditBookDetails
    {
        public int BookId { get; set; }
        /// <summary>
        /// 圖書類別
        /// </summary>
        [Required(ErrorMessage = "請選擇圖書類別")]
        [DisplayName("圖書類別")]
        public string BookClass { get; set; }

        /// <summary>
        /// 作者
        /// </summary>
        [DisplayName("作者")]
        public string BookAuthor { get; set; }

        /// <summary>
        /// 書名
        /// </summary>
        [DisplayName("書名")]
        public string BookName { get; set; }

        /// <summary>
        /// 購書日期
        /// </summary>        
        [DisplayName("購書日期")]
        public DateTime BookBoughtDate { get; set; }

        /// <summary>
        /// 借閱狀態
        /// </summary>
        [DisplayName("借閱狀態")]
        public string status { get; set; }

        /// <summary>
        /// 借閱人
        /// </summary>
        [DisplayName("借閱人")]
        public string BookKeeper { get; set; }

        /// <summary>
        /// 出版商
        /// </summary>
        [DisplayName("出版商")]
        public string BookPublisher { get; set; }

        /// <summary>
        /// 內容簡介
        /// </summary>
        [DisplayName("內容簡介")]
        public string BookNote { get; set; }
    }
}