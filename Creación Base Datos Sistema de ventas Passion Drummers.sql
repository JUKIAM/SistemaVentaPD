SET LANGUAGE us_english

GO

CREATE DATABASE DBSISTEMA_VENTA

GO

USE DBSISTEMA_VENTA

GO

CREATE TABLE ROL(
IdRol int primary key identity,
Decripcion varchar(50),
FechaRegistro datetime default getdate()
)

go

CREATE TABLE PROVEEDOR(
IdProveedor int primary key identity,
Documento varchar(50),
RazonSocial varchar(50),
Correo varchar(50),
Telefono varchar(50),
Estado bit,
FechaRegistro datetime default getdate()
)

go

CREATE TABLE CLIENTE(
IdCliente int primary key identity,
Documento varchar(50),
NombreCompleto varchar(50),
Correo varchar(50),
Telefono varchar(50),
Estado bit,
FechaRegistro datetime default getdate()
)

go

CREATE TABLE USUARIO(
IdUsuario int primary key identity,
Documento varchar(50),
NombreCompleto varchar(50),
Correo varchar(50),
Clave varchar(50),
IdRol int references ROL(IdRol),
Estado bit,
FechaRegistro datetime default getdate()
)

go

CREATE TABLE CATEGORIA(
IdCategoria int primary key identity,
Descripcion varchar(100),
Estado bit,
FechaRegistro datetime default getdate()
)

go

CREATE TABLE PRODUCTO(
IdProducto int primary key identity,
Codigo varchar(50),
Nombre varchar(50),
Descripcion varchar(50),
IdCategoria int references CATEGORIA(IdCAtegoria),
Stock int not null default 0,
PrecioCompra decimal(10,2) default 0,
PrecioVenta decimal(10,2) default 0,
Estado bit,
FechaRegistro datetime default getdate()
)

go

CREATE TABLE COMPRA(
IdCompra int primary key identity,
IdUsuario int references USUARIO(IdUsuario),
IdProveedor int references PROVEEDOR(IdProveedor),
TipoDocumento varchar(50),
NumeroDocumento varchar(50),
MontoTotal decimal(10,2),
FechaRegistro datetime default getdate()
)

go

CREATE TABLE DETALLE_COMPRA(
IdDetalleCompra int primary key identity,
IdCompra int references COMPRA(IdCompra) ,
IdProducto int references PRODUCTO(IdProducto),
PrecioCompra decimal(10,2) default 0,
PrecioVenta decimal(10,2) default 0,
Cantidad int,
MontoTotal decimal(10,2),
FechaRegistro datetime default getdate()
)

go

CREATE TABLE VENTA(
IdVenta int primary key identity,
IdUsuario int references USUARIO(IdUsuario),
TipoDocumento varchar(50),
NumeroDocumento varchar(50),
DocumentoCliente varchar(50),
NombreCliente varchar(100),
MontoPago decimal(10,2),
MontoCambio decimal(10,2),
MontoTotal decimal(10,2),
FechaRegistro datetime default getdate()
)

go

CREATE TABLE DETALLE_VENTA(
IdDetalleVenta int primary key identity,
IdVenta int references VENTA(IdVenta) ,
IdProducto int references PRODUCTO(IdProducto),
PrecioVenta decimal(10,2),
Cantidad int,
SubTotal decimal(10,2),
FechaRegistro datetime default getdate()
)

go

CREATE TABLE NEGOCIO(
IdNegocio int primary key,
Nombre varchar(60),
RUC varchar(60),
Direccion varchar(60),
Logo varbinary(max) null
)

go

---------------PROCEDIMIENTOS ALMACENADOS-------------------------------------------------------------------------------------

--PROCEDIMIENTOS PARA REGISTRAR USUARIO---------------------------------------------------------------------------------------

create proc SP_REGISTRARUSUARIO(
@Documento VARCHAR(50),
@NombreCompleto varchar(50),
@Correo varchar(50),
@Clave varchar(50),
@IdRol int,
@Estado bit,
@IdUsuarioResultado int output,
@Mensaje varchar(500) output
)
as
begin
	set @IdUsuarioResultado = 0
	set @Mensaje = ''

	if not exists(select *from USUARIO where Documento = @Documento)
	begin
		insert into USUARIO(Documento,NombreCompleto,Correo,Clave,IdRol,Estado) values
		(@Documento,@NombreCompleto,@Correo,@Clave,@IdRol,@Estado)

		set @IdUsuarioResultado = SCOPE_IDENTITY()
	end
	else
		set @Mensaje = 'No se puede repetir el documento para m�s de un usuario'

end

go

--PROCEDIMIENTOS PARA EDITAR USUARIO---------------------------------------------------------------------------------------

