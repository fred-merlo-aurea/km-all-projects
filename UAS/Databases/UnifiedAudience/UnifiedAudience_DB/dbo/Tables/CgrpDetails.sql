CREATE TABLE [dbo].[CgrpDetails] (
    [CgrpDetailsID] INT IDENTITY (1, 1) NOT NULL,
    [Cgrp_no]       INT NOT NULL,
    [MasterID]      INT NOT NULL,
    CONSTRAINT [PK_CgrpDetails] PRIMARY KEY CLUSTERED ([CgrpDetailsID] ASC) WITH (FILLFACTOR = 90)
);
GO
CREATE NONCLUSTERED INDEX [IDX_CgrpDetail_MasterID_Cgrp_no]
    ON [dbo].[CgrpDetails]([Cgrp_no] ASC, [MasterID] ASC) WITH (FILLFACTOR = 90);
GO
CREATE NONCLUSTERED INDEX [IDX_CgrpDetails_Cgrp_no]
    ON [dbo].[CgrpDetails]([Cgrp_no] ASC) WITH (FILLFACTOR = 90);
GO
CREATE NONCLUSTERED INDEX [IDX_CgrpDetails_MasterID]
    ON [dbo].[CgrpDetails]([MasterID] ASC) WITH (FILLFACTOR = 90);
GO