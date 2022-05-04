CREATE OR ALTER PROCEDURE dbo.GetItemsForListing
    @minDate DATETIME = '1970.01.01',
    @maxDate DATETIME = '2050.12.31'
AS
BEGIN
    SET NOCOUNT ON;    

    SELECT item.[Name], item.[Description], item.Notes, item.IsActive, item.IsDeleted, genre.[Name] GenreName, cat.[Name] CategoryName
    FROM dbo.Items item
    LEFT JOIN dbo.ItemGenges ig ON item.Id = ig.ItemId
    LEFT JOIN dbo.Genres genre ON ig.GenreId = genre.Id
    LEFT JOIN dbo.Categories cat ON item.CategoryId = cat.Id
    WHERE (@minDate Is NULL OR item.CreatedDate >= @minDate)
    AND (@maxDate IS NULL OR item.CreatedDate <= @maxDate)
END
GO