create proc SP_EDITARUSUARIO(
@IdUsuario int,
@Documento VARCHAR(50),
@NombreCompleto varchar(50),
@Correo varchar(50),
@Clave varchar(50),
@IdRol int,
@Estado bit,
@Respuesta bit output,
@Mensaje varchar(500) output
)
as
begin
	set @Respuesta = 0
	set @Mensaje = ''

	if not exists(select *from USUARIO where Documento = @Documento and IdUsuario != @IdUsuario)
	begin
		update USUARIO set
		Documento = @Documento,
		NombreCompleto = @NombreCompleto,
		Correo = @Correo,
		Clave = @Clave,
		IdRol = @IdRol,
		Estado = @Estado
		where IdUsuario = @IdUsuario

		set @Respuesta = 1
	end
	else
		set @Mensaje = 'No se puede repetir el documento para m�s de un usuario'

end

go

--PROCEDIMIENTOS PARA ELIMINAR USUARIO---------------------------------------------------------------------------------------

create proc SP_ELIMINARUSUARIO(
@IdUsuario int,
@Respuesta bit output,
@Mensaje varchar(500) output
)
as
begin
	set @Respuesta = 0
	set @Mensaje = ''
	declare @pasoreglas bit = 1

	IF EXISTS (SELECT * FROM COMPRA C
	INNER JOIN USUARIO U ON U.IdUsuario = C.IdUsuario
	WHERE U.IdUsuario = @IdUsuario
	)
	BEGIN
		set @pasoreglas = 0
		set @Respuesta = 0
		set @Mensaje = @Mensaje + 'No se puede eliminar porque el usuario se encuentra relacionado a una compra\n'
	END

	IF EXISTS (SELECT * FROM VENTA V
	INNER JOIN USUARIO U ON U.IdUsuario = V.IdUsuario
	WHERE U.IdUsuario = @IdUsuario
	)
	BEGIN
		set @pasoreglas = 0
		set @Respuesta = 0
		set @Mensaje = @Mensaje + 'No se puede eliminar porque el usuario se encuentra relacionado a una venta\n'
	END

	 if(@pasoreglas = 1)
	 BEGIN
		DELETE FROM USUARIO WHERE IdUsuario = @IdUsuario
		SET @Respuesta = 1
	 END
end

go

-----------------------------------------------------------------------------------------------------------------------

--Consulta para reiniciar el IdUsuario en caso de borrar un usuario
--DBCC CHECKIDENT ([USUARIO], RESEED, 2)
--Consulta para ver que numero tiene el IdUduario
--DBCC CHECKIDENT ([USUARIO], NORESEED)


--PROCEDIMIENTOS PARA CATEGORIA------------------------------------------------------------------------------------------

GO

--PROCEDIMIENTO PARA GUARDAR CATEGORIA-----------------------------------------------------------------------------------

CREATE PROC SP_RegistrarCategoria(
@Descripcion varchar(50),
@Estado bit,
@Resultado int output,
@Mensaje varchar(500) output
)as
begin
	SET @Resultado = 0
	IF NOT EXISTS (SELECT *FROM CATEGORIA WHERE Descripcion = @Descripcion)
	begin
		insert into CATEGORIA(Descripcion, Estado) values (@Descripcion,@Estado)
		set @Resultado = SCOPE_IDENTITY()
	end
	ELSE
		set @Mensaje = 'No se puede repetir la descripcion de una categoria'
end

go

--PROCEDIMIENTO PARA MODIFICAR CATEGORIA----------------------------------------------------------------------------------
CREATE PROC SP_EditarCategoria(
@IdCategoria int,
@Descripcion varchar(50),
@Estado bit,
@Resultado bit output,
@Mensaje varchar(500) output
)
as
begin
	SET @Resultado = 1
	IF NOT EXISTS (SELECT *FROM CATEGORIA WHERE Descripcion = @Descripcion and IdCategoria != @IdCategoria)
		
		update CATEGORIA set
		Descripcion = @Descripcion,
		Estado = @Estado
		where IdCategoria = @IdCategoria
	ELSE
	begin
		SET @Resultado = 0
		set @Mensaje = 'No se puede repetir la descripcion de una categoria'
	end
end

go

--PROCEDIMIENTO PARA ELIMINAR CATEGORIA-----------------------------------------------------------------------------------
CREATE PROC SP_EliminarCategoria(
@IdCategoria int,
@Resultado bit output,
@Mensaje varchar(500) output
)
as
begin
	SET @Resultado = 1
	IF NOT EXISTS (
		SELECT *FROM CATEGORIA c
		inner join PRODUCTO p on p.IdCategoria = c.IdCategoria
		where c.IdCategoria = @IdCategoria
	)begin
		delete top(1) from CATEGORIA where IdCategoria = @IdCategoria
	end	
	ELSE
	begin
		SET @Resultado = 0
		set @Mensaje = 'La categoria se encuentra relacionada a un producto'
	end
end

go



--PROCEDIMIENTO PARA REGISTRAR PRODUCTOS------------------------------------------------------------------------------------

