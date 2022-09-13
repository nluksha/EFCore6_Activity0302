

DECLARE @id INT
DECLARE @name VARCHAR(50)
DECLARE @firstName VARCHAR(50)
DECLARE @lastName VARCHAR(50)
--Mark Hamill
SET @name = 'Mark Hamill'
SET @firstName = 'Mark'
SET @lastName = 'Hamill'
SET @id = (SELECT ID FROM Players WHERE [Name] = @name)
IF NOT EXISTS (SELECT TOP 1 ID FROM PEOPLE WHERE ID = @id)
BEGIN
	INSERT INTO PEOPLE
	(
		ID, FirstName, LastName
	)
	VALUES
	(
		@id, @firstName, @lastName
	)
END
--Tom Cruise
SET @name = 'Tom Cruise'
SET @firstName = 'Tom'
SET @lastName = 'Cruise'
SET @id = (SELECT ID FROM Players WHERE [Name] = @name)
IF NOT EXISTS (SELECT TOP 1 ID FROM PEOPLE WHERE ID = @id)
BEGIN
	INSERT INTO PEOPLE
	(
		ID, FirstName, LastName
	)
	VALUES
	(
		@id, @firstName, @lastName
	)
END
--Leonardo DiCaprio
SET @name = 'Leonardo DiCaprio'
SET @firstName = 'Leonardo'
SET @lastName = 'DiCaprio'
SET @id = (SELECT ID FROM Players WHERE [Name] = @name)
IF NOT EXISTS (SELECT TOP 1 ID FROM PEOPLE WHERE ID = @id)
BEGIN
	INSERT INTO PEOPLE
	(
		ID, FirstName, LastName
	)
	VALUES
	(
		@id, @firstName, @lastName
	)
END
--Brian L. Gorman
SET @name = 'Brian L. Gorman'
SET @firstName = 'Brian L.'
SET @lastName = 'Gorman'
SET @id = (SELECT ID FROM Players WHERE [Name] = @name)
IF NOT EXISTS (SELECT TOP 1 ID FROM PEOPLE WHERE ID = @id)
BEGIN
	INSERT INTO PEOPLE
	(
		ID, FirstName, LastName
	)
	VALUES
	(
		@id, @firstName, @lastName
	)
END
--Terry Brooks
SET @name = 'Terry Brooks'
SET @firstName = 'Terry'
SET @lastName = 'Brooks'
SET @id = (SELECT ID FROM Players WHERE [Name] = @name)
IF NOT EXISTS (SELECT TOP 1 ID FROM PEOPLE WHERE ID = @id)
BEGIN
	INSERT INTO PEOPLE
	(
		ID, FirstName, LastName
	)
	VALUES
	(
		@id, @firstName, @lastName
	)
END
--Christian Bale
SET @name = 'Christian Bale'
SET @firstName = 'Christian'
SET @lastName = 'Bale'
SET @id = (SELECT ID FROM Players WHERE [Name] = @name)
IF NOT EXISTS (SELECT TOP 1 ID FROM PEOPLE WHERE ID = @id)
BEGIN
	INSERT INTO PEOPLE
	(
		ID, FirstName, LastName
	)
	VALUES
	(
		@id, @firstName, @lastName
	)
END
--Denzel Washington
SET @name = 'Denzel Washington'
SET @firstName = 'Denzel'
SET @lastName = 'Washington'
SET @id = (SELECT ID FROM Players WHERE [Name] = @name)
IF NOT EXISTS (SELECT TOP 1 ID FROM PEOPLE WHERE ID = @id)
BEGIN
	INSERT INTO PEOPLE
	(
		ID, FirstName, LastName
	)
	VALUES
	(
		@id, @firstName, @lastName
	)
END

DECLARE @companyName VARCHAR(150)
DECLARE @stockSymbol VARCHAR(10)
DECLARE @city VARCHAR(50)
--Electronic Arts
SET @name = 'Electronic Arts'
SET @companyName = 'Electronic Arts'
SET @stockSymbol = 'EA'
SET @id = (SELECT ID FROM Players WHERE [Name] = @name)
SET @city = 'Redwood City, CA, USA'
IF NOT EXISTS (SELECT TOP 1 ID FROM COMPANIES WHERE ID = @id)
BEGIN
	INSERT INTO Companies
	(
		ID, CompanyName, StockSymbol, City
	)
	VALUES
	(
		@id, @companyName, @stockSymbol, @city
	)
END
--Wargaming
SET @name = 'Wargaming'
SET @companyName = 'Wargaming'
SET @stockSymbol = 'N/A'
SET @id = (SELECT ID FROM Players WHERE [Name] = @name)
SET @city = 'Nicosia, Cyprus'
IF NOT EXISTS (SELECT TOP 1 ID FROM COMPANIES WHERE ID = @id)
BEGIN
	INSERT INTO Companies
	(
		ID, CompanyName, StockSymbol, City
	)
	VALUES
	(
		@id, @companyName, @stockSymbol, @city
	)
END
