create database proyecto_informatica
go

use proyecto_informatica
go

create table nivelacceso(
	idnivelacceso int primary key identity,
	nombreacceso varchar(60) not null unique,
	fecharegistro date default getdate()
)
go
insert into nivelacceso (nombreacceso) values
	('administrador')
select * from nivelacceso
go

create table usuarios(
	idusuario int primary key identity,
	documento varchar(50) unique not null,
	nombres varchar(150) null,
    apellidos varchar(150) null,
	nombreusuario varchar(50) not null,
	correo varchar(100) not null,
	clave varchar(150) not null,
	idnivelacceso int references nivelacceso(idnivelacceso),
	estado bit not null default 1,
	fecharegistro datetime default getdate()
)
select * from usuarios
select u.idusuario, u.documento, u.nombres, u.apellidos, u.nombreusuario, u.correo, u.clave, u.estado, nv.idnivelacceso, nv.nombreacceso from usuarios u
inner join nivelacceso nv on nv.idnivelacceso = u.idnivelacceso
where u.estado = 1
go

insert into usuarios (documento, nombres, apellidos, nombreusuario, correo, clave, idnivelacceso) values
	('12345678', 'peruba', 'pepopn', 'pepe', 'pepe@hotmail.pe', '12345', 1)

create table categorias(
	idcategoria int primary key identity,
	nombrecategoria varchar(100) not null,
	estado bit not null default 1,
	fecharegistro datetime default getdate()
)
go
select * from categorias
go

create table subcategorias(
	idsubcategorias int primary key identity,
	nombresubcategoria varchar(50) not null,
	idcategoria int references categorias(idcategoria) not null,
	estado bit not null default 1,
	fecharegistro DATETIME DEFAULT GETDATE()
)
go
insert into subcategorias (nombresubcategoria, idcategoria) values
	('Refrigeradoras', 1)
go

select idsubcategorias, c.idcategoria, c.nombrecategoria, nombresubcategoria, sb.estado, CONVERT(VARCHAR(10), sb.fecharegistro, 120)AS fecharegistro_tallas from subcategorias sb
inner join categorias c on c.idcategoria = sb.idcategoria

SELECT sb.idsubcategorias, c.idcategoria, c.nombrecategoria, sb.nombresubcategoria, sb.estado, CONVERT(VARCHAR(10), sb.fecharegistro, 120) AS fecharegistro_tallas FROM subcategorias sb
INNER JOIN categorias c ON c.idcategoria = sb.idcategoria
WHERE c.estado = 1 AND sb.estado = 1 and c.idcategoria = @idcategoria;

select * from subcategorias
go

create table productos(
	idproducto int primary key identity,
	codigo varchar(50) not null,
    nombre varchar(100) not null,
	descripcion varchar(255) not null,
    idcategoria int references categorias(idcategoria) not null,
    idsubcategorias int references subcategorias(idsubcategorias) not null,
	stock int default 0 not null,
    modelo varchar(50) null,
    marca varchar(50) null,
    precioventa decimal(10,2) default 0 not null,
    descuento int default 0,
    garantia varchar(50) null,
    eficiencia_energetica varchar(10) null,
    ubicacion varchar(50) null,
    estado bit not null default 1,
    fecharegistro datetime default getdate()
)
select * from productos
go
INSERT INTO productos (codigo, nombre, descripcion, idcategoria, idsubcategorias, stock, modelo, marca, precioventa, descuento, garantia, eficiencia_energetica, ubicacion) VALUES
	('B23456', 'Aire Acondicionado LG', 'Aire acondicionado de 12000 BTU', 1, 1, '50','AC12000-LG', 'LG', 450.00, 10, '2 años', 'A++', 'Estante B4')
go

SELECT p.idproducto, p.codigo, p.nombre, p.descripcion, c.idcategoria, c.nombrecategoria, sb.idsubcategorias, sb.nombresubcategoria, p.stock, p.modelo, p.marca, p.precioventa, p.descuento, p.garantia, p.eficiencia_energetica, p.ubicacion, p.estado, CONVERT(VARCHAR(10), p.fecharegistro, 120) AS fecharegistro_producto FROM productos p
INNER JOIN categorias c ON p.idcategoria = c.idcategoria
INNER JOIN subcategorias sb ON p.idsubcategorias = sb.idsubcategorias;

select p.codigo + ' ' + p.nombre + ' ' + p.descripcion + ' ' + modelo + ' ' + marca as productos, p.descuento, dv.precioventa, dv.cantidad, dv.subtotal from detalle_venta dv
inner join productos p on p.idproducto = dv.idproducto
where dv.idventa = '00001'


create table ventas(
	idventa int primary key identity,
	idusuario int references usuarios(idusuario),
	tipodocumento varchar(50),
	numerodocumento varchar(50),
	documentocliente varchar(50),
	nombrecliente varchar(100),
	montopago decimal(10,2),
	montocambio decimal(10,2),
	montototal decimal(10,2),
	fecharegistro datetime default getdate()
)
select * from ventas
go

create table detalle_venta(
	iddetalleventa int primary key identity,
	idventa int references ventas(idventa),
	idproducto INT REFERENCES productos(idproducto),
	precioventa decimal(10,2),
	cantidad int,
	subtotal decimal(10,2),
	fecharegistro datetime default getdate()
)
select * from detalle_venta
go

create table negocios(
	idnegocio int primary key,
	nombre varchar(60),
	ruc varchar(60),
	direccion varchar(60),
	logo varbinary(max) NULL
)
go

insert into negocios(idnegocio, nombre, ruc, direccion) values
(1, 'informatica', '2897641', 'barranco');
go

select * from negocios
select logo from negocios where idnegocio = 1
select idnegocio, nombre, ruc, direccion, logo from negocios where idnegocio = 1