CREATE PROC SP_RegistrarProducto(
@Codigo varchar(20),
@Nombre varchar(30),
@Descripcion varchar (30),
@IdCategoria int,
@Estado bit,
@Resultado int output,
@Mensaje varchar(500) output
)as
begin
	SET @Resultado = 0
	IF NOT EXISTS (SELECT *FROM PRODUCTO WHERE Codigo = @Codigo)
	begin
		INSERT INTO PRODUCTO(Codigo,Nombre,Descripcion,IdCategoria,Estado) VALUES (@Codigo,@Nombre,@Descripcion,@IdCategoria,@Estado)
		SET @Resultado = SCOPE_IDENTITY()
	end
	ELSE
		SET @Mensaje = 'Ya existe un producto con el mismo c�digo'
end

go

--PROCEDIMIENTO PARA MODIFICAR PRODUCTOS---------------------------------------------------------------------------------------

CREATE PROC SP_ModificarProducto(
@IdProducto int,
@Codigo varchar(20),
@Nombre varchar(30),
@Descripcion varchar (30),
@IdCategoria int,
@Estado bit,
@Resultado bit output,
@Mensaje varchar(500) output
)as
begin
	SET @Resultado = 1
	IF NOT EXISTS (SELECT *FROM PRODUCTO WHERE Codigo = @Codigo and IdProducto != @IdProducto)
		UPDATE PRODUCTO SET
		Codigo = @Codigo,
		Nombre = @Nombre,
		Descripcion = @Descripcion,
		IdCategoria = @IdCategoria,
		Estado = @Estado
		WHERE IdProducto = @IdProducto
	ELSE
	begin
		SET @Resultado = 0
		SET @Mensaje = 'Ya existe un producto con el mismo c�digo'
	end
end

go

--PROCEDIMIENTO PARA ELIMINAR PRODUCTOS-------------------------------------------------------------------------------------------

CREATE PROC SP_EliminarProduto(
@IdProducto int,
@Respuesta bit output,
@Mensaje varchar(500) output
)as
begin
	SET @Respuesta = 0
	SET @Mensaje = ''
	DECLARE @pasoreglas bit = 1

	IF EXISTS (SELECT *FROM DETALLE_COMPRA dc
	INNER JOIN PRODUCTO p ON p.IdProducto = dc.IdProducto
	WHERE p.IdProducto = @IdProducto
	)
	BEGIN
		SET @pasoreglas = 0
		SET @Respuesta = 0
		SET @Mensaje = @Mensaje + 'No se puede eliminar porque se encuentra relacionado con una compra\n'
	END

	IF EXISTS (SELECT *FROM DETALLE_VENTA dv
	INNER JOIN PRODUCTO p ON p.IdProducto = dv.IdProducto
	WHERE p.IdProducto = @IdProducto
	)
	BEGIN
		SET @pasoreglas = 0
		SET @Respuesta = 0
		SET @Mensaje = @Mensaje + 'No se puede eliminar porque se encuentra relacionado con una venta\n'
	END

	IF(@pasoreglas = 1)
	BEGIN
		DELETE FROM PRODUCTO WHERE IdProducto = @IdProducto
		SET @Respuesta = 1
	END
end

go


--PROCEDIMIENTO PARA REGISTRAR CLIENTES----------------------------------------------------------------------------------------------

CREATE PROC SP_RegistrarCliente(
@Documento varchar(50),
@NombreCompleto varchar(50),
@Correo varchar(50),
@Telefono varchar(50),
@Estado bit,
@Resultado int output,
@Mensaje varchar(500) output
)as
begin
	SET @Resultado = 0
	DECLARE @IDPERSONA INT
	IF NOT EXISTS (SELECT *FROM CLIENTE WHERE Documento = @Documento)
	BEGIN
		INSERT INTO CLIENTE(Documento,NombreCompleto,Correo,Telefono,Estado) VALUES (
		@Documento,@NombreCompleto,@Correo,@Telefono,@Estado)

		SET @Resultado = SCOPE_IDENTITY()
	END
	ELSE
		SET @Mensaje = 'El n�mero de documento ya existe'
end

go

--PROCEDIMIENTO PARA MODIFICAR CLIENTES--------------------------------------------------------------------------------------------

CREATE PROC SP_ModificarCliente(
@IdCliente int,
@Documento varchar(50),
@NombreCompleto varchar(50),
@Correo varchar(50),
@Telefono varchar(50),
@Estado bit,
@Resultado bit output,
@Mensaje varchar(500) output
)as
begin
	SET @Resultado = 1
	DECLARE @IDPERSONA INT
	IF NOT EXISTS (SELECT *FROM CLIENTE WHERE Documento = @Documento AND IdCliente != @IdCliente)
	begin
		UPDATE CLIENTE SET
		Documento = @Documento,
		NombreCompleto = @NombreCompleto,
		Correo = @Correo,
		Telefono = @Telefono,
		Estado = @Estado
		WHERE IdCliente = @IdCliente
	end
	ELSE
	BEGIN
		SET @Resultado = 0
		SET @Mensaje = 'El n�mero de documento ya existe'
	END
end

go
 
