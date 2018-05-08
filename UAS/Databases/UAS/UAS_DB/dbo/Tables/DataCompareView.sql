create table DataCompareView(
    DcViewId		int identity(1,1),
    DcRunId         int           NOT NULL,
    DcTargetCodeId  int           NOT NULL,
	DcTargetIdUad int             NULL,
	UadNetCount		int			  default((0)) NOT NULL,
	MatchedCount	int			  default((0)) NOT NULL,
	NoDataCount		int			  default((0)) NOT NULL,
	Cost			decimal(12,2) default((0)) NOT NULL,
	DateCreated		datetime	  default(GETDATE()) NOT NULL,
	DateUpdated datetime NULL, 
    CreatedByUserID int NOT NULL, 
    UpdatedByUserID int NULL, 
	IsBillable bit default('true') not null,
	Notes varchar(max) null,
	PaymentStatusId int null,
	PaidDate datetime null,
	DcTypeCodeId  int not null,
    CONSTRAINT PK_DataCompareView_DcViewId PRIMARY KEY NONCLUSTERED (DcViewId),
	CONSTRAINT FK_DataCompareView_DataCompareRun FOREIGN KEY (DcRunId) REFERENCES DataCompareRun (DcRunId)
)

