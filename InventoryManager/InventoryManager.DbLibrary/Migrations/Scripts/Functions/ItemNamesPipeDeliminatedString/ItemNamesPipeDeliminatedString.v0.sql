CREATE OR ALTER FUNCTION [dbo].[ItemNamesPipeDeliminatedString]
(@IsActive BIT)
RETURNS VARCHAR (2500)
AS
BEGIN
	RETURN (SELECT STRING_AGG (Name, '|')
		FROM Items
		WHERE IsActive = @IsActive)
END