--Consulta para reiniciar el IdCliente en caso de borrar un cliente
--DBCC CHECKIDENT ([CLIENTE], RESEED, 1)
--Consulta para ver que numero tiene el IdUduario
--DBCC CHECKIDENT (CLIENTE, NORESEED)

GO



--PROCEDIMIENTO PARA REGISTRAR PROVEEDORES---------------------------------------------------------------------------------------

CREATE PROC SP_RegistrarProveedor(
@Documento varchar(50),
@RazonSocial varchar(50),
@Correo varchar(50),
@Telefono varchar(50),
@Estado bit,
@Resultado int output,
@Mensaje varchar(500) output
)as
begin
	SET @Resultado = 0
	DECLARE @IDPERSONA INT
	IF NOT EXISTS (SELECT *FROM PROVEEDOR WHERE Documento = @Documento)
	begin
		INSERT INTO PROVEEDOR(Documento,RazonSocial,Correo,Telefono,Estado) VALUES (
		@Documento,@RazonSocial,@Correo,@Telefono,@Estado)

		SET @Resultado = SCOPE_IDENTITY()
	end
	ELSE
		SET @Mensaje = 'El n�mero de documento ya existe'
end

go

--PROCEDIMIENTO PARA MODIFICAR PROVEEDOR--------------------------------------------------------------------------------------

CREATE PROC SP_ModificarProveedor(
@IdProveedor int,
@Documento varchar(50),
@RazonSocial varchar(50),
@Correo varchar(50),
@Telefono varchar(50),
@Estado bit,
@Resultado bit output,
@Mensaje varchar(500) output
)as
begin
	SET @Resultado = 1
	DECLARE @IDPERSONA INT
	IF NOT EXISTS (SELECT *FROM PROVEEDOR WHERE Documento = @Documento AND IdProveedor != @IdProveedor)
	begin
		UPDATE PROVEEDOR SET
		Documento = @Documento,
		RazonSocial = @RazonSocial,
		Correo = @Correo,
		Telefono = @Telefono,
		Estado = @Estado
		WHERE IdProveedor = @IdProveedor
	end
	ELSE
	begin
		SET @Resultado = 0
		SET @Mensaje = 'El n�mero de documento ya existe'
	end
end

go

--PROCEDIMIENTO PARA ELIMINAR PROVEEDOR---------------------------------------------------------------------------------------

CREATE PROC SP_EliminarProveedor(
@IdProveedor int,
@Resultado bit output,
@Mensaje varchar(500) output
)as
begin
	SET @Resultado = 1
	IF NOT EXISTS (
	SELECT *FROM PROVEEDOR p
	INNER JOIN COMPRA c ON p.IdProveedor = c.IdProveedor
	WHERE p.IdProveedor = @IdProveedor
	)
	begin
		DELETE TOP(1) FROM PROVEEDOR WHERE IdProveedor = @IdProveedor
	end
	ELSE
	begin
		SET @Resultado = 0
		SET @Mensaje = 'El proveedor se encuentra relacionado a una compra'
	end
end

go

--Consulta para reiniciar el IdProveedor en caso de borrar un proveedor
--DBCC CHECKIDENT (PROVEEDOR, RESEED, 2)
--Consulta para ver que numero tiene el IdUduario
--DBCC CHECKIDENT (PROVEEDOR, NORESEED)

go

--TYPO DE DATO DETALLE COMPRA----------------------------------------------------------------------------------------

CREATE TYPE [dbo].[EDetalle_Compra] AS TABLE(
	[IdProducto] int null,
	[PrecioCompra] decimal(18,2) null,
	[PrecioVenta] decimal (18,2) null,
	[Cantidad] int null,
	[MontoTotal] decimal(18,2) null
)

GO

--PROCESOS PARA REGISTRAR UNA COMPRA----------------------------------------------------------------------------------------

CREATE PROC SP_RegistrarCompra(
@IdUsuario int,
@IdProveedor int,
@TipoDocumento varchar(500),
@NumeroDocumento varchar(500),
@MontoTotal  decimal(18,2),
@DetalleCompra [EDetalle_Compra] READONLY,
@Resultado bit output,
@Mensaje varchar(500) output
)
as
begin
	begin try
		declare @IdCompra int = 0
		set @Resultado = 1
		set @Mensaje = ''

		begin transaction registro

			insert into COMPRA(IdUsuario,IdProveedor,TipoDocumento,NumeroDocumento,MontoTotal)
			values (@IdUsuario,@IdProveedor,@TipoDocumento,@NumeroDocumento,@MontoTotal)

			set @IdCompra = SCOPE_IDENTITY()

			insert into DETALLE_COMPRA(IdCompra,IdProducto,PrecioCompra,PrecioVenta,Cantidad,MontoTotal)
			select @IdCompra,IdProducto,PrecioCompra,PrecioVenta,Cantidad,MontoTotal from @DetalleCompra

			update p set p.Stock = p.Stock + dc.Cantidad,
			p.PrecioCompra = dc.PrecioCompra,
			p.PrecioVenta = dc.PrecioVenta
			from PRODUCTO p
			inner join @DetalleCompra dc on dc.IdProducto = p.IdProducto

		commit transaction registro

	end try
	begin catch
		
		set @Resultado = 0
		set @Mensaje = ERROR_MESSAGE()
		rollback transaction registro

	end catch
