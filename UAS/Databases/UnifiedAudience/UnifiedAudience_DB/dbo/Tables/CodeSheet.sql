CREATE TABLE [dbo].[CodeSheet] (
    [CodeSheetID]     INT           IDENTITY (1, 1) NOT NULL,
    [PubID]           INT           NULL,
    [ResponseGroup]   VARCHAR (255) NULL,
    [Responsevalue]   VARCHAR (255) NULL,
    [Responsedesc]    VARCHAR (255) NULL,
    [ResponseGroupID] INT           NULL,
    [DateCreated]     DATETIME      CONSTRAINT [DF_CodeSheet_DateCreated] DEFAULT (getdate()) NULL,
    [DateUpdated]     DATETIME      NULL,
    [CreatedByUserID] INT           NULL,
    [UpdatedByUserID] INT           NULL,
    [DisplayOrder]    INT           NULL,
    [ReportGroupID]   INT           NULL,
    [IsActive]        BIT           NULL,
    [WQT_ResponseID]  INT           NULL,
    [IsOther]         BIT           NULL,
    CONSTRAINT [PK_Mapping] PRIMARY KEY CLUSTERED ([CodeSheetID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_CodeSheet_ResponseGroups] FOREIGN KEY ([ResponseGroupID]) REFERENCES [dbo].[ResponseGroups] ([ResponseGroupID])
);


GO
CREATE NONCLUSTERED INDEX [IDX_CodeSheet_PubID_ResponseGroup]
    ON [dbo].[CodeSheet]([PubID] ASC, [ResponseGroup] ASC) WITH (FILLFACTOR = 90);
GO
CREATE NONCLUSTERED INDEX [IDX_CodeSheet_ResponseGroupID]
    ON [dbo].[CodeSheet]([ResponseGroupID] ASC)
    INCLUDE([CodeSheetID], [Responsevalue]) WITH (FILLFACTOR = 90);
GO
CREATE TRIGGER [dbo].[TR_CodeSheet_Insert]
   ON [dbo].[CodeSheet] 
   AFTER INSERT 
AS  
BEGIN 
   SET NOCOUNT ON;  
   
   create table #ins (id int identity(1,1), responsegroupID int, displayorder int)
   
   insert into #ins (responsegroupID, displayorder)
   Select i.ResponseGroupID, isnull(MAX(mc.displayorder),0) 
   From 
		inserted i left outer join 
		CodeSheet mc on i.ResponseGroupID = mc.ResponseGroupID
	where 
		isnull(i.displayorder, 0) = 0
   group by 
		i.ResponseGroupID
   
   UPDATE mc
   SET displayorder = i1.displayorder + ID
   FROM 
		CodeSheet mc join 
		INSERTED I on mc.CodeSheetID = i.CodeSheetID join 
		#ins i1 on i.ResponseGroupID = i1.ResponseGroupID
   
   drop table #ins
END