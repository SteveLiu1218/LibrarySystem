using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LibrarySystem.Models
{
    public class BookData
    {
        [DisplayName("書名")]
        [Required(ErrorMessage = "書名必填")]
        public string BOOK_NAME { get; set; }

        [DisplayName("書本分類")]
        [Required(ErrorMessage = "分類必填")]
        public string BOOK_CLASS_ID { get; set; }

        [DisplayName("書本作者")]        
        public string BOOK_AUTHOR { get; set; }
        
        [DisplayName("購買日期")]        
        public DateTime BOOK_BOUGHT_DATE { get; set; }

        [DisplayName("出版社")]        
        public string BOOK_PUBLISHER { get; set; }

        [DisplayName("內容簡介")]
        public string BOOK_NOTE { get; set; }
    }
}