end

go

--TYPO DE DATO DETALLE VENTA------------------------------------------------------------------------------------------

CREATE TYPE [dbo].[EDetalle_Venta] AS TABLE(
	[IdProducto] int null,
	[PrecioVenta] decimal(18,2) null,
	[Cantidad] int null,
	[SubTotal] decimal(18,2) null
)

GO

--PROCESOS PARA REGISTRAR UNA VENTA-------------------------------------------------------------------------------------

CREATE PROC SP_RegistrarVenta(
@IdUsuario int,
@TipoDocumento varchar(500),
@NumeroDocumento varchar(500),
@DocumentoCliente varchar(500),
@NombreCliente varchar(500),
@MontoPago decimal(18,2),
@MontoCambio decimal(18,2),
@MontoTotal decimal(18,2),
@DetalleVenta [EDetalle_Venta] READONLY,
@Resultado bit output,
@Mensaje varchar(500) output
)
AS
BEGIN
	BEGIN TRY
		
		DECLARE @idventa int = 0
		SET @Resultado = 1
		SET @Mensaje = ''

		BEGIN TRANSACTION registrto
			
			INSERT INTO VENTA(IdUsuario,TipoDocumento,NumeroDocumento,DocumentoCliente,NombreCliente,MontoPago,MontoCambio,MontoTotal)
			VALUES(@IdUsuario,@TipoDocumento,@NumeroDocumento,@DocumentoCliente,@NombreCliente,@MontoPago,@MontoCambio,@MontoTotal)

			SET @idventa =  SCOPE_IDENTITY()

			INSERT INTO DETALLE_VENTA(IdVenta,IdProducto,PrecioVenta,Cantidad,SubTotal)
			SELECT @idventa, IdProducto, PrecioVenta, Cantidad, SubTotal FROM @DetalleVenta

		COMMIT TRANSACTION registro

	END TRY
	BEGIN CATCH

		SET @Resultado = 0
		SET @Mensaje = ERROR_MESSAGE()
		ROLLBACK TRANSACTION registro

	END CATCH

END

GO

--PROCEDIMIENTO PARA REPORTE DE COMPRAS-------------------------------------------------------------------------------------

CREATE PROC SP_ReporteCompras(
@fechainicio varchar(10),
@fechafin varchar(10),
@idproveedor int
)
as
begin
	SET DATEFORMAT dmy;
	SELECT 
	CONVERT(char(10),c.FechaRegistro,103)[FechaRegistro],c.TipoDocumento,c.NumeroDocumento,c.MontoTotal,
	u.NombreCompleto[UsuarioComprador],
	pr.Documento[DocumentoProveedor],pr.RazonSocial,
	p.Codigo[CodigoProducto],p.Nombre[NombreProducto],ca.Descripcion[Categoria],dc.PrecioCompra,dc.PrecioVenta,dc.Cantidad,dc.MontoTotal[SubTotal]
	FROM COMPRA c
	INNER JOIN USUARIO u ON u.IdUsuario = c.IdUsuario
	INNER JOIN PROVEEDOR pr ON pr.IdProveedor = c.IdProveedor
	INNER JOIN DETALLE_COMPRA dc ON dc.IdCompra = c.IdCompra
	INNER JOIN PRODUCTO p ON p.IdProducto = dc.IdProducto
	INNER JOIN CATEGORIA ca ON ca.IdCategoria = p.IdCategoria
	WHERE CONVERT(date,c.FechaRegistro) between @fechainicio and @fechafin
	and pr.IdProveedor = iif(@idproveedor = 0, pr.IdProveedor, @idproveedor)

end

GO

--PROCEDIMIENTO PARA REPORTE DE VENTAS-------------------------------------------------------------------------------------

CREATE PROC SP_ReporteVentas(
@fechainicio varchar(10),
@fechafin varchar(10)
)
as
begin
	SET DATEFORMAT dmy;
	SELECT 
	CONVERT(char(10),v.FechaRegistro,103)[FechaRegistro],v.TipoDocumento,v.NumeroDocumento,v.MontoTotal,
	u.NombreCompleto[UsuarioVendedor],
	v.DocumentoCliente,v.NombreCliente,
	p.Codigo[CodigoProducto],p.Nombre[NombreProducto],ca.Descripcion[Categoria],dv.PrecioVenta,dv.PrecioVenta,dv.Cantidad,dv.SubTotal
	FROM VENTA v
	INNER JOIN USUARIO u ON u.IdUsuario = v.IdUsuario
	INNER JOIN DETALLE_VENTA dv ON dv.IdVenta = v.IdVenta
	INNER JOIN PRODUCTO p ON p.IdProducto = dv.IdProducto
	INNER JOIN CATEGORIA ca ON ca.IdCategoria = p.IdCategoria
	WHERE CONVERT(date,v.FechaRegistro) between @fechainicio and @fechafin

