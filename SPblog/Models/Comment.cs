using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SPblog.Models
{
    public class Comment
    {

        public int Id { get; set; }
        public int BlogPostId { get; set; } // *is a foreign key so will need virtual property
        public string AuthorId { get; set; }//*
        [AllowHtml]
        public string Body { get; set; }
       
     

        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }
        public string UpdateReason { get; set; }

        public virtual BlogPost BlogPost { get; set; }//*
        public virtual ApplicationUser Author { get; set; }//*
       
    }
}