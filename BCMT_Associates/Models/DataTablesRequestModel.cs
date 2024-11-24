namespace BCMT_Associates.Models
{
	public class DataTablesRequestModel
	{
		public int draw { get; set; }
		public int start { get; set; }
		public int length { get; set; }
		public SearchModel search { get; set; }
		public List<ColumnModel> columns { get; set; }
		public List<OrderModel> order { get; set; }


        public int? Id { get; set; }
        public int? EmployeeId { get; set; }
        public int? CustomerId { get; set; }
        public int? InvoiceId { get; set; }
		public int? OrganizationId { get; set; }
		public int? OrganizationDetailId { get; set; }
		public int? DealerId { get; set; }
		public int? SubDealerId { get; set; }
		public int? ConnectionTypeId { get; set; }
		public int? AreaId { get; set; }
		public int? CityId { get; set; }
		public int? ZoneId { get; set; }
		public int? PackageId { get; set; }
		public int? PaymentStatusId { get; set; }
		public int? PaymentMethodId { get; set; }
		public int? ExpiredInDays { get; set; }
		public bool LoadAttachment { get; set; }
		public DateTime? StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public string? Module { get; set; }

	}

	public class SearchModel
	{
		public string value { get; set; }
		public bool regex { get; set; }
	}

	public class ColumnModel
	{
		public string data { get; set; }
		public string name { get; set; }
		public bool searchable { get; set; }
		public bool orderable { get; set; }
		public SearchModel search { get; set; }
	}

	public class OrderModel
	{
		public int column { get; set; }
		public string dir { get; set; }
	}
}