end

GO

---------------INSERSIONES-------------------------------------------------------


---------------EMPRESA-----------------------------------------------------------

INSERT INTO NEGOCIO(IdNegocio,Nombre,RUC,Direccion) VALUES (1, 'Passion Drummers', '318294', 'Carrera 19 #13-35')

go

---------------ROLES-------------------------------------------------------------

INSERT INTO ROL(Decripcion) VALUES ('ADMINISTRADOR')
INSERT INTO ROL(Decripcion) VALUES ('EMPLEADO')

GO

---------------USUARIOS-----------------------------------------------------------

INSERT INTO USUARIO(Documento,NombreCompleto,Correo,Clave,IdRol,Estado)
VALUES('1192778347','Julian Garcia','juliandgarcia@unicesar.edu.co','2407',1,1)
INSERT INTO USUARIO(Documento,NombreCompleto,Correo,Clave,IdRol,Estado)
VALUES('1192778348','Andrés Camilo','acamilo@unicesar.edu.co','2407',2,1)

go

---------------CATEGORIAS----------------------------------------------------------

INSERT INTO CATEGORIA(Descripcion,Estado)
VALUES ('Cuerdas',1) /*1*/
INSERT INTO CATEGORIA(Descripcion,Estado)
VALUES ('Teclados',1) /*2*/
INSERT INTO CATEGORIA(Descripcion,Estado) 
VALUES ('Vientos',1) /*3*/
INSERT INTO CATEGORIA(Descripcion,Estado) 
VALUES ('Percusi�n',1) /*4*/
INSERT INTO CATEGORIA(Descripcion,Estado) 
VALUES ('Micr�fonos',1) /*5*/
INSERT INTO CATEGORIA(Descripcion,Estado) 
VALUES ('Sistemas de Audio',1) /*6*/
INSERT INTO CATEGORIA(Descripcion,Estado) 
VALUES ('Accesorios',1) /*7*/
INSERT INTO CATEGORIA(Descripcion,Estado) 
VALUES ('Otros',1) /*8*/

go

---------------PRODUCTOS------------------------------------------------------------

