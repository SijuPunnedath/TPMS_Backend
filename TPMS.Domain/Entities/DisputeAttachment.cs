namespace TPMS.Domain.Entities;

public class DisputeAttachment
{
    public int DisputeAttachmentId { get; set; }

    public int DisputeId { get; set; }
    public Dispute Dispute { get; set; }

    public int DocumentId { get; set; } // Your existing DMS
    public string FileName { get; set; }

    public DateTime UploadedAt { get; set; }
}
