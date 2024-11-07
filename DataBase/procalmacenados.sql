use proyecto_informatica
go

create procedure spu_registrar_usuario(
    @documento VARCHAR(20),
    @nombres VARCHAR(40),
    @apellidos VARCHAR(40),
    @nombreusuario varchar(40),
    @correo varchar(70),
    @clave varchar(50),
    @estado bit,
    @idnivelacceso int,
    @idusuarioresultado int output,
    @mensaje varchar(100) output
)
as
begin
    set @idusuarioresultado = 0
    set @mensaje = ''
    if not exists(select * from usuarios where documento = @documento)
    begin
        insert into usuarios(documento, nombres, apellidos, nombreusuario, correo, clave, estado, idnivelacceso) values
        (@documento, @nombres, @apellidos, @nombreusuario, @correo, @clave, @estado, @idnivelacceso)
        set @idusuarioresultado = scope_identity()
    end
    else
        set @mensaje = 'no se puede repetir el documento de identidad'
    end
go

create PROCEDURE spu_editar_usuario(
    @idusuario int,
    @documento VARCHAR(20),
    @nombres VARCHAR(40),
    @apellidos VARCHAR(40),
    @nombreusuario varchar(40),
    @correo varchar(70),
    @clave varchar(50),
    @estado bit,
    @idnivelacceso int,
    @respuesta int output,
    @mensaje varchar(500) output
)
AS
BEGIN
    SET @respuesta = 0
    SET @mensaje = ''
    if not exists (SELECT * FROM usuarios where documento = @documento and idusuario != @idusuario)
    BEGIN
        UPDATE usuarios SET
        documento = @documento,
        nombres = @nombres,
        apellidos = @apellidos,
        nombreusuario = @nombreusuario,
        correo = @correo,
        clave = @clave,
        estado = @estado,
        idnivelacceso = @idnivelacceso
        WHERE idusuario = @idusuario
        set @respuesta = 1
    END
    ELSE
        SET @mensaje = 'No se puede repetir el mismo dni / carnet de extranjeria para mas de un usuario'
    END
GO

CREATE PROCEDURE spu_eliminar_usuario(
    @idusuario INT,
    @respuesta BIT OUTPUT,
    @mensaje VARCHAR(500) OUTPUT
)
AS
BEGIN
    SET @respuesta = 0
    SET @mensaje = ''
    DECLARE @pasoreglas BIT = 1
    IF EXISTS (SELECT * FROM ventas V 
        inner join usuarios u on u.idusuario = v.idusuario
        WHERE u.idusuario = @idusuario
    )
    BEGIN
        SET @pasoreglas = 0
        SET @respuesta = 0
        SET @mensaje = @mensaje + 'No se puede eliminar porque el usuario se encuentra relacionado a una VENTA\n'
    END
    if(@pasoreglas = 1)
    BEGIN
        delete from usuarios WHERE idusuario = @idusuario
        set @respuesta = 1
    END
END
GO

/*categoria*/
create procedure spu_registrar_categoria(
	@nombrecategoria varchar(50),
	@estado bit,
    @fecharegistro datetime,
	@resultado int output,
	@mensaje varchar(100) output
)
as
begin
	set @resultado = 0
	if not exists (select * from categorias where nombrecategoria = @nombrecategoria)
	begin
		insert into categorias(nombrecategoria, estado, fecharegistro) 
        values (@nombrecategoria, @estado, @fecharegistro)
		set @resultado = SCOPE_IDENTITY()
	end
	else
		set @mensaje = 'No se puede duplicar la categoria'
end
go

create procedure spu_editar_categoria(
	@idcategoria int,
	@nombrecategoria varchar(50),
	@estado bit,
	@resultado bit output,
	@mensaje varchar(100) output
)
as
begin
	SET @resultado = 1
    IF NOT EXISTS (SELECT 1 FROM categorias WHERE nombrecategoria = @nombrecategoria AND idcategoria != @idcategoria)
    BEGIN
        IF (EXISTS (SELECT 1 FROM productos WHERE idcategoria = @idcategoria))
        BEGIN
            SET @resultado = 0
            SET @mensaje = 'No se puede cambiar el estado de la categoría porque está relacionada a productos.'
        END
        ELSE
        BEGIN
            IF (EXISTS (SELECT 1 FROM subcategorias WHERE idcategoria = @idcategoria))
            BEGIN
                SET @resultado = 0
                SET @mensaje = 'No se puede cambiar el estado de la categoría porque está relacionada a tallas.'
            END
            ELSE
            BEGIN
                UPDATE categorias
                SET
                    nombrecategoria = @nombrecategoria,
                    estado = @estado
                WHERE idcategoria = @idcategoria
            END
        END
    END
    ELSE
    BEGIN
        SET @resultado = 0
        SET @mensaje = 'No se puede repetir el nombre de la categoría.'
    END