INSERT INTO PRODUCTO(Codigo,Nombre,Descripcion,IdCategoria,Estado)
VALUES('268833','Bajo 4C Fender','SAP Blanco, Diapason en maple',1,1)
INSERT INTO PRODUCTO(Codigo,Nombre,Descripcion,IdCategoria,Estado)
VALUES('295357','Guitarra Electrica Fender','SA Roja, Diapason en laurel',1,1)
INSERT INTO PRODUCTO(Codigo,Nombre,Descripcion,IdCategoria,Estado)
VALUES('327384','Guitarra Electrica Fender','MPL Blanco, Diapason en maple',1,1)
INSERT INTO PRODUCTO(Codigo,Nombre,Descripcion,IdCategoria,Estado)
VALUES('344486','Viola Cenvini','15 Cremona hva-150',1,1)
--------------------------------------------------------------------------------------
INSERT INTO PRODUCTO(Codigo,Nombre,Descripcion,IdCategoria,Estado)
VALUES('355225','Piano Casio CT-X5000','8 Octavas, negro',2,1)
INSERT INTO PRODUCTO(Codigo,Nombre,Descripcion,IdCategoria,Estado)
VALUES('357229','Piano Casio PX-5SWE','9 Octavas, blanco',2,1)
INSERT INTO PRODUCTO(Codigo,Nombre,Descripcion,IdCategoria,Estado)
VALUES('399444','Piano Casio GP-510BP','9 Octavas, negro',2,1)
INSERT INTO PRODUCTO(Codigo,Nombre,Descripcion,IdCategoria,Estado)
VALUES('424773','Piano Casio CT-X3000','8 Octavas, negro/rojo',2,1)
--------------------------------------------------------------------------------------
INSERT INTO PRODUCTO(Codigo,Nombre,Descripcion,IdCategoria,Estado)
VALUES('425588','Acorde�n Horner Rey','VA Rojo',3,1)
INSERT INTO PRODUCTO(Codigo,Nombre,Descripcion,IdCategoria,Estado)
VALUES('456586','Acorde�n Horner III','Corona besas blanco',3,1)
INSERT INTO PRODUCTO(Codigo,Nombre,Descripcion,IdCategoria,Estado)
VALUES('467926','Gaita Corta Milenium','PNHM Blanca',3,1)
INSERT INTO PRODUCTO(Codigo,Nombre,Descripcion,IdCategoria,Estado)
VALUES('476784','Trompeta Jinbao','JBTR-300N Plateada',3,1)
--------------------------------------------------------------------------------------
INSERT INTO PRODUCTO(Codigo,Nombre,Descripcion,IdCategoria,Estado)
VALUES('563362','Bater�a Ludwin Element 5P','5 Piezas, vino',4,1)
INSERT INTO PRODUCTO(Codigo,Nombre,Descripcion,IdCategoria,Estado)
VALUES('575777','Bater�a Ludwin Accent 5P','5 Piezas, roja',4,1)
INSERT INTO PRODUCTO(Codigo,Nombre,Descripcion,IdCategoria,Estado)
VALUES('597757','Conga LP Matador','Custom, rojo crema',4,1)
INSERT INTO PRODUCTO(Codigo,Nombre,Descripcion,IdCategoria,Estado)
VALUES('599973','Timbal LP Prestige','Karl perrazo, negro',4,1)
--------------------------------------------------------------------------------------
INSERT INTO PRODUCTO(Codigo,Nombre,Descripcion,IdCategoria,Estado)
VALUES('627696','Micr�fono Shure PGA58-XLR','Alambrico, negro',5,1)
INSERT INTO PRODUCTO(Codigo,Nombre,Descripcion,IdCategoria,Estado)
VALUES('654587','Micr�fono Prodipe M850','DPS DUO Inalambrico',5,1)
INSERT INTO PRODUCTO(Codigo,Nombre,Descripcion,IdCategoria,Estado)
VALUES('692697','Micr�fono Takstar TS6310PP','Solapa-Diadema Inalambrico',5,1)
INSERT INTO PRODUCTO(Codigo,Nombre,Descripcion,IdCategoria,Estado)
VALUES('735459','Set Micr�fonos Drum Xtuga','7 Piezas, negro',5,1)
--------------------------------------------------------------------------------------
INSERT INTO PRODUCTO(Codigo,Nombre,Descripcion,IdCategoria,Estado)
VALUES('748636','Amplificador QSC','RMX4050A, negro',6,1)
INSERT INTO PRODUCTO(Codigo,Nombre,Descripcion,IdCategoria,Estado)
VALUES('748962','Planta Bajo Fender','Rumble 500 V3',6,1)
INSERT INTO PRODUCTO(Codigo,Nombre,Descripcion,IdCategoria,Estado)
VALUES('777485','Mixer Kohlt Kmix','10 Canales',6,1)
INSERT INTO PRODUCTO(Codigo,Nombre,Descripcion,IdCategoria,Estado)
VALUES('798896','Procesador DBX Driver','Pack PA2',6,1)
--------------------------------------------------------------------------------------
INSERT INTO PRODUCTO(Codigo,Nombre,Descripcion,IdCategoria,Estado)
VALUES('799279','Baqueta Vic Firth','Freestyle 5A Nat',7,1)
INSERT INTO PRODUCTO(Codigo,Nombre,Descripcion,IdCategoria,Estado)
VALUES('799753','Cable Linea Fender Original','4.5 Mts Azul',7,1)
INSERT INTO PRODUCTO(Codigo,Nombre,Descripcion,IdCategoria,Estado)
VALUES('832299','Afinador Fender Bullet','Tuner modo2399',7,1)
INSERT INTO PRODUCTO(Codigo,Nombre,Descripcion,IdCategoria,Estado)
VALUES('857695','Forro Platillo Sabian','24 Pulgadas',7,1)
--------------------------------------------------------------------------------------
INSERT INTO PRODUCTO(Codigo,Nombre,Descripcion,IdCategoria,Estado)
VALUES('867538','Cabeza Mobil Big Dipper','LM70',8,1)
INSERT INTO PRODUCTO(Codigo,Nombre,Descripcion,IdCategoria,Estado)
VALUES('898268','Maquina de Humo Pro Dj','F1500 Led',8,1)
INSERT INTO PRODUCTO(Codigo,Nombre,Descripcion,IdCategoria,Estado)
VALUES('948873','Combo Presonus P/GRAB','AudioBox 96, 2 monitores',8,1)
INSERT INTO PRODUCTO(Codigo,Nombre,Descripcion,IdCategoria,Estado)
VALUES('988326','Auricular Shure SE215','Cl Transparente',8,1)

GO

---------------PROVEEDORES-------------------------------------------------------------

