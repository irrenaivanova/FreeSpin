namespace FreeSpin.Domain.Common;

public interface IAuditableEntity
{
	DateTime CreatedOn { get; set; }
	DateTime? ModifiedOn { get; set; }
}
