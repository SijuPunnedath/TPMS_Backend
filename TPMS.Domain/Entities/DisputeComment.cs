namespace TPMS.Domain.Entities;

public class DisputeComment
{
    public int DisputeCommentId { get; set; }

    public int DisputeId { get; set; }
    public Dispute Dispute { get; set; }

    public int CommentedByUserId { get; set; }
    public User CommentedByUser { get; set; }

    public string Comment { get; set; }

    public bool IsInternal { get; set; } // Admin-only notes

    public DateTime CreatedAt { get; set; }
}