INSERT INTO PROVEEDOR(Documento,RazonSocial,Correo,Telefono,Estado)
VALUES('228688799','Fender','fendersa@gmail.com','7706450',1)
INSERT INTO PROVEEDOR(Documento,RazonSocial,Correo,Telefono,Estado)
VALUES('243933724','Cervini','cervinisa@gmail.com','3474253',1)
INSERT INTO PROVEEDOR(Documento,RazonSocial,Correo,Telefono,Estado)
VALUES('264743948','Casio','casiosa@gmail.com','8002278',1)
INSERT INTO PROVEEDOR(Documento,RazonSocial,Correo,Telefono,Estado)
VALUES('267979422','Horner','hornersa@gmail.com','2786529',1)
INSERT INTO PROVEEDOR(Documento,RazonSocial,Correo,Telefono,Estado)
VALUES('283378963','Milenium','mileniumsa@gmail.com','9332473',1)
INSERT INTO PROVEEDOR(Documento,RazonSocial,Correo,Telefono,Estado)
VALUES('323377997','Jinbao','jinbaosa@gmail.com','4273355',1)
INSERT INTO PROVEEDOR(Documento,RazonSocial,Correo,Telefono,Estado)
VALUES('365238492','Ludwin','ludwinsa@gmail.com','8392923',1)
INSERT INTO PROVEEDOR(Documento,RazonSocial,Correo,Telefono,Estado)
VALUES('422662929','Matador','matadorsa@gmail.com','5466947',1)
INSERT INTO PROVEEDOR(Documento,RazonSocial,Correo,Telefono,Estado)
VALUES('497693967','Prestige','prestigesa@gmail.com','3277696',1)
INSERT INTO PROVEEDOR(Documento,RazonSocial,Correo,Telefono,Estado)
VALUES('529376748','Shure','shuresa@gmail.com','6647548',1)
INSERT INTO PROVEEDOR(Documento,RazonSocial,Correo,Telefono,Estado)
VALUES('637539355','Prodipe','prodipesa@gmail.com','4884457',1)
INSERT INTO PROVEEDOR(Documento,RazonSocial,Correo,Telefono,Estado)
VALUES('684483523','Takstar','takstarsa@gmail.com','9357984',1)
INSERT INTO PROVEEDOR(Documento,RazonSocial,Correo,Telefono,Estado)
VALUES('734246466','Xtuga','xtugasa@gmail.com','5535726',1)
INSERT INTO PROVEEDOR(Documento,RazonSocial,Correo,Telefono,Estado)
VALUES('798336456','Qsc','qscsa@gmail.com','2233562',1)
INSERT INTO PROVEEDOR(Documento,RazonSocial,Correo,Telefono,Estado)
VALUES('684483523','Kmix','kmixsa@gmail.com','9626367',1)
INSERT INTO PROVEEDOR(Documento,RazonSocial,Correo,Telefono,Estado)
VALUES('823996953','Dbx','dbxsa@gmail.com','6798375',1)
INSERT INTO PROVEEDOR(Documento,RazonSocial,Correo,Telefono,Estado)
VALUES('886324523','Vic Firth','vicfirthsa@gmail.com','9357984',1)
INSERT INTO PROVEEDOR(Documento,RazonSocial,Correo,Telefono,Estado)
VALUES('942757227','Sabian','sabiansa@gmail.com','8889386',1)
INSERT INTO PROVEEDOR(Documento,RazonSocial,Correo,Telefono,Estado)
VALUES('947723565','Dipper','dippersa@gmail.com','9225875',1)
INSERT INTO PROVEEDOR(Documento,RazonSocial,Correo,Telefono,Estado)
VALUES('952665382','Pro Dj','prodjsa@gmail.com','9228797',1)
INSERT INTO PROVEEDOR(Documento,RazonSocial,Correo,Telefono,Estado)
VALUES('972294892','Presonus','presonussa@gmail.com','9243446',1)

GO

---------------CLIENTES-------------------------------------------------------------

INSERT INTO CLIENTE(Documento,NombreCompleto,Correo,Telefono,Estado)
VALUES ('2573235533','Loraine Zambrano','lorainez@gmail.com','3432756894',1)
INSERT INTO CLIENTE(Documento,NombreCompleto,Correo,Telefono,Estado)
VALUES ('2225678839','Fabio Veiga','fabiov@gmail.com','3498844422',1)
INSERT INTO CLIENTE(Documento,NombreCompleto,Correo,Telefono,Estado)
VALUES ('2373887655','Angela Llamas','angelall@gmail.com','3733744269',1)
INSERT INTO CLIENTE(Documento,NombreCompleto,Correo,Telefono,Estado)
VALUES ('2665889282','Sonia Diez','soniad@gmail.com','3959953475',1)
INSERT INTO CLIENTE(Documento,NombreCompleto,Correo,Telefono,Estado)
VALUES ('2859257775','Ursula Maza','lorainez@gmail.com','3446422579',1)
INSERT INTO CLIENTE(Documento,NombreCompleto,Correo,Telefono,Estado)
VALUES ('3226437984','Cristiano Ronaldo','cristianor@gmail.com','3537447943',1)
INSERT INTO CLIENTE(Documento,NombreCompleto,Correo,Telefono,Estado)
VALUES ('3273453383','Federico Morillo','federicom@gmail.com','3756925955',1)
INSERT INTO CLIENTE(Documento,NombreCompleto,Correo,Telefono,Estado)
VALUES ('3384778646','Angel Gutierrez','angelg@gmail.com','3965845734',1)
INSERT INTO CLIENTE(Documento,NombreCompleto,Correo,Telefono,Estado)
VALUES ('3747572569','Fernanda Mercado','fernandam@gmail.com','3452269866',1)
INSERT INTO CLIENTE(Documento,NombreCompleto,Correo,Telefono,Estado)
VALUES ('4457822237','Felicidad Olivera','felicidado@gmail.com','3543395894',1)

GO