end
go

create procedure spu_eliminar_categoria(
	@idcategoria int,
	@resultado bit output,
	@mensaje varchar(100) output
)
as
begin
	set @resultado = 1
	if not exists (select *  from categorias c 
		inner join productos p on p.idcategoria = c.idcategoria 
		where c.idcategoria = @idcategoria)
	begin
		delete top(1) from categorias where idcategoria = @idcategoria
	end
	else
		begin
			set @resultado = 0
			set @mensaje = 'La categoria esta relacionada con productos'
		end
end
go
/*fin categoria*/

/*subcategorias*/
create procedure spu_registrar_subcategorias(
	@nombresubcategoria varchar(50),
	@idcategoria int,
	@estado bit,
    @fecharegistro datetime,
	@resultado int output,
	@mensaje varchar(100) output
)
as
begin
	set @resultado = 0
	begin
		insert into subcategorias(idcategoria, nombresubcategoria, estado, fecharegistro) values (@idcategoria, @nombresubcategoria, @estado, @fecharegistro)
		set @resultado = SCOPE_IDENTITY()
	end
end
go

create procedure spu_editar_subcategoria(
	@idsubcategorias int,
	@idcategoria int,
	@nombresubcategoria varchar(50),
	@estado bit,
	@resultado bit output,
	@mensaje varchar(100) output
)
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        SET @resultado = 1;

        UPDATE subcategorias
        SET 
            idcategoria = @idcategoria,
            nombresubcategoria = @nombresubcategoria,
            estado = @estado
        WHERE 
            idsubcategorias = @idsubcategorias;

        SET @mensaje = 'Subcategoría actualizada con éxito.';
    END TRY
    BEGIN CATCH
        SET @resultado = 0;
        SET @mensaje = ERROR_MESSAGE();
    END CATCH;
END;
GO

create procedure spu_eliminar_subcategoria(
	@idsubcategorias int,
	@resultado bit output,
	@mensaje varchar(100) output
)
as
begin
	set @resultado = 1
	if not exists (select *  from subcategorias sb 
		inner join productos pr on pr.idsubcategorias = sb.idsubcategorias
		where sb.idsubcategorias = @idsubcategorias)
	begin
		delete top(1) from subcategorias where idsubcategorias = @idsubcategorias
	end
	else
		begin
			set @resultado = 0
			set @mensaje = 'La subcategoria esta relacionada con uno de los productos y categorias'
		end
end
go
/*fin subcategorias*/

/*productos*/
create procedure spu_registrar_productos(
	@codigo varchar(50),
    @nombre varchar(100),
	@descripcion varchar(255),
    @idcategoria int,
    @idsubcategorias int,
	@stock int,
    @modelo varchar(50),
    @marca varchar(50),
    @precioventa decimal(10,2),
    @descuento int = 0,
    @garantia varchar(50),
    @eficiencia_energetica varchar(10),
    @ubicacion varchar(50),
	@resultado int output,
	@mensaje varchar(100) output
)
as
begin
	set @resultado = 0
	if not exists (select * from productos where codigo = @codigo)
	begin
		insert into productos (codigo, nombre, descripcion, idcategoria, idsubcategorias, stock, modelo, marca, precioventa, descuento, garantia, eficiencia_energetica, ubicacion) VALUES
		(@codigo, @nombre, @descripcion, @idcategoria, @idsubcategorias, @stock, @modelo, @marca, @precioventa, @descuento, @garantia, @eficiencia_energetica, @ubicacion)
		set @resultado = SCOPE_IDENTITY()
	end
	set @mensaje = 'El codigo ya se encuentra registrado en otro producto'
end
go

create procedure spu_editar_producto(
	@idproducto int,
	@codigo varchar(50),
    @nombre varchar(100),
	@descripcion varchar(255),
    @idcategoria int,
    @idsubcategorias int,
	@stock int,
    @modelo varchar(50),
    @marca varchar(50),
    @precioventa decimal(10,2),
    @descuento int = 0,
    @garantia varchar(50),
    @eficiencia_energetica varchar(10),
    @ubicacion varchar(50),
	@resultado int output,
    @mensaje varchar(100) output
)
as
begin
	set @resultado = 1;
	update productos
	set
		codigo = @codigo,
		nombre = @nombre,
		descripcion = @descripcion,
		idcategoria = @idcategoria,
		idsubcategorias = @idsubcategorias,
		stock = @stock,
		modelo = @modelo,
		marca = @marca,
		precioventa = @precioventa,
		descuento = @descuento,
		garantia = @garantia,
		eficiencia_energetica = @eficiencia_energetica,
		ubicacion = @ubicacion
	where idproducto = @idproducto;
	if @@ROWCOUNT = 0
	begin
		set @resultado = 0;
		set @mensaje = 'No se encontro el producto para actualizar';
	end
