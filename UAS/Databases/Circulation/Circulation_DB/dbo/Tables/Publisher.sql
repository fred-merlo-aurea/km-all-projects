CREATE TABLE [dbo].[Publisher] (
    [PublisherID]     INT          IDENTITY (1, 1) NOT NULL,
    [PublisherName]   VARCHAR (50) NOT NULL,
    [PublisherCode]   VARCHAR (50) NOT NULL,
    [IsActive]        BIT          CONSTRAINT [DF_Publisher_IsActive] DEFAULT ((1)) NOT NULL,
    [HasPaid]         BIT          NOT NULL,
	[ClientID]	      INT		   NOT NULL,
    [DateCreated]     DATETIME     NOT NULL,
    [DateUpdated]     DATETIME     NULL,
    [CreatedByUserID] INT          NOT NULL,
    [UpdatedByUserID] INT          NULL,
    CONSTRAINT [PK_Publisher] PRIMARY KEY CLUSTERED ([PublisherCode] ASC) WITH (FILLFACTOR = 80)
);

