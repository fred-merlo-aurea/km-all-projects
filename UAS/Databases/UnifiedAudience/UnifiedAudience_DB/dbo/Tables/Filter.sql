CREATE TABLE [dbo].[Filter]
(
    [FilterID]          INT      IDENTITY (1, 1) NOT NULL,
	[FilterName]	    VARCHAR(25)			  NOT NULL,
	[ProductID]	        INT					  NOT NULL,
	[FilterDetails]	    TEXT				  NOT NULL,
	[CreatedByUserID]	INT					  NOT NULL,
	[DateCreated]	    DATETIME			  NOT NULL,
	[UpdatedByUserID]	INT					  NULL,
	[DateUpdated]	    DATETIME			  NULL
	CONSTRAINT [PK_Filter] PRIMARY KEY CLUSTERED ([FilterID] ASC) WITH (FILLFACTOR = 80)
);
