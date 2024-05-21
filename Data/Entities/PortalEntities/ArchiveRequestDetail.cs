﻿using ClientPortal.Models.RequestModels;
using System.ComponentModel.DataAnnotations;

namespace ClientPortal.Data.Entities.PortalEntities
{
    public class ArchiveRequestDetail
    {
        [Key]
        [Required]
        public int ArchiveRequestDetailId { get; set; }
        
        [Required]
        public int ArchiveRequestId { get; set; }
        
        [Required]
        public int TenantId { get; set; }
        
        [Required]
        public int ShopId { get; set; }
        
        [Required]
        public string FileFormat { get; set; }
        
        public string FileName { get; set; }

        [Required]
        public DateTime CreatedDTM { get; set; }
        
        [Required]
        public DateTime LastUpdateDTM { get; set; }


        /// <summary>
        /// 1 = Requested 3 = Complete 4 = failed
        /// </summary>
        [Required]
        public int Status { get; set; } 

        public string? StatusMessage { get; set; }
        
        [Required]
        public bool Active { get; set; }

        public ArchiveRequestHeader ArchiveRequestHeader { get; set; }

        public ArchiveRequestDetail() { }
        public ArchiveRequestDetail(ArchiveReportsRequest report, int headerId)
        {
            ArchiveRequestId = headerId;
            TenantId = (int)report.TenantId!;
            ShopId = (int)report.ShopId!;
            FileFormat = report.FileFormat.FileNameFormat;
            FileName = report.FileName;
            CreatedDTM = DateTime.Now;
            LastUpdateDTM = DateTime.Now;
            Status = 1;
            Active = true;
        }
    }
}
