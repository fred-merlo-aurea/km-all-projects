CREATE PROCEDURE [dbo].[e_Frequency_Select_All]   

AS
	select FrequencyID, Frequency as FrequencyName, IsDeleted from Frequency where IsDeleted=0