-- PAGINADOR
CREATE PROCEDURE sp_GetClientesPaginados
    @first INT,
    @rows INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT c.Id, c.Rut, c.Nombre, c.Apellido, c.Email, c.Direccion, c.Telefono, Pais.Nombre as Pais, c.PaisId
    FROM Cliente c
    INNER JOIN Pais ON c.PaisId = Pais.Id
    ORDER BY c.Id DESC
    OFFSET @first ROWS
    FETCH NEXT @rows ROWS ONLY;
END;

-- PAGINADOR + BUSQUEDA + TotalRecords
CREATE PROCEDURE sp_GetClientesConPaginacion
    @first INT = 0, -- Primera fila a devolver (índice 0)
    @rows INT = 10, -- Número de filas por página
    @searchTerm NVARCHAR(100) = NULL -- Parámetro de búsqueda opcional
AS
BEGIN
    -- Intento de pasar el parámetro a int
    DECLARE @searchTermInt INT = TRY_CAST(@searchTerm AS INT);

    -- 1. Contar el total de registros que coinciden con el criterio de búsqueda
    DECLARE @totalRecords INT;
    SELECT 
        @totalRecords = COUNT(*)
    FROM 
        Cliente c
    INNER JOIN 
        Pais ON c.PaisId = Pais.Id
    WHERE 
        (@searchTerm IS NULL OR
            (@searchTermInt IS NOT NULL AND c.Id = @searchTermInt) OR
            c.Rut LIKE '%' + @searchTerm + '%' OR
            c.Nombre LIKE '%' + @searchTerm + '%' OR 
            c.Apellido LIKE '%' + @searchTerm + '%' OR
            c.Direccion LIKE '%' + @searchTerm + '%' OR
            c.Email LIKE '%' + @searchTerm + '%' OR
            c.Telefono LIKE '%' + @searchTerm +'%' OR
            Pais.Nombre LIKE '%' + @searchTerm + '%'
        );

    -- 2. Seleccionar los registros paginados
    SELECT 
        c.Id, 
        c.Rut, 
        c.Nombre, 
        c.Apellido, 
        c.Email, 
        c.Direccion, 
        c.Telefono, 
        Pais.Nombre as Pais, 
        c.PaisId,
        @totalRecords AS TotalRecords -- Incluir el total de registros en el resultado
    FROM 
        Cliente c
    INNER JOIN 
        Pais ON c.PaisId = Pais.Id
    WHERE 
        (@searchTerm IS NULL OR
            (@searchTermInt IS NOT NULL AND c.Id = @searchTermInt) OR
            c.Rut LIKE '%' + @searchTerm + '%' OR
            c.Nombre LIKE '%' + @searchTerm + '%' OR 
            c.Apellido LIKE '%' + @searchTerm + '%' OR
            c.Direccion LIKE '%' + @searchTerm + '%' OR
            c.Email LIKE '%' + @searchTerm + '%' OR
            c.Telefono LIKE '%' + @searchTerm +'%' OR
            Pais.Nombre LIKE '%' + @searchTerm + '%'
        )
    ORDER BY 
        c.Id DESC
    OFFSET 
        @first ROWS
    FETCH NEXT 
        @rows ROWS ONLY;
END;

'solicitud.created_at as FechaCreado',