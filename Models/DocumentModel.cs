using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterviewScheduler.Models
{
    public class DocumentModel
    {
        public Guid Id { get; internal set; }
        public string Title { get; internal set; }
        public string DocumentType { get; internal set; }
        public Guid DocumentTypeId { get; internal set; }
        public bool IsDeleted { get; internal set; }
        public byte[] Document { get; internal set; }
    }
}