end
go

create procedure spu_eliminar_productos(
	@idproducto int,
	@respuesta bit output,
	@mensaje varchar(100) output
)
as
begin
    set @respuesta = 0;
    set @mensaje = '';
    declare @pasoreglas bit = 1;
    if exists (
            select *
            from detalle_venta dv
            inner join productos p on p.idproducto = dv.idproducto
            where p.idproducto = @idproducto
        )
    begin
        set @pasoreglas = 0;
        set @respuesta = 0;
        set @mensaje = @mensaje + 'No se puede eliminar porque se encuentra relacionado a una Venta';
    end
        if (@pasoreglas = 1)
    begin
        delete from productos where idproducto = @idproducto;
        set @respuesta = 1;
    end
end;
go
/*fin productos*/

create type [dbo].[EDetalle_Venta] as table(
	[idproducto] int null,
	[precioventa] decimal(10,2) null,
	[cantidad] int null,
	[subtotal] decimal(10,2) null
)
go

create procedure spu_registrar_venta
    @idusuario int,
    @tipodocumento varchar(50),
    @numerodocumento varchar(50),
    @documentocliente varchar(50),
    @nombrecliente varchar(100),
    @montopago decimal(10,2),
    @montocambio decimal(10,2),
    @montototal DECIMAL(10,2),
    @detalleventa [EDetalle_Venta] readonly,
    @resultado bit output,
    @mensaje varchar(100) output
as
begin
    begin try
        declare @idventa int = 0;
        set @resultado = 1;
        set @mensaje = '';
        begin transaction registro;
        insert into ventas(idusuario, tipodocumento, numerodocumento, documentocliente, nombrecliente, montopago, montocambio, montototal)
        values (@idusuario, @tipodocumento, @numerodocumento, @documentocliente, @nombrecliente, @montopago, @montocambio, @montototal);

        set @idventa = SCOPE_IDENTITY();
        insert into detalle_venta(idventa, idproducto, precioventa, cantidad, subtotal)
        select @idventa,
               dv.idproducto,
               dv.precioventa,
               dv.cantidad,
               dv.precioventa * dv.cantidad as subtotal
        from @detalleventa dv
        inner join productos pt on dv.idproducto = pt.idproducto;
        commit transaction registro;
    end try
    begin catch
        set @resultado = 0;
        set @mensaje = ERROR_MESSAGE();
        rollback transaction registro;
    end catch
end
go

create procedure spu_reporte_venta(
    @fechainicio varchar(10),
    @fechafin varchar(10)
)
as
begin
    set dateformat dmy;
    select
        convert(char(10), v.fecharegistro, 103) as FechaRegistro,
        v.tipodocumento,
        v.numerodocumento,
        v.montototal,
        u.nombreusuario as UsuarioRegistro,
        v.documentocliente,
        v.nombrecliente,
        p.codigo as CodigoProducto,
        p.nombre as NombreProducto,
		p.descripcion as Descripcion,
		ca.nombrecategoria as Categoria,
		sb.nombresubcategoria as Subcategoria,
        dv.cantidad,
		p.modelo as Modelo,
		p.marca as Marca,
		dv.precioventa,
        p.descuento as Descuento,
		p.garantia as Garantia,
		p.eficiencia_energetica as Eficiencia,
        dv.subtotal
    from ventas v
    INNER JOIN usuarios u on u.idusuario = v.idusuario
    INNER JOIN detalle_venta dv on dv.idventa = v.idventa
    INNER JOIN productos p on p.idproducto = dv.idproducto
    INNER JOIN categorias ca on ca.idcategoria = p.idcategoria
	INNER JOIN subcategorias sb on p.idsubcategorias = sb.idsubcategorias
    where convert(date, v.fecharegistro, 103) BETWEEN CONVERT(date, @fechainicio, 103) AND CONVERT(date, @fechafin, 103)
END
GO

EXEC spu_reporte_venta '01/08/2024', '07/11/2024';
go

SELECT COUNT(*) FROM ventas;
SELECT COUNT(*) FROM usuarios;
SELECT COUNT(*) FROM productos;
