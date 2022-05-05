CREATE OR ALTER FUNCTION [dbo].[GetItemsTotalValue]
(@IsActive BIT = true)
RETURNS TABLE
AS
RETURN (
	SELECT Id, [Name], [Description], Quantity, PurchasePrice, Quantity * PurchasePrice AS TotalValue
	FROM Items
	WHERE IsActive = @IsActive
)