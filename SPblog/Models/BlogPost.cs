using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SPblog.Models
{
    public class BlogPost
    {
       public BlogPost()
        {
            Comments = new HashSet<Comment>(); // will have may comments
        }

        public int Id { get; set; }
        public  string AuthorId { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; } // ? means it can be null
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Abstract { get; set; }
        [AllowHtml]
        public string Body { get; set; }
        public string MediaURL { get; set; }
        public bool Published { get; set; }

        public virtual ApplicationUser Author { get; set; }
        public virtual ICollection<Comment>Comments { get; set; } // Virtual Nav 
        
    }
}