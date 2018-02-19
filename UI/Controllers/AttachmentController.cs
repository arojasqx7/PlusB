using Domain.DAL;
using Domain.Entities;
using System.Net.Mime;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace UI.Controllers
{
    public class AttachmentController : Controller
    {
        private PlusBContext db = new PlusBContext();

        [HttpGet]
        public FileResult DownLoadFile(int id, int idTicket)
        {
            List<Attachment> ObjFiles = GetFileList(idTicket);

            var FileById = (from x in ObjFiles
                            where x.Id.Equals(id)
                            select new { x.FileName, x.FileContent }).ToList().FirstOrDefault();
            return File(FileById.FileContent,MediaTypeNames.Application.Octet, FileById.FileName);
        }

        [HttpGet]
        public PartialViewResult FileDetails(int id)
        {
            List<Attachment> attachmentList = GetFileList(id);
            return PartialView("PartialAttachments/FileDetails", attachmentList);
        }

        private List<Attachment> GetFileList(int id)
        {
            List<Attachment> attachmentList = new List<Attachment>();
            attachmentList = db.Attachments.Where(x => x.IdTicket.Equals(id)).ToList();
            return attachmentList;
        }
    }
}